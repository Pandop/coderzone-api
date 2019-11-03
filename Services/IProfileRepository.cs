using CoderzoneGrapQLAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CoderzoneGrapQLAPI.Services
{
	public interface IProfileRepository
	{
		Task<Profile> GetProgrammerProfileAsync(Guid programmerId);
		Task<IEnumerable<Profile>> GetAllProfilesAsync();
		Task<bool> ProgrammerProfileExistsAsync(Guid profileId);
		Task<IEnumerable<Skill>> GetAllSkillsByProgrammerAsync(Guid profileId);
		Task<IEnumerable<Project>> GetAllProjectsByProgrammerAsync(Guid profileId);
		Task<IEnumerable<WorkExperience>> GetAllWorkExperiencesByProgrammerAsync(Guid profileId);
		Task<IEnumerable<Qualification>> GetAllQualificationsByProgrammerAsync(Guid profileId);
		Task<ILookup<Guid, Project>> GetAllProjectsAsync(IEnumerable<Guid> profileIds);
		Task<IDictionary<Guid, Project>> GetProjectsAsync(IEnumerable<Guid> profileIds, CancellationToken token);
	}
}