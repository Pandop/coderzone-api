using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;

namespace CoderzoneGrapQLAPI.helpers.services
{
	public class XsrfService
	{
		private const string TokenName = "XSRF-TOKEN";

		private readonly IAntiforgery _antiforgery;

		public XsrfService(IAntiforgery antiforgery)
		{
			_antiforgery = antiforgery;
		}

		public void AddXsrfToken(HttpContext context)
		{
			if (string.IsNullOrEmpty(context.User.Identity.Name))
			{
				return;
			}

			var tokens = _antiforgery.GetAndStoreTokens(context);

			var date = new DateTime(DateTime.Now.Ticks, DateTimeKind.Unspecified);
			date = date.AddDays(7);

			context.Response.Cookies.Append(
				TokenName,
				tokens.RequestToken,
				new CookieOptions
				{
					HttpOnly = false,
					Expires = new DateTimeOffset(date, TimeSpan.FromHours(0))
				});
		}

		public void AddXsrfToken(HttpContext context, ClaimsPrincipal userClaim)
		{
			var existingClaim = context.User;
			context.User = userClaim;

			AddXsrfToken(context);

			context.User = existingClaim;
		}
	}
}
