using CoderzoneGrapQLAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoderzoneGrapQLAPI.Services
{
	public interface IProgrammerRepository
	{
		Task<IEnumerable<Programmer>> GetProgrammersAsync();
		Task<Programmer> GetProgrammerAsync(Guid programmerId);
		Task<IEnumerable<Project>> GetAllProjectsByProgrammerAsync(Guid programmerId);
		Task<bool> ProgrammerExistsAsync(Guid programmerId);
	}
}
