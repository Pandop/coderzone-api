using CoderzoneGrapQLAPI.GraphQL.Types;
using CoderzoneGrapQLAPI.Models;
using CoderzoneGrapQLAPI.Services;
using GraphQL;
using GraphQL.Types;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace CoderzoneGrapQLAPI.GraphQL.Mutations
{
	public class CoderzoneApiMutation : ObjectGraphType
	{
		public CoderzoneApiMutation(ICountryRepository countryRepository)
		{
			///TODO:
			FieldAsync<CountryType>(
				Name = "AddCountry",
				arguments: new QueryArguments(new QueryArgument<NonNullGraphType<CountryInputType>> { Name = "country" }),
				resolve: async context=>
				{
					var countryToCreate = context.GetArgument<Country>("country");
					// Make sure country is not already in the database
					var countryInDb = countryRepository.GetCountriesAsync().Result.FirstOrDefault(c=> string.Equals(c.Name, countryToCreate.Name, StringComparison.OrdinalIgnoreCase));
					if(countryInDb !=null)
					{
						context.Errors.Add(new ExecutionError($"The Country '{countryToCreate.Name}' already exists!"));
						return null;
					}
					// Make sure a country was added successfully to database
					if (!await countryRepository.AddCountryAsync(countryToCreate))
					{
						context.Errors.Add(new ExecutionError($"Something went wrong saving {countryToCreate.Name}"));
						return null;
					}
					return countryToCreate;
				}
			);
			FieldAsync<CountryType>(
				Name="UpdateCountry",
				arguments: new QueryArguments(
					new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "countryId" },
					new QueryArgument<NonNullGraphType<CountryInputType>> { Name = "country" }),
				resolve: async context =>
				{
					var countryId = context.GetArgument<Guid>("countryId");
					var countryInfoToUpdate = context.GetArgument<Country>("country");

					// country Ids do not match
					if (countryId != countryInfoToUpdate.Id)
					{
						context.Errors.Add(new ExecutionError($"Bad Request!"));
						return null;
					}
					// Country does not exist in database
					//var ctup = await countryRepository.CountryExistsAsync(countryId);
					var countryInfoToUpdateOld = await countryRepository.GetCountryAsync(countryId);
					if (countryInfoToUpdateOld == null)
					{
						context.Errors.Add(new ExecutionError($"{countryInfoToUpdate.Name} already exists!"));
						return null;
					}
					// Country is a duplicate country
					if (await countryRepository.IsDuplicateCountryNameAsync(countryId, countryInfoToUpdate.Name))
					{
						context.Errors.Add(new ExecutionError($"The Country '{countryInfoToUpdate.Name}' already exists!"));
						return null;
					}

					// Now try to update the country
					//countryInfoToUpdate.Id = countryId;
					countryInfoToUpdateOld.Name = countryInfoToUpdate.Name;
					if (!await countryRepository.UpdateCountryAsync(countryInfoToUpdateOld))
					{
						context.Errors.Add(new ExecutionError($"Something went wrong Updating {countryInfoToUpdate.Name}"));
						return null;
					}
					// If all is good
					return countryInfoToUpdate;
				}
			);
			FieldAsync<CountryType>(
				Name="RemoveCountry",
				arguments: new QueryArguments(new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "countryId" }),
				resolve: async context =>
				{
					var countryId = context.GetArgument<Guid>("countryId");

					var countryToDelete = await countryRepository.GetCountryAsync(countryId);

					// Country does not exist in db
					if (countryToDelete==null)
					{
						context.Errors.Add(new ExecutionError($"Country with ID {countryId} does not exist!"));
						return null;
					}

					// Check that country does not have a state or programmer associated with it
					var states = await countryRepository.GetStatesForCountryAsync(countryId);
					var programmers = await countryRepository.GetUsersForCountryAsync(countryId);
					if (states.Count() > 0 || programmers.Count() > 0)
					{
						context.Errors.Add(new ExecutionError($"{countryToDelete.Name} cannot be deleted as it's used by at least {programmers.Count()} programmers or {states.Count()} states!"));
						return null;
					}

					// At stage, we can delete the country
					if (!await countryRepository.DeleteCountryAsync(countryToDelete))
					{
						context.Errors.Add(new ExecutionError($"Something went wrong deleting {countryToDelete.Name}"));
						return null;
					}
					return countryToDelete;
				}
			);

		}

		public void CsharpReferenceMutations()
		{
			Name = "Mutation";

			// Add input types for each entity
			AddMutationField<CountryInputType, CountryInputType, CountryType, Country>("Country");
			//AddMutationField<SolarSystemInputType, SolarSystemInputType, SolarSystemType, SolarSystem>("SolarSystem");
			//AddMutationField<SectorInputType, SectorInputType, SectorType, Sector>("Sector");
			//AddMutationField<IncidentInputType, IncidentInputType, IncidentType, Incident>("Incident");
			//AddMutationField<ReportInputType, ReportInputType, ReportType, Report>("Report");
			//AddMutationField<StationInputType, StationInputType, StationType, Station>("Station");
			//AddMutationField<ShipCreateInputType, ShipInputType, ShipType, Ship>(
			//	"Ship",
			//	CreateMutation.CreateUserCreateMutation<Ship, ShipRegistrationModel, ShipGraphQlRegistrationModel>("Ship"));
			//AddMutationField<TeamInputType, TeamInputType, TeamType, Team>("Team");
			//AddMutationField<TeamMemberCreateInputType, TeamMemberInputType, TeamMemberType, TeamMember>(
			//	"TeamMember",
			//	CreateMutation.CreateUserCreateMutation<TeamMember, TeamMemberRegistrationModel, TeamMemberGraphQlRegistrationModel>("TeamMember"));
			//AddMutationField<ExpeditionInputType, ExpeditionInputType, ExpeditionType, Expedition>("Expedition");
			//AddMutationField<ObjectiveInputType, ObjectiveInputType, ObjectiveType, Objective>("Objective");
			//AddMutationField<PassiveObserverCreateInputType, PassiveObserverInputType, PassiveObserverType, PassiveObserver>(
			//	"PassiveObserver",
			//	CreateMutation.CreateUserCreateMutation<PassiveObserver, PassiveObserverRegistrationModel, PassiveObserverGraphQlRegistrationModel>("PassiveObserver"));
			//AddMutationField<RobotSlaveInputType, RobotSlaveInputType, RobotSlaveType, RobotSlave>("RobotSlave");

			//// Add input types for each many to many reference
			//AddMutationField<TeamIncidentInputType, TeamIncidentInputType, TeamIncidentType, TeamIncident>("TeamIncident");
			//AddMutationField<OwnerRobotSlaveOfRobotSlaveInputType, OwnerRobotSlaveOfRobotSlaveInputType, OwnerRobotSlaveOfRobotSlaveType, OwnerRobotSlaveOfRobotSlave>("OwnerRobotSlaveOfRobotSlave");

		}

		public void AddMutationField<TModelCreateInputType, TModelUpdateInputType, TModelType, TModel>(
			string name,
			Func<ResolveFieldContext<object>, Task<object>> createMutation = null,
			Func<ResolveFieldContext<object>, Task<object>> updateMutation = null,
			Func<ResolveFieldContext<object>, Task<object>> deleteMutation = null,
			Func<ResolveFieldContext<object>, Task<object>> conditionalUpdateMutation = null,
			Func<ResolveFieldContext<object>, Task<object>> conditionalDeleteMutation = null)
			where TModelCreateInputType : InputObjectGraphType
			where TModelUpdateInputType : InputObjectGraphType
			where TModelType : ObjectGraphType<TModel>
			//where TModel : class, IOwnerAbstractModel, new()
		{
			FieldAsync<ListGraphType<TModelType>>(
				$"create{name}",
				arguments: new QueryArguments(
					new QueryArgument<ListGraphType<TModelCreateInputType>> { Name = name + "s" },
					new QueryArgument<ListGraphType<StringGraphType>> { Name = "MergeReferences" }
				),
				resolve: createMutation ?? CreateMutation.CreateCreateMutation<TModel>(name)
			);

			FieldAsync<ListGraphType<TModelType>>(
				$"update{name}",
				arguments: new QueryArguments(
					new QueryArgument<ListGraphType<TModelUpdateInputType>> { Name = name + "s" },
					new QueryArgument<ListGraphType<StringGraphType>> { Name = "MergeReferences" }
				),
				resolve: updateMutation ?? UpdateMutation.CreateUpdateMutation<TModel>(name)
			);

			FieldAsync<ListGraphType<IdObjectType>>(
				$"delete{name}",
				arguments: new QueryArguments(
					new QueryArgument<ListGraphType<IdGraphType>> { Name = $"{name}Ids" }
				),
				resolve: deleteMutation ?? DeleteMutation.CreateDeleteMutation<TModel>(name)
			);

			FieldAsync<BooleanObjectType>(
				$"update{name}sConditional",
				arguments: new QueryArguments(
					new QueryArgument<IdGraphType> { Name = "id" },
					new QueryArgument<ListGraphType<IdGraphType>> { Name = "ids" },
					new QueryArgument<ListGraphType<ListGraphType<WhereExpressionGraph>>>
					{
						Name = "conditions",
						Description = ConditionalWhereDesc
					},
					new QueryArgument<TModelUpdateInputType> { Name = "valuesToUpdate" },
					new QueryArgument<ListGraphType<StringGraphType>> { Name = "fieldsToUpdate" }
				),
				resolve: conditionalUpdateMutation ?? UpdateMutation.CreateConditionalUpdateMutation<TModel>(name)
			);

			FieldAsync<BooleanObjectType>(
				$"delete{name}sConditional",
				arguments: new QueryArguments(
					new QueryArgument<IdGraphType> { Name = "id" },
					new QueryArgument<ListGraphType<IdGraphType>> { Name = "ids" },
					new QueryArgument<ListGraphType<ListGraphType<WhereExpressionGraph>>>
					{
						Name = "conditions",
						Description = ConditionalWhereDesc
					}
				),
				resolve: conditionalDeleteMutation ?? DeleteMutation.CreateConditionalDeleteMutation<TModel>(name)
			);
		}
	}

	public class CreateMutation
	{
		public static Func<ResolveFieldContext<object>, Task<object>> CreateCreateMutation<TModel>(string name)
			where TModel : class, IOwnerAbstractModel, new()
		{
			return async context =>
			{
				var graphQlContext = (CsharpReferenceGraphQlContext)context.UserContext;
				var crudService = graphQlContext.CrudService;
				var models = context.GetArgument<List<TModel>>(name.ToCamelCase() + "s");
				List<string> mergeReferences = null;

				if (context.HasArgument("mergeReferences"))
				{
					mergeReferences = context.GetArgument<List<string>>("mergeReferences");
				}

				try
				{
					return await crudService.Create(models, new UpdateOptions
					{
						MergeReferences = mergeReferences
					});
				}
				catch (AggregateException exception)
				{
					context.Errors.AddRange(
						exception.InnerExceptions.Select(error => new ExecutionError(error.Message)));
					return new List<TModel>();
				}
			};
		}
		public static Func<ResolveFieldContext<object>, Task<object>> CreateUserCreateMutation<TModel, TRegisterModel, TGraphQlRegisterModel>(string name)
			where TModel : User, IOwnerAbstractModel, new()
			where TRegisterModel : IRegistrationModel<TModel>
			where TGraphQlRegisterModel : TRegisterModel
		{
			return async context =>
			{
				var graphQlContext = (CsharpReferenceGraphQlContext)context.UserContext;
				var crudService = graphQlContext.CrudService;
				var models = context.GetArgument<List<TGraphQlRegisterModel>>(name.ToCamelCase() + "s");
				var mergeReferences = context.GetArgument<List<string>>("mergeReferences");

				try
				{
					return await crudService.CreateUser<TModel, TGraphQlRegisterModel>(models, new UpdateOptions
					{
						MergeReferences = mergeReferences
					});
				}
				catch (AggregateException exception)
				{
					context.Errors.AddRange(
						exception.InnerExceptions.Select(error => new ExecutionError(error.Message)));
					return new List<TModel>();
				}
			};
		}
	}
}

