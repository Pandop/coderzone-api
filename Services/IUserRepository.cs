using CoderzoneGrapQLAPI.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoderzoneGrapQLAPI.Services
{
	public interface IUserRepository
	{
		Task<IEnumerable<User>> GetUsersAsync();
		Task<User> GetUserAsync(Guid userId);
		Task<Profile> GetUserProfileAsync(Guid profileId);
		Task<Country> GetCountryForUserAsync(Guid userId);

		Task<IEnumerable<string>> GetSkillsForUserAsync(Guid userId);
		Task<IEnumerable<string>> GetTechStackForUserAsync(Guid userId);
		Task<bool>UserExistsAsync(Guid userId);
	}
}
