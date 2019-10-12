using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoderzoneGrapQLAPI.Models;

namespace CoderzoneGrapQLAPI.Services
{
	public class ProjectRepository : IProjectRepository
	{
		public ProjectRepository()
		{

		}

		public Task<IEnumerable<Project>> GetAllProjectsAsync()
		{
			throw new NotImplementedException();
		}
	}
}
