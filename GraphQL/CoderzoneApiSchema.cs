using CoderzoneGrapQLAPI.GraphQL.Mutations;
using GraphQL;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoderzoneGrapQLAPI.GraphQL
{
	public class CoderzoneApiSchema : Schema
	{
		public CoderzoneApiSchema(IDependencyResolver resolver) : base(resolver)
		{
			// Query
			Query = resolver.Resolve<CoderzoneApiQuery>();

			// Mutation
			Mutation = resolver.Resolve<CoderzoneApiMutation>();

			// Subscription
		}
	}
}
