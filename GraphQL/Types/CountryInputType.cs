using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoderzoneGrapQLAPI.GraphQL.Types
{
	public class CountryInputType : InputObjectGraphType
	{
		public CountryInputType()
		{
			Name = "CountryInputType";
			Field<NonNullGraphType<StringGraphType>>("name");
		}
	}
}
