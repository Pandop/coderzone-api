using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoderzoneGrapQLAPI.GraphQL.Mutations.otherTypes;
using CoderzoneGrapQLAPI.helpers.CsharpReference.Graphql.Helpers;
using CsharpReference.Services;
using GraphQL;
using GraphQL.Types;

namespace CoderzoneGrapQLAPI.helpers.crudFields
{
	public static class DeleteMutation
	{
		public static Func<ResolveFieldContext<object>, Task<object>> CreateDeleteMutation<TModel>(string name)	where TModel : class, IOwnerAbstractModel, new()
		{
			return async context =>
			{
				var graphQlContext = (GraphQLCsharpReferenceContext)context.UserContext;
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

		public static Func<ResolveFieldContext<object>, Task<object>> CreateConditionalDeleteMutation<TModel>(string name)	where TModel : class, IOwnerAbstractModel, new()
		{
			return async context =>
			{
				var graphQlContext = (GraphQLCsharpReferenceContext)context.UserContext;
				var crudService = graphQlContext.CrudService;
				var user = graphQlContext.Coder;
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
}
