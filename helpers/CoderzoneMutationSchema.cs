using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoderzoneGrapQLAPI.GraphQL.Mutations.otherTypes;
using CoderzoneGrapQLAPI.GraphQL.Types;
using CoderzoneGrapQLAPI.helpers.crudFields;
using CoderzoneGrapQLAPI.Models;
using CsharpReference.Services;
using GraphQL;
using GraphQL.EntityFramework;
using GraphQL.Types;

namespace CoderzoneGrapQLAPI.helpers
{
	public class CsharpReferenceSchema : Schema
	{
		public CsharpReferenceSchema(IDependencyResolver resolver) : base(resolver)
		{
			//Query = resolver.Resolve<CsharpReferenceQuery>();
			Mutation = resolver.Resolve<CoderzoneReferenceMutation>();
		}
	}
	public class CoderzoneReferenceMutation : ObjectGraphType<object>
	{
		private const string ConditionalWhereDesc = "A list of lists of where conditions. The conditions inside the " +	"innermost lists are joined with and OR and the results of those " +  "lists are joined with an AND";
		public CoderzoneReferenceMutation()
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

		public void AddMutationField<TModelCreateInputType, TModelUpdateInputType, TModelType, TModel>(	string name, Func<ResolveFieldContext<object>, Task<object>> createMutation = null, Func<ResolveFieldContext<object>, Task<object>> updateMutation = null, Func<ResolveFieldContext<object>, Task<object>> deleteMutation = null, Func<ResolveFieldContext<object>, Task<object>> conditionalUpdateMutation = null, Func<ResolveFieldContext<object>, Task<object>> conditionalDeleteMutation = null) 
			where TModelCreateInputType : InputObjectGraphType
			where TModelUpdateInputType : InputObjectGraphType
			where TModelType : ObjectGraphType<TModel>
			where TModel : class, IOwnerAbstractModel, new()
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
}
