using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CoderzoneGrapQLAPI.helpers.utils
{
	public class AuthenticationFilterAttribute : Attribute, IFilterFactory, IOrderedFilter
	{
		public bool IsReusable => true;
		public int Order { get; set; } = 1000;

		public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
		{
			return serviceProvider.GetRequiredService<AuthenticationFilter>();
		}
	}

	public class AuthenticationFilter : AutoValidateAntiforgeryTokenAuthorizationFilter
	{
		public AuthenticationFilter(
			IAntiforgery antiforgery,
			ILoggerFactory loggerFactory)
			: base(antiforgery, loggerFactory)
		{
		}

		protected override bool ShouldValidate(AuthorizationFilterContext context)
		{
			// Should only validate the XSRF token of authenticated api requests which use cookie auth
			return context.HttpContext.Request.Cookies.ContainsKey(".AspNetCore." + CookieAuthenticationDefaults.AuthenticationScheme)
					&& context.HttpContext.User.Identity.IsAuthenticated
					&& base.ShouldValidate(context);
		}
	}
}
