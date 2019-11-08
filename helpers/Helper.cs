using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoderzoneGrapQLAPI.helpers
{
	/*
 * @bot-written
 * 
 * WARNING AND NOTICE
 * Any access, download, storage, and/or use of this source code is subject to the terms and conditions of the
 * Full Software Licence as accepted by you before being granted access to this source code and other materials,
 * the terms of which can be accessed on the Codebots website at https://codebots.com/full-software-license. Any
 * commercial use in contravention of the terms of the Full Software Licence may be pursued by Codebots through
 * licence termination and further legal action, and be required to indemnify Codebots for any loss or damage,
 * including interest and costs. You are deemed to have accepted the terms of the Full Software Licence on any
 * access, download, storage, and/or use of this source code.
 * 
 * BOT WARNING
 * This file is bot-written.
 * Any changes out side of "protected regions" will be lost next time the bot makes any changes.
 */
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using CsharpReference.Graphql.Types;
	using CsharpReference.Helpers;
	using CsharpReference.Models;
	using CsharpReference.Services;
	using global::GraphQL.Types;
	using GraphQL.EntityFramework;
	using GraphQL.Types;

	namespace CsharpReference.Graphql.Helpers
	{
		public class QueryHelpers
		{

			public static Func<ResolveFieldContext<object>, IQueryable<TModel>> CreateResolveFunction<TModel>()
				where TModel : class, IOwnerAbstractModel, new()
			{
				return context =>
				{
					var graphQlContext = (CsharpReferenceGraphQlContext)context.UserContext;
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
