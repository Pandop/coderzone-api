
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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

		public Task<IEnumerable<Project>> GetAllProjectsByProgrammerAsync(Guid profileId)
		{
			// Fetch all Projects from programmer
			return Task.FromResult(_profileContext.Projects.Where(p => p.Profile.Id == profileId).AsEnumerable());
		}

		public Task<IEnumerable<Qualification>> GetAllQualificationsByProgrammerAsync(Guid profileId)
		{
			// Fetch all Qualification from programmer
			return Task.FromResult(_profileContext.Qualifications.Where(q => q.Profile.Id == profileId).AsEnumerable());
		}

		public Task<IEnumerable<Skill>> GetAllSkillsByProgrammerAsync(Guid profileId)
		{
			// Fetch all Qualification from programmer
			return Task.FromResult(_profileContext.Skills.Where(q => q.Profile.Id == profileId).AsEnumerable());
		}

		public Task<IEnumerable<WorkExperience>> GetAllWorkExperiencesByProgrammerAsync(Guid profileId)
		{
			// Fetch all Work Experience from programmer
			return Task.FromResult(_profileContext.WorkExperiences.Where(w => w.Profile.Id == profileId).AsEnumerable());
		}

		public Task<ILookup<Guid, Project>> GetAllProjectsAsync(IEnumerable<Guid> profileIds)
		{
			var reviews = _profileContext.Projects.Where(p => profileIds.Contains(p.Profile.Id)).ToLookup(r => r.Id);

			var projects = _profileContext.Projects.Where(p => profileIds.Contains(p.Profile.Id)).ToLookup(r => r.Profile.Id);
			//return Task.FromResult(_programmerContext.Projects.Where(p => programmerId.Contains(p.Programmer.Id)).ToLookup(r => r.Programmer.Id));
			return Task.FromResult(projects);
		}

		public async Task<IDictionary<Guid, Project>> GetProjectsAsync(IEnumerable<Guid> profileIds, CancellationToken token)
		{
			var reviews = _profileContext.Projects.Where(p => profileIds.Contains(p.Profile.Id)).ToDictionary(x => x);
			//return await Task.FromResult<Dictionary<Guid, Project>>(reviews);
			var taskResults = await Task.FromResult<IDictionary<Guid, Project>>(_profileContext.Projects.Where(p => profileIds.Contains(p.Profile.Id)).ToDictionary(p => p.Id));

			return taskResults; // Task.FromResult(taskResults);
		}
	}
}
