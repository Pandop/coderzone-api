using CoderzoneGrapQLAPI.Models;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoderzoneGrapQLAPI.GraphQL.Types
{
	public class SkillType : ObjectGraphType<Skill>
	{
		public SkillType()
		{
			Field(t => t.Id, type: typeof(IdGraphType)).Description("Id of Skill");
			Field(t => t.Name);
		}
	}
}
