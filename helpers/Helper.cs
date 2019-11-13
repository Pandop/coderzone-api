namespace CoderzoneGrapQLAPI.helpers
{

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using CoderzoneGrapQLAPI.GraphQL.Mutations.otherTypes;
	using global::CsharpReference.Services;
	using global::GraphQL.EntityFramework;
	using global::GraphQL.Types;

	namespace CsharpReference.Graphql.Helpers
	{
		public static class QueryHelpers
		{

			public static Func<ResolveFieldContext<object>, IQueryable<TModel>> CreateResolveFunction<TModel>()
				where TModel : class, IOwnerAbstractModel, new()
			{
				return context =>
				{
					var graphQlContext = (GraphQLCsharpReferenceContext)context.UserContext;
					var crudService = graphQlContext.CrudService;
					var auditFields = AuditReadData.FromGraphqlContext(context);
					return crudService.Get<TModel>(auditFields: auditFields);
				};
			}

			public static IQueryable<T> CreateConditionalWhere<T>(
				ResolveFieldContext<object> context,
				IQueryable<T> models,
				string argName = "conditions")
				where T : IOwnerAbstractModel
			{
				if (context.HasArgument(argName))
				{
					var wheres = context.GetArgument<List<List<WhereExpression>>>(argName);
					return models.AddConditionalWhereFilter(wheres);
				}

				return models;
			}

			public static IQueryable<T> CreateWhereCondition<T>(ResolveFieldContext<object> context, IQueryable<T> models, string argName = "where")
				where T : IOwnerAbstractModel
			{
				if (context.HasArgument(argName))
				{
					var wheres = context.GetArgument<List<WhereExpression>>(argName);
					return models.AddWhereFilter(wheres);
				}

				return models;
			}

			public static IQueryable<T> CreateIdCondition<T>(ResolveFieldContext<object> context, IQueryable<T> models, string argName = "id")
				where T : IOwnerAbstractModel
			{
				if (context.HasArgument(argName))
				{
					var id = context.GetArgument<Guid>(argName);
					models = models.Where(model => model.Id == id);
				}

				return models;
			}

			public static IQueryable<T> CreateIdsCondition<T>(ResolveFieldContext<object> context, IQueryable<T> models, string argName = "ids")
				where T : IOwnerAbstractModel
			{
				if (context.HasArgument(argName))
				{
					var ids = context.GetArgument<List<Guid>>(argName);
					models = models.Where(model => ids.Contains(model.Id));
				}

				return models;
			}

			public static IQueryable<T> CreateSkip<T>(ResolveFieldContext<object> context, IQueryable<T> models, string argName = "skip")
				where T : IOwnerAbstractModel
			{
				if (context.HasArgument(argName))
				{
					var skip = context.GetArgument<int>(argName);
					models = models.Skip(skip);
				}

				return models;
			}

			public static IQueryable<T> CreateTake<T>(ResolveFieldContext<object> context, IQueryable<T> models, string argName = "take")
				where T : IOwnerAbstractModel
			{
				if (context.HasArgument(argName))
				{
					var take = context.GetArgument<int>(argName);
					models = models.Take(take);
				}

				return models;
			}

			public static IQueryable<T> CreateOrderBy<T>(ResolveFieldContext<object> context, IQueryable<T> models, string argName = "orderBy")
				where T : IOwnerAbstractModel
			{
				if (context.HasArgument(argName))
				{
					var orderBys = context.GetArgument<List<OrderBy>>(argName);
					return models.AddOrderBys(orderBys);
				}

				return models;
			}
		}
	}
}
