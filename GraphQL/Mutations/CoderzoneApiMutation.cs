using CoderzoneGrapQLAPI.GraphQL.Types;
using CoderzoneGrapQLAPI.Models;
using CoderzoneGrapQLAPI.Services;
using GraphQL;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoderzoneGrapQLAPI.GraphQL.Mutations
{
	public class CoderzoneApiMutation : ObjectGraphType
	{
		public CoderzoneApiMutation(ICountryRepository countryRepository)
		{
			///TODO:
			FieldAsync<CountryType>(
				Name = "AddCountry",
				arguments: new QueryArguments(new QueryArgument<NonNullGraphType<CountryInputType>> { Name = "country" }),
				resolve: async context=>
				{
					var countryToCreate = context.GetArgument<Country>("country");
					// Make sure country is not already in the database
					var countryInDb = countryRepository.GetCountriesAsync().Result.FirstOrDefault(c=> c.Name.ToLower()== countryToCreate.Name.ToLower());
					if(countryInDb !=null)
					{
						context.Errors.Add(new ExecutionError($"The Country '{countryToCreate.Name}' already exists!"));
						return null;
					}
					// Make sure a country was added successfully to database
					if (!await countryRepository.AddCountry(countryToCreate))
					{
						context.Errors.Add(new ExecutionError($"Something went wrong saving {countryToCreate.Name}"));
						return context.Errors;
					}
					return countryToCreate;
				}
			);
		}
	}
}
