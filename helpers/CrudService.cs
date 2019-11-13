using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CoderzoneGrapQLAPI.GraphQL.Mutations.otherTypes;
using CoderzoneGrapQLAPI.Services;
using CsharpReference.Services;
using GraphQL;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Z.EntityFramework.Plus;

namespace CoderzoneGrapQLAPI.helpers
{
	public class CrudService
	{
		private readonly CoderzoneApiDbContext _dbContext;
		private readonly UserManager<Coder> _userManager;
		private readonly SecurityService _securityService;
		private readonly IdentityService _identityService;
		private readonly ILogger<CrudService> _logger;
		private readonly UserService _userService;
		//private readonly AuditService _auditService;

		public CrudService(CoderzoneApiDbContext dbContext, UserManager<Coder> userManager, SecurityService securityService, IdentityService identityService, ILogger<CrudService> logger, UserService userService /* AuditService auditService*/
			)
		{
			_dbContext = dbContext;
			_userManager = userManager;
			_securityService = securityService;
			_identityService = identityService;
			_logger = logger;
			_userService = userService;
			//_auditService = auditService;
		}

		public IQueryable<T> GetById<T>(Guid id)
			where T : class, IOwnerAbstractModel, new()
		{
			return Get<T>().Where(model => model.Id == id);
		}

		public IQueryable<T> Get<T>(Pagination pagination = null, object auditFields = null) where T : class, IOwnerAbstractModel, new()
		{
			_identityService.RetrieveUserAsync().Wait();
			var dbSet = _dbContext.GetDbSet<T>(typeof(T).Name) as IQueryable<T>;
			//_auditService.CreateReadAudit(_identityService.User.Id.ToString(), typeof(T).Name, auditFields);
			return dbSet
				.AddReadSecurityFiltering(_identityService, _userManager, _dbContext)
				.AddPagination(pagination);
		}

		public async Task<ICollection<T>> Create<T>(ICollection<T> models, UpdateOptions options = null) where T : class, IOwnerAbstractModel, new()
		{
			await _identityService.RetrieveUserAsync();
			var dbSet = _dbContext.GetDbSet<T>(typeof(T).Name);

			using (var transaction = _dbContext.Database.BeginTransaction())
			{
				try
				{

					MergeReferences(models, options);

					foreach (var model in models)
					{
						// Update is used here so references are properly handled
						dbSet.Update(model);
					}

					// Ensure that we create all of the base entities instead of updating
					var addedEntries = _dbContext
						.ChangeTracker
						.Entries()
						.Where(entry => models.Contains(entry.Entity));
					foreach (var entry in addedEntries)
					{
						entry.State = EntityState.Added;
					}

					await AssignModelMetaData(_identityService.User);
					ValidateModels(_dbContext.ChangeTracker.Entries().Select(e => e.Entity));

					// % protected region % [Do extra things after create] off begin
					// % protected region % [Do extra things after create] end

					var errors = await _securityService.CheckDbSecurityChanges(_identityService, _dbContext);
					if (errors.Any())
					{
						throw new AggregateException(
							errors.Select(error => new InvalidOperationException(error)));
					}

					await _dbContext.SaveChangesAsync();
					transaction.Commit();

					return models;
				}
				catch (Exception e)
				{
					_logger.LogInformation("Error completing create action - " + e);
					throw;
				}
			}
		}
		public async Task<ICollection<T>> Update<T>(ICollection<T> models, UpdateOptions options = null) where T : class, IOwnerAbstractModel, new()
		{
			await _identityService.RetrieveUserAsync();
			var dbSet = _dbContext.GetDbSet<T>(typeof(T).Name);

			using (var transaction = _dbContext.Database.BeginTransaction())
			{
				try
				{
					MergeReferences(models, options);

					foreach (var model in models)
					{
						dbSet.Update(model);
					}

					await AssignModelMetaData(_identityService.User);
					ValidateModels(_dbContext.ChangeTracker.Entries().Select(e => e.Entity));
					var errors = await _securityService.CheckDbSecurityChanges(_identityService, _dbContext);
					if (errors.Any())
					{
						throw new AggregateException(
							errors.Select(error => new InvalidOperationException(error)));
					}

					await _dbContext.SaveChangesAsync();
					transaction.Commit();

					return models;
				}
				catch (Exception e)
				{
					_logger.LogInformation("Error completing update action - " + e);
					throw;
				}
			}
		}
		public async Task<ICollection<Guid>> Delete<T>(ICollection<Guid> ids)
			where T : class, IAbstractModel
		{
			await _identityService.RetrieveUserAsync();
			var dbSet = _dbContext.GetDbSet<T>(typeof(T).Name);

			var models = dbSet.Where(o => ids.Contains(o.Id));
			dbSet.RemoveRange(models);

			var errors = await _securityService.CheckDbSecurityChanges(_identityService, _dbContext);
			if (errors.Any())
			{
				throw new AggregateException(errors.Select(error => new InvalidOperationException(error)));
			}

			try
			{
				await _dbContext.SaveChangesAsync();
			}
			catch (Exception e)
			{
				var exceptionData = e.InnerException.Data;
				if (exceptionData.Contains("Detail"))
				{
					string errorMessage = exceptionData["Detail"].ToString();
					throw new AggregateException(new InvalidOperationException(errorMessage));
				}
				throw new AggregateException(new InvalidOperationException(e.Message));
			}

			return ids;
		}

