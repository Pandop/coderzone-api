using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQL.Types;

namespace CoderzoneGrapQLAPI.GraphQL.Mutations.otherTypes
{
	public class IdObject
	{
		public Guid Id { get; set; }

		public static List<IdObject> FromList(IEnumerable<Guid> ids)
		{
			return ids.Select(o => new IdObject { Id = o }).ToList();
		}
	}

	public class IdObjectType : ObjectGraphType<IdObject>
	{
		public IdObjectType()
		{
			Field<IdGraphType>(
				"Id",
				resolve: o => o.Source.Id,
				description: "An ID in the form of a GUID"
			);
		}
	}
}