public interface IOwnerAbstractModel : IAbstractModel
{
	Guid Owner { get; set; }
	IEnumerable<IAcl> Acls { get; }

	int CleanReference<T>(string reference, IEnumerable<T> models, CoderzoneApiDbContext dbContext)
		where T : IOwnerAbstractModel;
}

public interface IAcl
{
	string Group { get; }

	Expression<Func<TModel, bool>> GetReadConditions<TModel>(Programmer user, SecurityContext context)
		where TModel : IOwnerAbstractModel, new();
	Expression<Func<TModel, bool>> GetUpdateConditions<TModel>(Programmer user, SecurityContext context)
		where TModel : IOwnerAbstractModel, new();
	Expression<Func<TModel, bool>> GetDeleteConditions<TModel>(Programmer user, SecurityContext context)
		where TModel : IOwnerAbstractModel, new();
	bool GetCreate(Programmer user, IEnumerable<IAbstractModel> models, SecurityContext context);
	bool GetUpdate(Programmer user, IEnumerable<IAbstractModel> models, SecurityContext context);
	bool GetDelete(Programmer user, IEnumerable<IAbstractModel> models, SecurityContext context);

}

public class EntityForeignKey : Attribute
{
	public string Name { get; }
	public string OppositeName { get; }
	public Type OppositeEntity { get; }

