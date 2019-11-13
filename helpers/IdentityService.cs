using System.Collections.Generic;
using System.Threading.Tasks;
using CoderzoneGrapQLAPI.helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace CsharpReference.Services
{
	public class IdentityService
	{

		private bool _fetched = false;

		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly UserService _userService;
		private readonly UserManager<Coder> _userManager;

		public Coder User { get; private set; }
		public IList<string> Groups { get; private set; }

		public IdentityService(
			IHttpContextAccessor httpContextAccessor,
			UserService userService,
			UserManager<Coder> userManager)
		{
			_httpContextAccessor = httpContextAccessor;
			_userService = userService;
			_userManager = userManager;
		}

		public async Task RetrieveUserAsync()
		{
			if (_fetched != true)
			{
				User = await _userService.GetUserFromClaim(_httpContextAccessor.HttpContext.User);
				Groups = await _userManager.GetRolesAsync(User);
				_fetched = true;
			}
		}
	}
}