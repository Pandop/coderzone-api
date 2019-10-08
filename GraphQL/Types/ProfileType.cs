using CoderzoneGrapQLAPI.Models;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoderzoneGrapQLAPI.GraphQL.Types
{
	public class ProfileType : ObjectGraphType<Profile>
	{
		public ProfileType()
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
		}
	}
}
