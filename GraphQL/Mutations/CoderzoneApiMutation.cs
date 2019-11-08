using CoderzoneGrapQLAPI.GraphQL.Types;
using CoderzoneGrapQLAPI.Models;
using CoderzoneGrapQLAPI.Services;
using GraphQL;
using GraphQL.Types;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace CoderzoneGrapQLAPI.GraphQL.Mutations
{
	public class CoderzoneApiMutation : ObjectGraphType
	{
		public CoderzoneApiMutation(ICountryRepository countryRepository, IStateRepository stateRepository)
		{
			///TODO:
			FieldAsync<CountryType>(
				Name = "AddCountry",
				arguments: new QueryArguments(new QueryArgument<NonNullGraphType<CountryInputType>> { Name = "country" }),
				resolve: async context =>
				{
					var countryToCreate = context.GetArgument<Country>("country");
					// Make sure country is not already in the database
					var countryInDb = countryRepository.GetCountriesAsync().Result.FirstOrDefault(c => string.Equals(c.Name, countryToCreate.Name, StringComparison.OrdinalIgnoreCase));
					if (countryInDb != null)
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
				Name = "UpdateCountry",
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
					var countryInfoToUpdateOld = await countryRepository.GetCountryAsync(countryId);
					if (countryInfoToUpdateOld == null)
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
					countryInfoToUpdateOld.Name = countryInfoToUpdate.Name;
					if (!await countryRepository.UpdateCountryAsync(countryInfoToUpdateOld))
					{
						context.Errors.Add(new ExecutionError($"Something went wrong Updating {countryInfoToUpdate.Name}"));
						return null;
					}
					// If all is good
					return countryInfoToUpdate;
				}
			);
			FieldAsync<CountryType>(
				Name = "RemoveCountry",
				arguments: new QueryArguments(new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "countryId" }),
				resolve: async context =>
				{
					var countryId = context.GetArgument<Guid>("countryId");

					var countryToDelete = await countryRepository.GetCountryAsync(countryId);

					// Country does not exist in db
					if (countryToDelete == null)
					{
						context.Errors.Add(new ExecutionError($"Country with ID {countryId} does not exist!"));
						return null;
					}

					// Check that country does not have a state or programmer associated with it
					var states = await countryRepository.GetStatesForCountryAsync(countryId);
					var programmers = await countryRepository.GetUsersForCountryAsync(countryId);
					if (states.Count() > 0 || programmers.Count() > 0)
					{
						context.Errors.Add(new ExecutionError($"{countryToDelete.Name} cannot be deleted as it's used by at least {programmers.Count()} programmers or {states.Count()} states!"));
						return null;
					}

					// At stage, we can delete the country
					if (!await countryRepository.DeleteCountryAsync(countryToDelete))
					{
						context.Errors.Add(new ExecutionError($"Something went wrong deleting {countryToDelete.Name}"));
						return null;
					}
					return countryToDelete;
				}
			);

			///MUTATIONS FOR STATES
			FieldAsync<StateType>(
				Name = "AddState",
				arguments: new QueryArguments(new QueryArgument<NonNullGraphType<StateInputType>> { Name = "state" }),
				resolve: async context =>
				{
					var stateToCreate = context.GetArgument<State>("state");
					// Make sure the country exists in database
					var stateCountry = await countryRepository.GetCountryAsync(stateToCreate.CountryId);
					if (stateCountry == null)
					{
						context.Errors.Add(new ExecutionError($"'{stateToCreate.Name}' does not exists!"));
						return null;
					}

					// Make sure state is not already in the database
					var stateInDatabase = stateRepository.GetStatesAsync()
											.Result.FirstOrDefault(c => string.Equals(c.Name, stateToCreate.Name, StringComparison.OrdinalIgnoreCase) && c.CountryId == stateCountry.Id);
					if (stateInDatabase != null)
					{
						context.Errors.Add(new ExecutionError($"The State '{stateToCreate.Name}' already exists!"));
						return null;
					}
					// Make sure a country was added successfully to database
					//stateToCreate.Country.Id = stateToCreate.CountryId;

					if (!await stateRepository.AddCountryAsync(stateToCreate))
					{
						context.Errors.Add(new ExecutionError($"Something went wrong saving {stateToCreate.Name}"));
						return null;
					}
					return stateToCreate;
				}
			);

		}

	}
}