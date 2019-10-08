using System;
using System.Collections.Generic;
using System.Linq;
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
			return Task.FromResult(_programmerContext.Projects.Where(p => p.Programmer.Id == programmerId).AsEnumerable());
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

		public Task<bool> ProgrammerExistsAsync(Guid programmerId)
		{
			// bookId is null or empty
			if (programmerId == Guid.Empty) throw new ArgumentNullException(nameof(programmerId));

			// Return true if author with authorId exists
			return Task.FromResult(_programmerContext.Profiles.Any(p => p.Id == programmerId));
		}
	}
}
