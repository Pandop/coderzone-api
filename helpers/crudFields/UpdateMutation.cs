using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CoderzoneGrapQLAPI.helpers.CsharpReference.Graphql.Helpers;
using CsharpReference.Services;
using GraphQL;
using GraphQL.Types;

namespace CoderzoneGrapQLAPI.helpers.crudFields
{
	public static class UpdateMutation
	{
		public static Func<ResolveFieldContext<object>, Task<object>> CreateUpdateMutation<TModel>(string name)
			where TModel : class, IOwnerAbstractModel, new()
		{
			return async context =>
			{
				var graphQlContext = (GraphQLCsharpReferenceContext)context.UserContext;
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

		public static Func<ResolveFieldContext<object>, Task<object>> CreateConditionalUpdateMutation<TModel>(string name)
			where TModel : class, IOwnerAbstractModel, new()
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
}