		public async Task<BooleanObject> ConditionalUpdate<T>(IQueryable<T> models, MemberInitExpression updateMemberInitExpression)where T : class, IOwnerAbstractModel, new()
		{
			var param = Expression.Parameter(typeof(T), "model");
			var replacer = new ParameterReplacer(param);
			var updateFactory = Expression.Lambda<Func<T, T>>(replacer.Visit(updateMemberInitExpression), param);
			models.AddUpdateSecurityFiltering(_identityService, _userManager, _dbContext).Update(updateFactory);
			var errors = await _securityService.CheckDbSecurityChanges(_identityService, _dbContext);
			if (errors.Any())
			{
				throw new AggregateException(errors.Select(error => new InvalidOperationException(error)));
			}
			await _dbContext.SaveChangesAsync();
			return new BooleanObject { Value = true };
		}

		public async Task<BooleanObject> ConditionalDelete<T>(IQueryable<T> models)
			where T : class, IOwnerAbstractModel, new()
		{
			try
			{
				models.AddDeleteSecurityFiltering(_identityService, _userManager, _dbContext).Delete();
			}
			catch (Exception e)
			{
				var exceptionData = e.Data;
				if (exceptionData.Contains("Detail"))
				{
					string errorMessage = exceptionData["Detail"].ToString();
					throw new AggregateException(new InvalidOperationException(errorMessage));
				}
				throw new AggregateException(new InvalidOperationException(e.Message));
			}

			var errors = await _securityService.CheckDbSecurityChanges(_identityService, _dbContext);
			if (errors.Any())
			{
				throw new AggregateException(errors.Select(error => new InvalidOperationException(error)));
			}
			await _dbContext.SaveChangesAsync();
			return new BooleanObject { Value = true };
		}

		private static void ValidateModels<T>(IEnumerable<T> models)
		{
			var validationExceptions = models.SelectMany(model =>
			{
				var errors = new List<ValidationResult>();
				model.ValidateObjectFields(errors);
				if (errors.Count > 0)
				{
					return new List<ValidationException>
					{
						new ValidationException(string.Join(
							"; ",
							errors.Select(e => e.ErrorMessage).ToArray()))
					};
				}
				return new List<ValidationException>();
			}).ToList();

			if (validationExceptions.Count > 0)
			{
				throw new AggregateException(validationExceptions);
			}
		}

		private async Task AssignModelMetaData(Coder user)
		{
			foreach (var entry in _dbContext.ChangeTracker.Entries<IOwnerAbstractModel>())
			{
				entry.Entity.Modified = DateTime.Now;

				switch (entry.State)
				{
					case EntityState.Added:
						entry.Entity.Created = DateTime.Now;
						entry.Entity.Owner = user.Id;

						// If we haven't been given an id to create against then we need to make a new one
						if (entry.Entity.Id == Guid.Empty)
						{
							entry.Entity.Id = Guid.NewGuid();
						}

						break;
					case EntityState.Modified:
						// Unset fields we don't want to be changed on update
						entry.Property("Owner").IsModified = false;
						entry.Property("Created").IsModified = false;
						break;
				}
			}

			var userProperties = typeof(Coder)
				.GetProperties()
				.Select(p => p.Name)
				.ToList();

			// Users have a concurrency stamp so they need to be pulled from the db and have
			// the concurrency stamp applied to each of the objects we save back to the database
			var userEntries = _dbContext.ChangeTracker
				.Entries<Coder>()
				.ToList();

			// When updating users the core object should not ever change
			foreach (var entry in userEntries)
			{
				switch (entry.State)
				{
					case EntityState.Added:
						// A user should always own themselves
						entry.Entity.Owner = entry.Entity.Id;
						break;
					case EntityState.Modified:
						var databaseProperties = await entry.GetDatabaseValuesAsync();
						var proposedProperties = entry.CurrentValues;

						foreach (var userProperty in userProperties)
						{
							proposedProperties[userProperty] = databaseProperties[userProperty];
						}

						entry.OriginalValues.SetValues(databaseProperties);
						entry.Property("Discriminator").IsModified = false;

						break;
				}
			}
		}

		private void MergeReferences<T>(ICollection<T> models, UpdateOptions options)
			where T : IOwnerAbstractModel, new()
		{
			if (options == null) return;

			var referencesToMerge = typeof(T)
				.GetProperties()
				.SelectMany(prop => prop
					.GetCustomAttributes(typeof(EntityForeignKey), false)
					.Select(attr => attr as EntityForeignKey))
				.Where(attr => options.MergeReferences != null &&
								options.MergeReferences.Contains(attr?.Name, StringComparer.OrdinalIgnoreCase))
				.ToList();

			if (options.MergeReferences != null)
			{
				foreach (var reference in options.MergeReferences)
				{
					try
					{
						var foreignAttribute = referencesToMerge.First(attr => string
							.Equals(attr?.Name, reference, StringComparison.OrdinalIgnoreCase));
						models.First().CleanReference(foreignAttribute.Name, models, _dbContext);
					}
					catch
					{
						// ignored
					}
				}
			}
		}

