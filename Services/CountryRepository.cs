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

		// READ OPERATIONS
		public Task<IEnumerable<Country>> GetCountriesAsync()
		{
			return Task.FromResult(_countryContext.Countries.AsEnumerable());
		}

		public Task<Country> GetCountryAsync(Guid countryId)
		{
			if(countryId ==Guid.Empty)
				throw new ArgumentNullException(nameof(countryId));

			return Task.FromResult(_countryContext.Countries.FirstOrDefault(c => c.Id == countryId));
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

		public Task<IEnumerable<Programmer>> GetUsersForCountryAsync(Guid countryId)
		{
			return Task.FromResult(_countryContext.Programmers.Where(c => c.Country.Id== countryId).AsEnumerable());
		}

		public Task<bool> CountryExistsAsync(Guid countryId)
		{
			// countryId is null or empty
			if (countryId == Guid.Empty)
				throw new ArgumentNullException(nameof(countryId));

			return Task.FromResult(_countryContext.Profiles.Any(a => a.Id == countryId));
		}

		public Task<bool> IsDuplicateCountryName(Guid countryId, string countryName)
		{
			// countryId is null or empty
			if (countryId == Guid.Empty)
				throw new ArgumentNullException(nameof(countryId));

			return Task.FromResult(_countryContext.Countries.Any(c => c.Name.ToLower() == countryName.ToLower() && c.Id!=countryId));
		}


		// CREATE | UPDATE | DELETE OPERATIONS
		public Task<bool> AddCountry(Country country)
		{
			// Add country Object to country context and save it
			_countryContext.Add(country);
			return Save();
		}

		public Task<bool> UpdateCountry(Country country)
		{
			// Update country Object to country context and save it
			_countryContext.Update(country);
			return Save();
		}

		public Task<bool> DeleteCountry(Country country)
		{
			// Remove country Object to country context and save it
			_countryContext.Remove(country);
			return Save();
		}

		public Task<bool> Save() => Task.FromResult(_countryContext.SaveChanges() >= 0 ? true : false);
	}
}
