using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoderzoneGrapQLAPI.helpers.services;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace CoderzoneGrapQLAPI.helpers.utils
{
	public class XsrfActionFilterAttribute : Attribute, IFilterFactory, IOrderedFilter
	{
		public bool IsReusable => true;
		public int Order { get; set; } = 1100;

		public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
		{
			return serviceProvider.GetRequiredService<XsrfActionFilter>();
		}

		public class XsrfActionFilter : IActionFilter
		{
			private readonly XsrfService _xsrfService;

			public XsrfActionFilter(XsrfService xsrfService)
			{
				_xsrfService = xsrfService;
			}

			public void OnActionExecuting(ActionExecutingContext context)
			{
				_xsrfService.AddXsrfToken(context.HttpContext);
			}

			public void OnActionExecuted(ActionExecutedContext context)
			{
			}
		}
	}
}
