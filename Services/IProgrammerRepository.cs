using CoderzoneGrapQLAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CoderzoneGrapQLAPI.Services
{
	public interface IProgrammerRepository
	{
		Task<IEnumerable<Programmer>> GetProgrammersAsync();
		Task<Programmer> GetProgrammerAsync(Guid programmerId);
		Task<Country> GetCountryForProgrammerAsync(Guid programmerId);
		Task<State> GetStateForProgrammerAsync(Guid programmerId);
		Task<IEnumerable<Skill>> GetAllSkillsByProgrammerAsync(Guid programmerId);
		Task<IEnumerable<Project>> GetAllProjectsByProgrammerAsync(Guid programmerId);
		Task<IEnumerable<WorkExperience>> GetAllWorkExperiencesByProgrammerAsync(Guid programmerId);
		Task<IEnumerable<Qualification>> GetAllQualificationsByProgrammerAsync(Guid programmerId);
		
		Task<bool> ProgrammerExistsAsync(Guid programmerId);
		Task<ILookup<Guid, Project>> GetAllProjectsAsync(IEnumerable<Guid> programmerId);
		Task<IDictionary<Guid, Project>> GetProjectsAsync(IEnumerable<Guid> programmerId, CancellationToken token);


	}
}
