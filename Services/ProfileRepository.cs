
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoderzoneGrapQLAPI.Models;

namespace CoderzoneGrapQLAPI.Services
{
	public class ProfileRepository : IProfileRepository
	{
		private readonly CoderzoneApiDbContext _profileContext;
		public ProfileRepository(CoderzoneApiDbContext profileContext)
		{
			_profileContext = profileContext;
		}
		public Task<IEnumerable<Profile>> GetAllProfilesAsync()
		{
			// Fetch all profiles
			return Task.FromResult(_profileContext.Profiles.AsEnumerable());
		}

		public Task<Profile> GetProgrammerProfileAsync(Guid programmerId)
		{
			// Empty programmerId field
			if (programmerId == Guid.Empty) throw new ArgumentNullException(nameof(programmerId));

			// Fetch the programmer's profile
			return Task.FromResult(_profileContext.Profiles.FirstOrDefault(p => p.ProgrammerId == programmerId));
		}

		public Task<bool> ProgrammerProfileExistsAsync(Guid profileId)
		{
			// bookId is null or empty
			if (profileId == Guid.Empty) throw new ArgumentNullException(nameof(profileId));

			// Return true if author with authorId exists
			return Task.FromResult(_profileContext.Profiles.Any(a => a.Id == profileId));
		}
	}
}
