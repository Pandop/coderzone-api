using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQL.Types;

namespace CoderzoneGrapQLAPI.GraphQL.Mutations.otherTypes
{
	public class NumberObject
	{
		public int Number { get; set; }

	}

	public class NumberObjectType : ObjectGraphType<NumberObject>
	{
		public NumberObjectType()
		{
			Field<IntGraphType>(
				"Number",
				resolve: o => o.Source.Number,
				description: "The total number"
			);
		}
	}
}
