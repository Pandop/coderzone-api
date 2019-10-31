using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CoderzoneGrapQLAPI.Models;

namespace CoderzoneGrapQLAPI.Services
{
	public class ProgrammerRepository : IProgrammerRepository
	{
		private readonly CoderzoneApiDbContext _programmerContext;

		public ProgrammerRepository(CoderzoneApiDbContext programmerContext)
		{
			_programmerContext = programmerContext;
		}

		public Task<IEnumerable<Project>> GetAllProjectsByProgrammerAsync(Guid programmerId)
		{
			// Fetch all Projects from programmer
			return Task.FromResult(_programmerContext.Projects.Where(p => p.Profile.Id == programmerId).AsEnumerable());
		}

		public Task<IEnumerable<Qualification>> GetAllQualificationsByProgrammerAsync(Guid programmerId)
		{
			// Fetch all Qualification from programmer
			return Task.FromResult(_programmerContext.Qualifications.Where(q => q.Profile.Id == programmerId).AsEnumerable());
		}

		public Task<IEnumerable<Skill>> GetAllSkillsByProgrammerAsync(Guid programmerId)
		{
			// Fetch all Qualification from programmer
			return Task.FromResult(_programmerContext.Skills.Where(q => q.Profile.Id == programmerId).AsEnumerable());
		}

		public Task<IEnumerable<WorkExperience>> GetAllWorkExperiencesByProgrammerAsync(Guid programmerId)
		{
			// Fetch all Work Experience from programmer
			return Task.FromResult(_programmerContext.WorkExperiences.Where(w => w.Profile.Id == programmerId).AsEnumerable());
		}

		public Task<Country> GetCountryForProgrammerAsync(Guid programmerId)
		{
			// Fetch all Qualification from programmer
			return Task.FromResult(_programmerContext.Programmers.Where(p => p.Id==programmerId).Select(c=> c.Country).FirstOrDefault());
		}

		public Task<Programmer> GetProgrammerAsync(Guid programmerId)
		{
			// Empty programmerId field
			if (programmerId == Guid.Empty) throw new ArgumentNullException(nameof(programmerId));

			// Fetch the programmer's profile
			return Task.FromResult(_programmerContext.Programmers.FirstOrDefault(p => p.Id == programmerId));
		}

		public Task<IEnumerable<Programmer>> GetProgrammersAsync()
		{
			return Task.FromResult(_programmerContext.Programmers.AsEnumerable());
		}

		public Task<ILookup<Guid, Project>> GetAllProjectsAsync(IEnumerable<Guid> programmerId)
		{
			var reviews = _programmerContext.Projects.Where(p => programmerId.Contains(p.Profile.Id)).ToLookup(r => r.Id);

			var projects = _programmerContext.Projects.Where(p => programmerId.Contains(p.Profile.Id)).ToLookup(r => r.Profile.Id);
			//return Task.FromResult(_programmerContext.Projects.Where(p => programmerId.Contains(p.Programmer.Id)).ToLookup(r => r.Programmer.Id));
			return Task.FromResult(projects);
		}

		public async Task<IDictionary<Guid, Project>> GetProjectsAsync(IEnumerable<Guid> programmerId, CancellationToken token)
		{
			var reviews =  _programmerContext.Projects.Where(p =>  programmerId.Contains(p.Profile.Id)).ToDictionary(x => x);
			//return await Task.FromResult<Dictionary<Guid, Project>>(reviews);
			var taskResults = await Task.FromResult<IDictionary<Guid, Project>>(_programmerContext.Projects.Where(p => programmerId.Contains(p.Profile.Id)).ToDictionary(p=>p.Id));

			return taskResults; // Task.FromResult(taskResults);
		}

		public Task<State> GetStateForProgrammerAsync(Guid programmerId)
		{
			return Task.FromResult(_programmerContext.Programmers.Where(p=> p.Id==programmerId).Select(s=>s.State).FirstOrDefault());
		}

		public Task<bool> ProgrammerExistsAsync(Guid programmerId)
		{
			// bookId is null or empty
			if (programmerId == Guid.Empty) throw new ArgumentNullException(nameof(programmerId));

			// Return true if author with authorId exists
			return Task.FromResult(_programmerContext.Profiles.Any(p => p.Id == programmerId));
		}
	}
}