	public EntityForeignKey(string name, string oppositeName, Type oppositeEntity)
	{
		Name = name;
		OppositeName = oppositeName;
		OppositeEntity = oppositeEntity;
	}
}

public interface IAbstractModel
{
	Guid Id { get; set; }
	DateTime Created { get; set; }
	DateTime Modified { get; set; }

	// % protected region % [Add any extra abstract model fields here] off begin
	// % protected region % [Add any extra abstract model fields here] end
}

public static class AbstractModelExtensions
{
	public static bool ValidateObjectFields(this object abstractModel, List<ValidationResult> errors)
	{
		var context = new ValidationContext(abstractModel, serviceProvider: null, items: null);
		return Validator.TryValidateObject(abstractModel, context, errors, validateAllProperties: true);
	}
}

public class AbstractModelConfiguration
{
	public static void Configure<T>(EntityTypeBuilder<T> builder)
		where T : class, IAbstractModel
	{
		// Configuration for a POSTGRES database
		builder
			.Property(e => e.Id)
			.HasDefaultValueSql("uuid_generate_v4()");
		builder
			.Property(e => e.Created)
			.HasDefaultValueSql("CURRENT_TIMESTAMP");
		builder
			.Property(e => e.Modified)
			.HasDefaultValueSql("CURRENT_TIMESTAMP");

		// % protected region % [Add any extra abstract model configuration here] off begin
		// % protected region % [Add any extra abstract model configuration here] end
	}
}

