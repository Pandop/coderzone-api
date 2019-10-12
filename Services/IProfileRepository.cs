using CoderzoneGrapQLAPI.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoderzoneGrapQLAPI.Services
{
	public interface IProfileRepository
	{
		Task<Profile> GetProgrammerProfileAsync(Guid programmerId);
		Task<IEnumerable<Profile>> GetAllProfilesAsync();
		Task<bool> ProgrammerProfileExistsAsync(Guid profileId);
	}
}