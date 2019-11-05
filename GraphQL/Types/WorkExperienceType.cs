using CoderzoneGrapQLAPI.Models;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoderzoneGrapQLAPI.GraphQL.Types
{
	public class WorkExperienceType : ObjectGraphType<WorkExperience>
	{
		public WorkExperienceType()
		{
			Field(t => t.Id, type: typeof(IdGraphType)).Description("Id of Work Experience");
			Field(t => t.Title);
			Field(t => t.Description);
			Field(t => t.StartDate);
			Field(t => t.EndDate);
		}
	}
}