public class SecurityContext
{
	public CoderzoneApiDbContext DbContext { get; set; }
	public UserManager<Programmer> UserManager { get; set; }
	public IList<string> Groups { get; set; }
}

public class WhereExpressionGraph : InputObjectGraphType<WhereExpression>
{
	public WhereExpressionGraph();
}

public class CsharpReferenceGraphQlContext
{
	public CoderzoneApiDbContext DbContext { get; set; }
	public Programmer User { get; set; }
	public IList<string> UserGroups { get; set; }
	//public SecurityService SecurityService { get; set; }
	public UserManager<Programmer> UserManager { get; set; }
	//public UserService UserService { get; set; }
	//public CrudService CrudService { get; set; }
	//public IdentityService IdentityService { get; set; }
	//public AuditService AuditService { get; set; }
}

public class GraphQlService
{
	private readonly IDocumentExecuter _executer;
	private readonly ISchema _schema;
	private readonly CoderzoneApiDbContext _dataContext;
	//private readonly SecurityService _securityService;
	private readonly UserManager<Programmer> _userManager;
	//private readonly UserService _userService;
	//private readonly CrudService _crudService;
	//private readonly IdentityService _identityService;
	//private readonly AuditService _auditService;

	public GraphQlService(
		ISchema schema,
		IDocumentExecuter executer,
		CoderzoneApiDbContext dataContext,
		//SecurityService securityService,
		//UserManager<User> userManager,
		//UserService userService,
		//CrudService crudService,
		//IdentityService identityService,
		//AuditService auditService)
	{
		_schema = schema;
		_executer = executer;
		_dataContext = dataContext;
		//_securityService = securityService;
		//_userManager = userManager;
		//_userService = userService;
		//_crudService = crudService;
		//_identityService = identityService;
		//_auditService = auditService;
	}

	public async Task<ExecutionResult> Execute(
		string query,
		string operationName,
		JObject variables,
		Programmer user,
		CancellationToken cancellation)
	{
		var executionOptions = new ExecutionOptions
		{
			Schema = _schema,
			Query = query,
			OperationName = operationName,
			Inputs = variables?.ToInputs(),
			UserContext = new CsharpReferenceGraphQlContext
			{
				DbContext = _dataContext,
				User = user,
				UserGroups = await _userManager.GetRolesAsync(user),
				//SecurityService = _securityService,
				//CrudService = _crudService,
				//IdentityService = _identityService,
				UserManager = _userManager,
				//UserService = _userService,
				//AuditService = _auditService,
			},
			CancellationToken = cancellation,
#if (DEBUG)
			ExposeExceptions = true,
			EnableMetrics = true,
#endif
		};

		var result = await _executer.ExecuteAsync(executionOptions)
			.ConfigureAwait(false);

		return result;
	}
}

public class UpdateMutation
{
	/// <summary>
	/// Creates a mutation that will update entities in the database
	/// </summary>
	/// <param name="name">The name of the model to update</param>
	/// <typeparam name="TModel">The type of the model to update</typeparam>
	/// <returns>A function that takes a graphql context and returns a list of the updated models</returns>
	public static Func<ResolveFieldContext<object>, Task<object>> CreateUpdateMutation<TModel>(string name)
		where TModel : class, IOwnerAbstractModel, new()
	{
		return async context =>
		{
			var graphQlContext = (CsharpReferenceGraphQlContext)context.UserContext;
			var crudService = graphQlContext.CrudService;
			var models = context.GetArgument<List<TModel>>(name.ToCamelCase() + "s");
			var mergeReferences = context.GetArgument<List<string>>("mergeReferences");

			try
			{
				return await crudService.Update(models, new UpdateOptions
				{
					MergeReferences = mergeReferences
				});
			}
			catch (AggregateException exception)
			{
				context.Errors.AddRange(exception.InnerExceptions.Select(error => new ExecutionError(error.Message)));
				return new List<TModel>();
			}
		};
	}
	public class UpdateOptions
	{
		public IEnumerable<string> MergeReferences { get; set; }
	}

