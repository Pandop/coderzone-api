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
	}
}
