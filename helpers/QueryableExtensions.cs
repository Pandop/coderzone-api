using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CoderzoneGrapQLAPI.GraphQL.Mutations.otherTypes;
using CoderzoneGrapQLAPI.Services;
using CsharpReference.Services;
using GraphQL.EntityFramework;
using Microsoft.AspNetCore.Identity;
using static CoderzoneGrapQLAPI.helpers.CrudService;

namespace CoderzoneGrapQLAPI.helpers
{
	public static class QueryableExtensions
	{
		public static IQueryable<T> AddReadSecurityFiltering<T>(
			this IQueryable<T> queryable,
			IdentityService identityService,
			UserManager<Coder> userManager,
			CoderzoneApiDbContext dbContext)
			where T : IOwnerAbstractModel, new()
		{
			return queryable.Where(SecurityService.CreateReadSecurityFilter<T>(identityService, userManager, dbContext));
		}

		public static IQueryable<T> AddUpdateSecurityFiltering<T>(
			this IQueryable<T> queryable,
			IdentityService identityService,
			UserManager<Coder> userManager,
			CoderzoneApiDbContext dbContext)
			where T : IOwnerAbstractModel, new()
		{
			return queryable.Where(SecurityService.CreateUpdateSecurityFilter<T>(identityService, userManager, dbContext));
		}

		public static IQueryable<T> AddDeleteSecurityFiltering<T>(
			this IQueryable<T> queryable,
			IdentityService identityService,
			UserManager<Coder> userManager,
			CoderzoneApiDbContext dbContext)
			where T : IOwnerAbstractModel, new()
		{
			return queryable.Where(SecurityService.CreateDeleteSecurityFilter<T>(identityService, userManager, dbContext));
		}

		public static IQueryable<T> AddPagination<T>(this IQueryable<T> queryable, Pagination pagination)
		{
			if (pagination != null && pagination.PageSize.HasValue && pagination.SkipAmount.HasValue)
			{
				return queryable
					.Take(pagination.PageSize.Value)
					.Skip(pagination.SkipAmount.Value);
			}

			return queryable;
		}

		public static IQueryable<T> AddConditionalWhereFilter<T>(this IQueryable<T> models,	IEnumerable<IEnumerable<WhereExpression>> wheres) where T : IOwnerAbstractModel
		{
			Expression<Func<T, bool>> baseRule = _ => false;
			foreach (var where in wheres)
			{
				var combinedPredicate = Expression.OrElse(baseRule.Body, baseRule.Body);
				foreach (var expression in where)
				{
					var predicate = ExpressionBuilder<T>.BuildPredicate(expression);
					combinedPredicate = Expression.OrElse(combinedPredicate, predicate.Body);
				}

				var param = Expression.Parameter(typeof(T), "model");
				var replacer = new ParameterReplacer(param);
				var func = Expression.Lambda<Func<T, bool>>(replacer.Visit(combinedPredicate), param);

				models = models.Where(func);
			}

			return models;
		}

		public static IQueryable<T> AddWhereFilter<T>(this IQueryable<T> models, IEnumerable<WhereExpression> wheres)
			where T : IOwnerAbstractModel
		{
			foreach (var where in wheres)
			{
				var predicate = ExpressionBuilder<T>.BuildPredicate(where);
				models = models.Where(predicate);
			}

			return models;
		}

		public static IQueryable<T> AddOrderBys<T>(this IQueryable<T> models, List<OrderBy> orderBys)
		{
			IOrderedQueryable<T> orderedQueryable = null;

			for (var i = 0; i < orderBys.Count; i++)
			{
				var orderBy = orderBys[i];

				var param = Expression.Parameter(typeof(T));
				var field = Expression.PropertyOrField(param, orderBy.Path);
				var func = Expression.Lambda<Func<T, object>>(Expression.Convert(field, typeof(object)), param);

				if (orderBy.Descending != null && orderBy.Descending == true)
				{
					orderedQueryable = i == 0
						? models.OrderByDescending(func)
						: orderedQueryable.ThenByDescending(func);
				}
				else
				{
					orderedQueryable = i == 0
						? models.OrderBy(func)
						: orderedQueryable.ThenBy(func);
				}
			}

			return orderedQueryable ?? models;
		}
	}
}
