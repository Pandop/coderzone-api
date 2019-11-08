using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQL.Types;

namespace CoderzoneGrapQLAPI.GraphQL.Mutations.otherTypes
{
	public class OrderBy
	{
		public string Path { get; set; }
		public bool? Descending { get; set; }
	}

	public class OrderGraph : InputObjectGraphType<OrderBy>
	{
		public OrderGraph()
		{
			Field(x => x.Path)
				.Description("The field to order by");
			Field(x => x.Descending, true)
				.Description("Weather or not the field is descending");
		}
	}
}
