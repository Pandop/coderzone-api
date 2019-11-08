using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoderzoneGrapQLAPI.Models;
using CoderzoneGrapQLAPI.Services;
using GraphQL.Types;

namespace CoderzoneGrapQLAPI.GraphQL.Types
{
	public class StateInputType : InputObjectGraphType
	{
		public StateInputType()
		{
			Name = "StateInputType";
			Field<IdGraphType>("Id");
			Field<NonNullGraphType<StringGraphType>>("name");
			Field<StringGraphType>("PostCode");
			Field<IdGraphType>("CountryId");
			Field<CountryInputType>("Country");
		}
	}
}
