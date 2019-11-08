using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQL.Types;

namespace CoderzoneGrapQLAPI.GraphQL.Mutations.otherTypes
{
	public class BooleanObject
	{
		public bool Value { get; set; }
	}

	public class BooleanObjectType : ObjectGraphType<BooleanObject>
	{
		public BooleanObjectType()
		{
			Field<BooleanGraphType>(
				"value",
				resolve: o => o.Source.Value,
				description: "The value of the boolean"
			);
		}
	}
}
