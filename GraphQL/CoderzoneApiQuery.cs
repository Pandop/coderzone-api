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
			Name = "Query";
			Field<ListGraphType<CountryType>>(
				name: "countries",
				resolve: context => country.GetCountriesAsync()
				);
			
			Field<ListGraphType<ProgrammerType>>(
				name: "programmers",
				resolve: context => programmer.GetProgrammersAsync()
				);

			Field<ProgrammerType>(
				name: "programmer",
				arguments: new QueryArguments(new QueryArgument<IdGraphType> { Name = "id" }),
				resolve: context =>
					{
						Guid programmerId;
						if (!Guid.TryParse(context.GetArgument<string>("id"), out programmerId))
						{
							context.Errors.Add(new ExecutionError($"Wrong value for guid: {programmerId}"));
							return null;
						}
						return programmer.GetProgrammerAsync(programmerId);
					}
				);
		}
	}
}
