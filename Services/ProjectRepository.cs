using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoderzoneGrapQLAPI.Models;

namespace CoderzoneGrapQLAPI.Services
{
	public class ProjectRepository : IProjectRepository
	{
		public readonly CoderzoneApiDbContext _projectContext;
		public ProjectRepository(CoderzoneApiDbContext projectContext)
		{
			_projectContext = projectContext;
		}

		

		public Task<IEnumerable<Project>> GetAllProjectsAsync()
		{
			throw new NotImplementedException();
		}

		public async Task<ILookup<Guid, Project>> GetProjectsAsync(IEnumerable<Guid> programmerId)
		{
			var reviews = _projectContext.Projects.Where(a => programmerId.Contains(a.Profile.Id)).ToAsyncEnumerable();
			return await reviews.ToLookup(r => r.Profile.Id);
		}
	}
}