		private static string ObjectAsString(object obj, IEnumerable<string> properties, string openDelimiter, string closeDelimiter, string separator)
		{
			return string.Join(
				separator,
				properties
					.Select(obj.GetPropertyValue)
					.Select((property) =>
					{
						var propertyString = "";
						switch (property)
						{
							case DateTime dt:
								propertyString = dt.ToIsoString();
								break;
							case null:
								break;
							default:
								propertyString = property.ToString();
								break;
						}
						return openDelimiter + (propertyString) + closeDelimiter;
					})
				);
		}

		private static IEnumerable<string> GetExportProperties<T>()
		{
			var properties = ObjectHelper.GetNonReferenceProperties(typeof(T)).ToArray();
			var propertyNames = properties.Select(p => char.ToLowerInvariant(p.Name[0]) + p.Name.Substring(1)).ToList();
			if (propertyNames.Contains("owner"))
			{
				propertyNames.Remove("owner");
			}

			return propertyNames;
		}

		public async Task<ICollection<Coder>> CreateUser<TModel, TRegisterModel>(ICollection<TRegisterModel> models, UpdateOptions options = null)where TModel : Coder, IOwnerAbstractModel, new() where TRegisterModel : IRegistrationModel<TModel>
		{
			await _identityService.RetrieveUserAsync();

			var dbModels = models.Select(m => m.ToModel()).ToList();
			var dbSet = _dbContext.GetDbSet<TModel>();

			using (var transaction = _dbContext.Database.BeginTransaction())
			{
				try
				{
					MergeReferences(dbModels, options);

					// Create each user
					var createdUsers = new List<Coder>();
					foreach (var registrationModel in models)
					{
						var model = registrationModel.ToModel();

						// Validate the password matches the applications password strength
						var validationTasks = _userManager
							.PasswordValidators
							.Select(v => v.ValidateAsync(_userManager, model, registrationModel.Password));
						var validationResults = await Task.WhenAll(validationTasks);
						var failed = validationResults.Where(r => r.Succeeded == false).ToList();
						if (failed.Any())
						{
							throw new AggregateException(failed
								.SelectMany(f => f.Errors.Select(s => s.Description))
								.Select(s => new InvalidOperationException(s)));
						}

						model.UserName = model.Email;
						model.PasswordHash = _userManager.PasswordHasher.HashPassword(model, registrationModel.Password);
						model.ConcurrencyStamp = await _userManager.GenerateConcurrencyStampAsync(model);
						model.NormalizedEmail = _userManager.NormalizeKey(model.Email);
						model.NormalizedUserName = _userManager.NormalizeKey(model.UserName);
						model.EmailConfirmed = true;
						model.SecurityStamp = Guid.NewGuid().ToString();
						dbSet.Update(model);

						createdUsers.Add(model);
					}

					// Ensure that we create all of the base entities instead of updating
					var addedEntries = _dbContext.ChangeTracker.Entries()
						.Where(entry => createdUsers.Contains(entry.Entity));
					foreach (var entry in addedEntries)
					{
						entry.State = EntityState.Added;
					}

					await AssignModelMetaData(_identityService.User);
					ValidateModels(_dbContext.ChangeTracker.Entries().Select(e => e.Entity));

					foreach (var user in createdUsers)
					{
						user.Owner = user.Id;
					}

					var errors = await _securityService.CheckDbSecurityChanges(_identityService, _dbContext);
					if (errors.Any())
					{
						throw new AggregateException(
							errors.Select(error => new InvalidOperationException(error)));
					}

					await _dbContext.SaveChangesAsync();
					transaction.Commit();

					return createdUsers;
				}
				catch (Exception e)
				{
					_logger.LogInformation("Error completing create user action - " + e);
					throw;
				}
			}
		}

	}


	public class PaginationOptions
	{
		public int? PageNo { get; set; }
		public int? PageSize { get; set; }
	}

	public class Pagination
	{
		public Pagination() { }

		public Pagination(PaginationOptions options)
		{
			PageNo = options.PageNo;
			PageSize = options.PageSize;
		}

		public int? PageSize { get; set; }
		public int? PageNo { get; set; }

		public int? SkipAmount
		{
			get
			{
				if (!isValid())
				{
					return null;
				}
				return (PageNo - 1) * PageSize;
			}
		}

		public bool isValid()
		{
			return PageSize != null && PageSize > 0 && PageNo != null && PageNo > 0;
		}
	}

	public class UpdateOptions
	{
		public IEnumerable<string> MergeReferences { get; set; }
	}
}