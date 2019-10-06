using CoderzoneGrapQLAPI.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoderzoneGrapQLAPI.Services
{
	public interface ICountryRepository
	{
		Task<IEnumerable<Country>> GetCountriesAsync();
		Task<Country> GetCountryAsync(Guid CountryId);
		Task<Country> GetCountryOfUserAsync(Guid userId);
		Task<Country> GetCountryOfStateAsync(Guid stateId);
		Task<IEnumerable<Programmer>> GetUsersForCountryAsync(Guid CountryId);
		Task<IEnumerable<State>> GetStatesForCountryAsync(Guid CountryId);
		Task<bool> CountryExistsAsync(Guid CountryId);
		Task<bool> IsDuplicateCountryName(Guid countryId, string countryName);
	}
}
