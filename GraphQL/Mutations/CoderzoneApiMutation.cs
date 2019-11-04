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
					var countryInDb = countryRepository.GetCountriesAsync().Result.FirstOrDefault(c=> string.Equals(c.Name, countryToCreate.Name, StringComparison.OrdinalIgnoreCase));
					if(countryInDb !=null)
					{
						context.Errors.Add(new ExecutionError($"The Country '{countryToCreate.Name}' already exists!"));
						return null;
					}
					// Make sure a country was added successfully to database
					if (!await countryRepository.AddCountryAsync(countryToCreate))
					{
						context.Errors.Add(new ExecutionError($"Something went wrong saving {countryToCreate.Name}"));
						return null;
					}
					return countryToCreate;
				}
			);
			FieldAsync<CountryType>(
				Name="UpdateCountry",
				arguments: new QueryArguments(
					new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "countryId" },
					new QueryArgument<NonNullGraphType<CountryInputType>> { Name = "country" }),
				resolve: async context =>
				{
					var countryId = context.GetArgument<Guid>("countryId");
					var countryInfoToUpdate = context.GetArgument<Country>("country");

					// country Ids do not match
					if (countryId != countryInfoToUpdate.Id)
					{
						context.Errors.Add(new ExecutionError($"Bad Request!"));
						return null;
					}
					// Country does not exist in database
					//var ctup = await countryRepository.CountryExistsAsync(countryId);
					if (!await countryRepository.CountryExistsAsync(countryId))
					{
						context.Errors.Add(new ExecutionError($"{countryInfoToUpdate.Name} already exists!"));
						return null;
					}
					// Country is a duplicate country
					if (await countryRepository.IsDuplicateCountryNameAsync(countryId, countryInfoToUpdate.Name))
					{
						context.Errors.Add(new ExecutionError($"The Country '{countryInfoToUpdate.Name}' already exists!"));
						return null;
					}

					// Now try to update the country
					//countryInfoToUpdate.Id = countryId;
					if (!await countryRepository.UpdateCountryAsync(countryInfoToUpdate))
					{
						context.Errors.Add(new ExecutionError($"Something went wrong Updating {countryInfoToUpdate.Name}"));
						return null;
					}
					// If all is good
					return countryInfoToUpdate;
				}
			);

		}
	}
}
