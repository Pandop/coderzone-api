using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoderzoneGrapQLAPI.Models;

namespace CoderzoneGrapQLAPI.Services
{
	public class CountryRepository : ICountryRepository
	{
		private readonly CoderzoneApiDbContext _countryContext;

		public CountryRepository(CoderzoneApiDbContext countryContext)
		{
			_countryContext = countryContext;
		}
		public Task<bool> CountryExistsAsync(Guid CountryId)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<Country>> GetCountriesAsync()
		{
			return Task.FromResult(_countryContext.Countries.AsEnumerable());
		}

		public Task<Country> GetCountryAsync(Guid CountryId)
		{
			throw new NotImplementedException();
		}

		public Task<Country> GetCountryOfStateAsync(Guid stateId)
		{
			throw new NotImplementedException();
		}

		public Task<Country> GetCountryOfUserAsync(Guid userId)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<State>> GetStatesForCountryAsync(Guid countryId)
		{
			return Task.FromResult(_countryContext.States.Where(s=> s.Country.Id==countryId).AsEnumerable());
		}

		public Task<IEnumerable<Programmer>> GetUsersForCountryAsync(Guid CountryId)
		{
			throw new NotImplementedException();
		}

		public Task<bool> IsDuplicateCountryName(Guid countryId, string countryName)
		{
			throw new NotImplementedException();
		}
	}
}
