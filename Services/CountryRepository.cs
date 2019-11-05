using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoderzoneGrapQLAPI.Models;
using Microsoft.EntityFrameworkCore;

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

		public async Task<bool> CountryExistsAsync(Guid countryId)
		{
			// countryId is null or empty
			if (countryId == Guid.Empty)
				throw new ArgumentNullException(nameof(countryId));

			return await _countryContext.Countries.AnyAsync(c => c.Id == countryId);
		}

		public async Task<bool> IsDuplicateCountryNameAsync(Guid countryId, string countryName)
		{
			// countryId is null or empty
			if (countryId == Guid.Empty)
				throw new ArgumentNullException(nameof(countryId));

			return await _countryContext.Countries.AnyAsync(c => c.Name.Equals(countryName) && c.Id == countryId);
		}


		// CREATE | UPDATE | DELETE OPERATIONS
		public async Task<bool> AddCountryAsync(Country country)
		{
			// Add country Object to country context and save it
			_countryContext.Add(country);
			return await SaveAsync();
		}

		public Task<bool> UpdateCountryAsync(Country country)
		{
			// Update country Object to country context and save it
			_countryContext.Update(country);
			return SaveAsync();
		}

		public async Task<bool> DeleteCountryAsync(Country country)
		{
			// Remove country Object to country context and save it
			_countryContext.Remove(country).State = EntityState.Modified;
			return await SaveAsync();
		}

		public async Task<bool> SaveAsync() => await _countryContext.SaveChangesAsync() >= 0 ? true : false;
	}
}
