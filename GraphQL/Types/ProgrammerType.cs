using CoderzoneGrapQLAPI.Models;
using CoderzoneGrapQLAPI.Services;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoderzoneGrapQLAPI.GraphQL.Types
{
	public class ProgrammerType : ObjectGraphType<Programmer>
	{
		public ProgrammerType( IProfileRepository profile, IProjectRepository project, IProgrammerRepository programmer)
		{
			Field(t => t.Id, type: typeof(IdGraphType)).Description("User Profile Id");
			Field(t => t.Email);
			Field(t => t.UserName);
			Field(t => t.PhoneNumber);
			Field<ProfileType>(
				name: "profile",
				//arguments: new QueryArguments(new QueryArgument<IdGraphType> { Name = "id" }),
				resolve: context =>
				{
					//var profileId = context.GetArgument<Guid>("id");
					var profileId = context.Source.Id;
					
					return profile.GetProgrammerProfileAsync(profileId);
				}
			);
			Field<ListGraphType<ProjectType>>(
				name: "projects",
				resolve: context => programmer.GetAllProjectsByProgrammerAsync(context.Source.Id)

			);
		}

	}
}
