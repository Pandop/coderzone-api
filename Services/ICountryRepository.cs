using CoderzoneGrapQLAPI.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoderzoneGrapQLAPI.Services
{
	public interface ICountryRepository
	{
		// READ OPERATIONS
		Task<IEnumerable<Country>> GetCountriesAsync();
		Task<Country> GetCountryAsync(Guid countryId);
		Task<Country> GetCountryOfUserAsync(Guid userId);
		Task<Country> GetCountryOfStateAsync(Guid stateId);
		Task<IEnumerable<Programmer>> GetUsersForCountryAsync(Guid countryId);
		Task<IEnumerable<State>> GetStatesForCountryAsync(Guid countryId);
		Task<bool> CountryExistsAsync(Guid countryId);
		Task<bool> IsDuplicateCountryNameAsync(Guid countryId, string countryName);

		// CREATE | UPDATE | DELETE OPERATIONS
		Task<bool> AddCountryAsync(Country country);
		Task<bool> UpdateCountryAsync(Country country);
		Task<bool> DeleteCountryAsync(Country country);
		Task<bool> SaveAsync();
	}
}
