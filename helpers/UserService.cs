using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using CoderzoneGrapQLAPI.helpers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using AspNet.Security.OpenIdConnect.Extensions;
using AspNet.Security.OpenIdConnect.Primitives;
using Microsoft.AspNetCore.Authentication;
using OpenIddict.Abstractions;
using OpenIddict.Server;


namespace CsharpReference.Services
{
	public class RegisterModel
	{
		[Required]
		public string Email { get; set; }

		[Required]
		public string Password { get; set; }

		public ICollection<string> Groups { get; set; }
	}

	public class RegisterResult
	{
		public Coder User { get; set; }
		public IdentityResult Result { get; set; }
	}

	public class UserUpdateModel : RegisterModel
	{
		public Guid Id { get; set; }
		public new string Password { get; set; }
	}

	public class GroupResult
	{
		public string Name { get; set; }
		public bool HasBackendAccess { get; set; }

		public GroupResult(Group group)
		{
			Name = group.Name;
			HasBackendAccess = group.HasBackendAccess ?? false;
		}
	}


	public class UserResult
	{
		public Guid Id { get; set; }
		public string Email { get; set; }
		public List<GroupResult> Groups { get; set; } = new List<GroupResult>();

		public UserResult(Coder user, IEnumerable<Group> groups)
		{
			Id = user.Id;
			Email = user.UserName;
			if (groups != null)
			{
				Groups.AddRange(groups.Select(group => new GroupResult(group)));
			}
		}
	}


	public class UserService
	{
		private readonly IOptions<IdentityOptions> _identityOptions;
		private readonly SignInManager<Coder> _signInManager;
		private readonly UserManager<Coder> _userManager;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly RoleManager<Group> _roleManager;
		private readonly EmailService _emailService;
		private readonly IConfiguration _configuration;

		public UserService(
			IOptions<IdentityOptions> identityOptions,
			SignInManager<Coder> signInManager,
			UserManager<Coder> userManager,
			IHttpContextAccessor httpContextAccessor,
			RoleManager<Group> roleManager,
			EmailService emailService,
			IConfiguration configuration)
		{
			_identityOptions = identityOptions;
			_signInManager = signInManager;
			_userManager = userManager;
			_httpContextAccessor = httpContextAccessor;
			_roleManager = roleManager;
			_emailService = emailService;
			_configuration = configuration;
		}

		public async Task<List<UserResult>> getUsers()
		{
			return await _userManager.Users.Select(user => new UserResult(user, null)).ToListAsync();
		}

		public async Task<UserResult> GetUser(ClaimsPrincipal principal)
		{
			try
			{
				var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == principal.Identity.Name);
				return await GetUser(user);
			}
			catch
			{
				throw new InvalidIdException();
			}
		}

