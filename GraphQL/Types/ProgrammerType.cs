﻿using CoderzoneGrapQLAPI.Models;
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
				resolve: context => profile.GetProgrammerProfileAsync(context.Source.Id)
			);
			Field<CountryType>(
				name: "country",
				resolve: context => programmer.GetCountryForProgrammerAsync(context.Source.Id)
			);
			Field<ListGraphType<ProjectType>>(
				name: "projects",
				resolve: context => programmer.GetAllProjectsByProgrammerAsync(context.Source.Id)

			);
			Field<ListGraphType<WorkExperienceType>>(
				name: "works",
				resolve: context => programmer.GetAllWorkExperiencesByProgrammerAsync(context.Source.Id)

			);
			Field<ListGraphType<QualificationType>>(
				name: "qualifications",
				resolve: context => programmer.GetAllQualificationsByProgrammerAsync(context.Source.Id)

			);
			Field<ListGraphType<SkillType>>(
				name: "skills",
				resolve: context => programmer.GetAllSkillsByProgrammerAsync(context.Source.Id)

			);
		}

	}
}
