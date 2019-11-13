using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQL.Types;

namespace CoderzoneGrapQLAPI.helpers
{
	public class AuditReadData
	{
		public static AuditReadData FromGraphqlContext<T>(ResolveFieldContext<T> context)
		{
			return new AuditReadData
			{
				Arguments = context.Arguments,
				Query = context.Document.OriginalQuery,
				Variables = context.Variables,
				QueryName = context.FieldName,
			};
		}

		public string Query { get; set; }
		public Dictionary<string, object> Arguments { get; set; }
		public object Variables { get; set; }
		public string QueryName { get; set; }
	}
}
