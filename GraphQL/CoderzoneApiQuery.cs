using CoderzoneGrapQLAPI.controllers;
using CoderzoneGrapQLAPI.GraphQL.Types;
using CoderzoneGrapQLAPI.Services;
using GraphQL;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoderzoneGrapQLAPI.GraphQL
{
	public class CoderzoneApiQuery : ObjectGraphType
	{
		public CoderzoneApiQuery(ICountryRepository country, IProgrammerRepository programmer)
		{
			Field<ListGraphType<CountryType>>(
				name: "countries",
				resolve: context =>
				{
					//var userContext = (CodersGraphQLContext)context.UserContext;
					//userContext.DbContext
					return country.GetCountriesAsync();
				}
				);
			
			Field<ListGraphType<ProgrammerType>>(
				name: "programmers",
				resolve: context => programmer.GetProgrammersAsync()
				);

			Field<ProgrammerType>(
				name: "programmer",
				arguments: new QueryArguments(new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "id" }),
				resolve: context => programmer.GetProgrammerAsync(context.GetArgument<Guid>("id"))
				);
		}
	}
}
