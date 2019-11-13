
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CoderzoneGrapQLAPI.helpers;
using CoderzoneGrapQLAPI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CsharpReference.Services
{
	public enum DATABASE_OPERATION
	{
		CREATE,
		READ,
		UPDATE,
		DELETE
	}

	internal class ParameterReplacer : ExpressionVisitor
	{
		private readonly ParameterExpression _parameter;

		internal ParameterReplacer(ParameterExpression parameter)
		{
			_parameter = parameter;
		}

		protected override Expression VisitParameter
			(ParameterExpression node)
		{
			return _parameter;
		}
	}

	public class SecurityService
	{
		private const bool ALLOW_DEFAULT = false;

		private readonly UserManager<Coder> _userManager;

		public SecurityService(UserManager<Coder> userManager)
		{
			_userManager = userManager;
		}

		public static Expression<Func<TModel, bool>> CreateSecurityFilter<TModel>(
			DATABASE_OPERATION operation,
			IdentityService identityService,
			UserManager<Coder> userManager,
			CoderzoneApiDbContext dbContext)
			where TModel : IOwnerAbstractModel, new()
		{
			identityService.RetrieveUserAsync().Wait();
			var model = new TModel();

			Expression<Func<TModel, bool>> baseRule = _ => false;
			var filter = Expression.OrElse(baseRule.Body, baseRule.Body);

			if (!model.Acls.Any())
			{
				// If we have no rules on this model then we should inherit from the model driven base rule
				Expression<Func<TModel, bool>> defaultFilter = _ => ALLOW_DEFAULT;
				filter = Expression.OrElse(filter, defaultFilter.Body);
			}
			else
			{
				// Otherwise combine the filter on acl with any existing filters
				var securityContext = new SecurityContext
				{
					DbContext = dbContext,
					UserManager = userManager,
					Groups = identityService.Groups,
				};
				IEnumerable<Expression<Func<TModel, bool>>> acls = null;
				switch (operation)
				{
					case DATABASE_OPERATION.READ:
						acls = model.Acls.Select(acl => acl.GetReadConditions<TModel>(identityService.User, securityContext));
						break;
					case DATABASE_OPERATION.UPDATE:
						acls = model.Acls.Select(acl => acl.GetUpdateConditions<TModel>(identityService.User, securityContext));
						break;
					case DATABASE_OPERATION.DELETE:
						acls = model.Acls.Select(acl => acl.GetUpdateConditions<TModel>(identityService.User, securityContext));
						break;
					default:
						break;
				}

				filter = acls.Aggregate(filter, (current, expression) =>
						Expression.OrElse(current, expression.Body));
			}

			var param = Expression.Parameter(typeof(TModel), "model");
			var replacer = new ParameterReplacer(param);

			return Expression.Lambda<Func<TModel, bool>>(replacer.Visit(filter), param);
		}


		public static Expression<Func<TModel, bool>> CreateReadSecurityFilter<TModel>(
			IdentityService identityService,
			UserManager<Coder> userManager,
			CoderzoneApiDbContext dbContext)
			where TModel : IOwnerAbstractModel, new()
		{
			return CreateSecurityFilter<TModel>(DATABASE_OPERATION.READ, identityService, userManager, dbContext);
		}

		public static Expression<Func<TModel, bool>> CreateUpdateSecurityFilter<TModel>(
			IdentityService identityService,
			UserManager<Coder> userManager,
			CoderzoneApiDbContext dbContext)
			where TModel : IOwnerAbstractModel, new()
		{
			return CreateSecurityFilter<TModel>(DATABASE_OPERATION.UPDATE, identityService, userManager, dbContext);
		}

		public static Expression<Func<TModel, bool>> CreateDeleteSecurityFilter<TModel>(
			IdentityService identityService,
			UserManager<Coder> userManager,
			CoderzoneApiDbContext dbContext)
			where TModel : IOwnerAbstractModel, new()
		{
			return CreateSecurityFilter<TModel>(DATABASE_OPERATION.DELETE, identityService, userManager, dbContext);
		}

		public async Task<List<string>> CheckDbSecurityChanges(IdentityService identityService, CoderzoneApiDbContext dbContext)
		{
			await identityService.RetrieveUserAsync();
			var roles = await _userManager.GetRolesAsync(identityService.User);

			var securityContext = new SecurityContext
			{
				DbContext = dbContext,
				UserManager = _userManager,
				Groups = identityService.Groups
			};

			return dbContext.ChangeTracker
				.Entries()
				.Where(entry => entry.Entity is IOwnerAbstractModel)
				.GroupBy(entry => entry.Entity.GetType())
				.ToDictionary(grouping => grouping.Key, grouping =>
				{
					var entityName = grouping.First().Entity.GetType().Name;
					var modelErrors = new List<string>();

					// Even though we have filtered this before we need this check for the compiler
					if (!(grouping.FirstOrDefault()?.Entity is IOwnerAbstractModel model))
					{
						modelErrors.Add("Unknown model type");
						return modelErrors;
					}

					// If the model has no security rules that apply to us then just fall back to the default rule
					var schemes = model.Acls
						.Where(scheme => roles.Contains(scheme.Group) || scheme.Group == null)
						.ToList();
					if (!schemes.Any())
					{
						if (!ALLOW_DEFAULT)
						{
							modelErrors.Add("No applicable schemes for this group");
						}

						return modelErrors;
					}

					// Group on each type of operation that is being performed on the entity
					var entityStates = grouping.GroupBy(entrySet => entrySet.State, (state, entityEntries) =>
					{
						return new
						{
							State = state,
							Entities = entityEntries.Select(e => (IOwnerAbstractModel)e.Entity),
						};
					});

					// For each type of mutation call the rule on the acls that is applicable to that model
					foreach (var state in entityStates)
					{
						// We will always have at least one entry and all entries are the same type so fetch the acl
						// from the first model
						var acls = state.Entities.First().Acls.ToList();

						// Construct a list of auth functions to iterate over
						var authFunctionList = new List<Func<Coder, IEnumerable<IAbstractModel>, SecurityContext, bool>>();
						AddAuthFunctions(authFunctionList, acls, state.State);

						// Iterate over each of the auth functions, and work out weather we have permissions to do this
						// operation. Permissions are additive so we can use a bitwise or to represent the adding of
						// permissions
						var canOperate = false;
						foreach (var func in authFunctionList)
						{
							canOperate |= func(identityService.User, state.Entities, securityContext);

							// Once we hit a true we can break out of the loop since (true | anything == true)
							if (canOperate)
							{
								break;
							}
						}

						// After running the auth functions if we still have no permissions then throw an error message
						if (!canOperate)
						{
							AddError(modelErrors, state.State, entityName);
						}
					}

					return modelErrors;
				})
				.Aggregate(new List<string>(), (list, pair) =>
				{
					// Combine the lists of error messages for each model to a single list of errors ready to return
					list.AddRange(pair.Value);
					return list;
				});
		}

		private static void AddError(ICollection<string> errors, EntityState operation, string entityName)
		{
			errors.Add($"The current user does not have permissions to perform {operation} on {entityName}");
		}

		private static void AddAuthFunctions(ICollection<Func<Coder, IEnumerable<IAbstractModel>, SecurityContext, bool>> funcs, IEnumerable<IAcl> acls, EntityState state)
		{
			switch (state)
			{
				case EntityState.Added:
					foreach (IAcl acl in acls)
					{
						funcs.Add(acl.GetCreate);
					}

					break;
				case EntityState.Modified:
					foreach (IAcl acl in acls)
					{
						funcs.Add(acl.GetUpdate);
					}

					break;
				case EntityState.Deleted:
					foreach (IAcl acl in acls)
					{
						funcs.Add(acl.GetDelete);
					}

					break;
			}
		}
	}
}