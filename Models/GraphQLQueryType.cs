using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoderzoneGrapQLAPI.Models
{
	public class GraphQLQueryType
	{
		public string OperationName { get; set; }
		public string Query { get; set; }
		public JObject Variables;
	}
}