	/// <summary>
	/// Creates a mutation that will update things from the database by a where condition
	/// </summary>
	/// <param name="name">The name of the model to update</param>
	/// <typeparam name="TModel">The type of the model to update</typeparam>
	/// <returns>A function that takes a graphql context and returns whether the delte is successful</returns>
	public static Func<ResolveFieldContext<object>, Task<object>> CreateConditionalUpdateMutation<TModel>(string name)
		where TModel : class, IOwnerAbstractModel, new()
	{
		return async context =>
		{
			var graphQlContext = (CsharpReferenceGraphQlContext)context.UserContext;
			var crudService = graphQlContext.CrudService;
			var user = graphQlContext.User;
			var dbSet = graphQlContext.DbContext.GetDbSet<TModel>(typeof(TModel).Name).AsQueryable();

			var models = QueryHelpers.CreateConditionalWhere(context, dbSet);
			models = QueryHelpers.CreateIdsCondition(context, models);
			models = QueryHelpers.CreateIdCondition(context, models);
			var fieldsToUpdate = context.GetArgument<List<string>>("fieldsToUpdate");
			var valuesToUpdate = context.GetArgument<TModel>("valuesToUpdate");

			var createObject = Expression.New(typeof(TModel));

			var fields = new List<MemberBinding>();
			foreach (string field in fieldsToUpdate)
			{
				var modelType = valuesToUpdate.GetType();
				var prop = modelType.GetProperty(field.ConvertToPascalCase());

				object value;
				try
				{
					value = prop.GetValue(valuesToUpdate);
				}
				catch (NullReferenceException)
				{
					throw new ArgumentException($"Property {field} does not exist in the entity");
				}

				var target = Expression.Constant(value, prop.PropertyType);

				fields.Add(Expression.Bind(prop, target));
			}
			var initializePropertiesOnObject = Expression.MemberInit(
				createObject,
				fields);

			try
			{
				return await crudService.ConditionalUpdate(models, initializePropertiesOnObject);
			}
			catch (AggregateException exception)
			{
				context.Errors.AddRange(
					exception.InnerExceptions.Select(error => new ExecutionError(error.Message)));
				return false;
			}
		};
	}
}

public class IdObject
{
	public Guid Id { get; set; }

