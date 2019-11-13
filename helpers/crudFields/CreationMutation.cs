using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CsharpReference.Services;
using GraphQL;
using GraphQL.Types;

namespace CoderzoneGrapQLAPI.helpers.crudFields
{
	public static class CreateMutation
	{
		public static Func<ResolveFieldContext<object>, Task<object>> CreateCreateMutation<TModel>(string name)
			where TModel : class, IOwnerAbstractModel, new()
		{
			return async context =>
			{
				var graphQlContext = (GraphQLCsharpReferenceContext)context.UserContext;
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
			where TModel : Coder, IOwnerAbstractModel, new()
			where TRegisterModel : IRegistrationModel<TModel>
			where TGraphQlRegisterModel : TRegisterModel
		{
			return async context =>
			{
				var graphQlContext = (GraphQLCsharpReferenceContext)context.UserContext;
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
