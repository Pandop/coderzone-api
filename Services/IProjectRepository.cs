using CoderzoneGrapQLAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoderzoneGrapQLAPI.Services
{
	public interface IProjectRepository
	{
		Task<IEnumerable<Project>> GetAllProjectsAsync();
	}
}