		public async Task<UserResult> GetUser(Coder user)
		{
			var roleNames = await _userManager.GetRolesAsync(user);
			var roles = await _roleManager.Roles.Where(role => roleNames.Contains(role.Name)).ToListAsync();
			return new UserResult(user, roles);
		}
		public async Task<Coder> GetUserFromClaim(ClaimsPrincipal principal)
		{
			return await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == principal.Identity.Name);
		}

		public async Task<RegisterResult> RegisterUser(
			RegisterModel model,
			IEnumerable<string> groups,
			bool sendRegisterEmail = false)
		{
			var user = await _userManager.FindByEmailAsync(model.Email);
			if (user != null)
			{
				throw new DuplicateUserException();
			}

			user = new Coder
			{
				UserName = model.Email,
				Email = model.Email,
			};

			return await RegisterUser(user, model.Password, groups, sendRegisterEmail);
		}

		public async Task<RegisterResult> RegisterUser(
			Coder user,
			string password,
			IEnumerable<string> groups,
			bool sendRegisterEmail = false)
		{
			// A user should own their own entity
			if (user.Id == default(Guid))
			{
				user.Id = Guid.NewGuid();
			}
			if (string.IsNullOrWhiteSpace(user.UserName))
			{
				user.UserName = user.Email;
			}

			user.Owner = user.Id;

			user.EmailConfirmed = !sendRegisterEmail;
			var result = await _userManager.CreateAsync(user, password);

			if (!result.Succeeded)
			{
				return new RegisterResult { Result = result, User = user };
			}

			var newUser = await _userManager.Users.FirstAsync(u => u.UserName == user.UserName);

			if (sendRegisterEmail)
			{
				var serverUrl = _configuration.GetSection("ServerSettings")["ServerUrl"];
				var token = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);

				token = HttpUtility.UrlEncode(token);
				var username = HttpUtility.UrlEncode(newUser.UserName);

				var email = File.ReadAllText("Assets/Emails/RegisterEmail.template.html")
					.Replace("${user}", user.UserName)
					.Replace("${confirmEmailUrl}", $"{serverUrl}/confirm-email?token={token}&username={username}");

				await _emailService.SendEmail(new EmailEntity	{To = new[] { newUser.Email },	Body = email, Subject = "Confirm Account",});
			}

			if (groups != null)
			{
				await _userManager.AddToRolesAsync(newUser, groups);
			}

			return new RegisterResult { Result = result, User = newUser };
		}

		public async Task<IdentityResult> ConfirmEmail(string email, string token)
		{
			var user = await _userManager.Users.FirstAsync(u => u.Email == email);
			return await _userManager.ConfirmEmailAsync(user, token);
		}

		public async Task<IdentityResult> UpdateUser(UserUpdateModel model)
		{
			var user = await _userManager.FindByNameAsync(model.Email);

			if (user == null)
			{
				throw new UserNotFoundException();
			}

			user.UserName = model.Email;
			user.Email = model.Email;
			// % protected region % [Add extra user update fields here] off begin
			// % protected region % [Add extra user update fields here] end

			var result = await _userManager.UpdateAsync(user);
			if (result.Succeeded && model.Password != null)
			{
				var token = await _userManager.GeneratePasswordResetTokenAsync(user);
				result = await _userManager.ResetPasswordAsync(user, token, model.Password);
			}

			if (model.Groups != null)
			{
				var currentGroups = await _userManager.GetRolesAsync(user);
				await _userManager.RemoveFromRolesAsync(user, currentGroups);
				await _userManager.AddToRolesAsync(user, model.Groups);
			}

			return result;
		}

		public async Task<bool> SendPasswordResetEmail(Coder user)
		{
			var token = await _userManager.GeneratePasswordResetTokenAsync(user);
			token = HttpUtility.UrlEncode(token);
			var serverUrl = _configuration.GetSection("ServerSettings")["ServerUrl"];

			var email = File.ReadAllText("Assets/Emails/ResetPassword.template.html")
				.Replace("${user}", user.UserName)
				.Replace("${passwordResetUrl}", $"{serverUrl}/reset-password?token={token}&username={user.UserName}");

			return await _emailService.SendEmail(new EmailEntity
			{
				To = new[] { user.Email },
				Body = email,
				Subject = "Reset Password",
			});
		}

		public async Task<bool> DeleteUser(Guid id)
		{
			var user = await _userManager.FindByIdAsync(id.ToString());

			if (user == null)
			{
				return false;
			}

			var result = await _userManager.DeleteAsync(user);
			return result.Succeeded;
		}

		public async Task<Coder> CheckCredentials(string username,	string password, bool lockoutOnFailure = true, bool validateEmailConfirmation = true)
		{
			var user = await _userManager.FindByNameAsync(username);
			if (user == null)
			{
				throw new InvalidUserPasswordException();
			}
			if (validateEmailConfirmation && !user.EmailConfirmed)
			{
				throw new InvalidUserPasswordException("This account is not yet activated");
			}

			var success = await _signInManager.CheckPasswordSignInAsync(user, password, lockoutOnFailure);
			if (!success.Succeeded)
			{
				throw new InvalidUserPasswordException();
			}

			return user;
		}

		public async Task<ClaimsPrincipal> CreateUserPrincipal(
			Coder user,
			string authenticationScheme = CookieAuthenticationDefaults.AuthenticationScheme)
		{
			var identity = new ClaimsIdentity(
				authenticationScheme,
				ClaimTypes.Name,
				ClaimTypes.Role);
			identity.AddClaim(new Claim("UserId", user.Id.ToString()));
			identity.AddClaim(new Claim(OpenIdConnectConstants.Claims.Subject, user.UserName));
			identity.AddClaim(new Claim(ClaimTypes.Name, user.Email));
			identity.AddClaims((await _userManager.GetRolesAsync(user)).Select(r => new Claim(ClaimTypes.Role, r)));

			return new ClaimsPrincipal(identity);
		}

		public async Task<AuthenticationTicket> Exchange(OpenIdConnectRequest request)
		{
			if (request.IsPasswordGrantType())
			{
				var user = await CheckCredentials(request.Username, request.Password);

				// Create a new authentication ticket.
				var ticket = await CreateTicketAsync(request, user);

				return ticket;
			}

			if (request.IsRefreshTokenGrantType())
			{
				var info = await _httpContextAccessor.HttpContext.AuthenticateAsync(OpenIddictServerDefaults.AuthenticationScheme);

				var user = await _userManager.GetUserAsync(info.Principal);
				if (user == null)
				{
					throw new InvalidUserPasswordException();
				}

				if (!await _signInManager.CanSignInAsync(user))
				{
					throw new InvalidUserPasswordException();
				}

				return await CreateTicketAsync(request, user);
			}

			throw new InvalidGrantTypeException();
		}

		private async Task<AuthenticationTicket> CreateTicketAsync(OpenIdConnectRequest request, Coder user)
		{
			// Create a new ClaimsPrincipal containing the claims that
			// will be used to create an id_token, a token or a code.
			var principal = await CreateUserPrincipal(user, OpenIdConnectConstants.Schemes.Bearer);

			// Create a new authentication ticket holding the user identity.
			var ticket = new AuthenticationTicket(principal,
				new AuthenticationProperties(),
				OpenIddictServerDefaults.AuthenticationScheme);

			// The scopes that are provided to every request
			var defaultScopes = new List<string>
			{
				OpenIdConnectConstants.Scopes.OpenId,
				OpenIddictConstants.Scopes.Roles,
			};
			// These are additional scopes that the client can also explicitly request
			var allowedScopes = new List<string>
			{
				OpenIdConnectConstants.Scopes.Email,
				OpenIdConnectConstants.Scopes.Profile,
				OpenIdConnectConstants.Scopes.OfflineAccess,
			};
			ticket.SetScopes(defaultScopes.Concat(allowedScopes.Intersect(request.GetScopes())));

			ticket.SetResources("resource-server");

			ticket.SetAccessTokenLifetime(new TimeSpan(7, 0, 0, 0));

			// Note: by default, claims are NOT automatically included in the access and identity tokens.
			// To allow OpenIddict to serialize them, you must attach them a destination, that specifies
			// whether they should be included in access tokens, in identity tokens or in both.

			foreach (var claim in ticket.Principal.Claims)
			{
				// Never include the security stamp in the access and identity tokens, as it's a secret value.
				if (claim.Type == _identityOptions.Value.ClaimsIdentity.SecurityStampClaimType)
				{
					continue;
				}

				var destinations = new List<string>
				{
					OpenIdConnectConstants.Destinations.AccessToken
				};

				// Only add the iterated claim to the id_token if the corresponding scope was granted to the client application.
				// The other claims will only be added to the access_token, which is encrypted when using the default format.
				if ((claim.Type == OpenIdConnectConstants.Claims.Name && ticket.HasScope(OpenIdConnectConstants.Scopes.Profile)) ||
					(claim.Type == OpenIdConnectConstants.Claims.Email && ticket.HasScope(OpenIdConnectConstants.Scopes.Email)) ||
					(claim.Type == OpenIdConnectConstants.Claims.Role && ticket.HasScope(OpenIddictConstants.Claims.Roles)))
				{
					destinations.Add(OpenIdConnectConstants.Destinations.IdentityToken);
				}

				claim.SetDestinations(destinations);
			}

			return ticket;
		}

	}
}