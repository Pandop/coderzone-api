using CoderzoneGrapQLAPI.Models;
using CoderzoneGrapQLAPI.Services;
using GraphQL.DataLoader;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoderzoneGrapQLAPI.GraphQL.Types
{
	public class ProfileType : ObjectGraphType<Profile>
	{
		public ProfileType(IProfileRepository profile,IProjectRepository project, IProgrammerRepository _programmer,	IDataLoaderContextAccessor dataLoaderAccessor)
		{
			Field(t=> t.Id, type: typeof(IdGraphType)).Description("User Profile Id");
			Field(t => t.FirstName);
			Field(t => t.LastName);
			Field(t => t.Avatar);
			Field(t => t.Bio);
			Field(t => t.City);
			Field(t => t.Street);
			Field(t => t.Number);
			Field(t => t.CreatedAt);
			Field(t => t.UpdatedAt);
			Field(t => t.ProgrammerId, type: typeof(IdGraphType));
			Field(t => t.DatePublished);

			Field<ListGraphType<ProjectType>>(
				name: "projects",
				resolve: context =>
				{
					var loader = dataLoaderAccessor.Context.GetOrAddCollectionBatchLoader<Guid, Project>("GetAllProjectsAsync", profile.GetAllProjectsAsync);
					//var loader = dataLoaderAccessor.Context.GetOrAddBatchLoader<Guid, Project>("GetProjectsAsync", programmer.GetProjectsAsync);
					//return programmer.GetAllProjectsByProgrammerAsync(context.Source.Id);
					//return loader.LoadAsync(context.Source.Id);
					return loader.LoadAsync(context.Source.Id);
				}

			);
			Field<ListGraphType<WorkExperienceType>>(
				name: "works",
				resolve: context => profile.GetAllWorkExperiencesByProgrammerAsync(context.Source.Id)

			);
			Field<ListGraphType<QualificationType>>(
				name: "qualifications",
				resolve: context => profile.GetAllQualificationsByProgrammerAsync(context.Source.Id)

			);
			Field<ListGraphType<SkillType>>(
				name: "skills",
				resolve: context => profile.GetAllSkillsByProgrammerAsync(context.Source.Id)
			);
		}
	}
}
