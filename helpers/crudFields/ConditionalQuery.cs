using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoderzoneGrapQLAPI.helpers.CsharpReference.Graphql.Helpers;
using CsharpReference.Services;
using GraphQL.Types;

namespace CoderzoneGrapQLAPI.helpers.crudFields
{
	public static class ConditionalQuery
	{
		public static Func<ResolveFieldContext<object>, IQueryable<TModel>> CreateConditionalQuery<TModel>()
			where TModel : class, IOwnerAbstractModel, new()
		{
			return context =>
			{
				// Fetch the models that we need
				var models = QueryHelpers.CreateResolveFunction<TModel>()(context);

				// Apply the conditions to the query
				models = QueryHelpers.CreateConditionalWhere(context, models);

				return models;
			};
		}
	}
}
