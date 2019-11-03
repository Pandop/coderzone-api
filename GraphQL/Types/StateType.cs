using CoderzoneGrapQLAPI.Models;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoderzoneGrapQLAPI.GraphQL.Types
{
	public class StateType : ObjectGraphType<State>
	{
		public StateType()
		{
			Field(t => t.Id, type: typeof(IdGraphType)).Description("Id for State");
			Field(t => t.Name);
			Field(t => t.PostCode);
		}
	}
}