	public static List<IdObject> FromList(IEnumerable<Guid> ids)
	{
		return ids.Select(o => new IdObject { Id = o }).ToList();
	}
}

public class DeleteMutation
{
	public static Func<ResolveFieldContext<object>, Task<object>> CreateDeleteMutation<TModel>(string name)
		where TModel : class, IOwnerAbstractModel, new()
	{
		return async context =>
		{
			var graphQlContext = (CsharpReferenceGraphQlContext)context.UserContext;
			var crudService = graphQlContext.CrudService;
			var ids = context.GetArgument<List<Guid>>($"{name}Ids".ToCamelCase());

			try
			{
				var deletedIds = await crudService.Delete<TModel>(ids);
				return IdObject.FromList(deletedIds);
			}
			catch (AggregateException exception)
			{
				context.Errors.AddRange(
					exception.InnerExceptions.Select(error => new ExecutionError(error.Message)));
				return new List<TModel>();
			}
		};
	}

	public static Func<ResolveFieldContext<object>, Task<object>> CreateConditionalDeleteMutation<TModel>(string name)
		where TModel : class, IOwnerAbstractModel, new()
	{
		return async context =>
		{
			var graphQlContext = (CsharpReferenceGraphQlContext)context.UserContext;
			var crudService = graphQlContext.CrudService;
			var user = graphQlContext.User;
			var dbSet = graphQlContext.DbContext.GetDbSet<TModel>(typeof(TModel).Name).AsQueryable();

			var models = QueryHelpers.CreateConditionalWhere(context, dbSet);
			models = QueryHelpers.CreateIdsCondition(context, models);
			models = QueryHelpers.CreateIdCondition(context, models);

			try
			{
				return await crudService.ConditionalDelete(models);
			}
			catch (AggregateException exception)
			{
				context.Errors.AddRange(
					exception.InnerExceptions.Select(error => new ExecutionError(error.Message)));
				return false;
			}
		};
	}
}