using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoderzoneGrapQLAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CoderzoneGrapQLAPI.Services
{
	public class StateRepository : IStateRepository
	{
		private readonly CoderzoneApiDbContext _stateContext;

		public StateRepository(CoderzoneApiDbContext stateContext)
		{
			_stateContext = stateContext;
		}
		public Task<State> GetStateAsync(Guid stateId)
		{
			if (stateId == Guid.Empty)
				throw new ArgumentNullException(nameof(stateId));

			return Task.FromResult(_stateContext.States.SingleOrDefault(s => s.Id == stateId));
		}
		public Task<IEnumerable<State>> GetStatesAsync()
		{
			return Task.FromResult(_stateContext.States.AsEnumerable());
		}

		public async Task<bool> IsDuplicateStateName(Guid stateId, string stateName)
		{
			// stateId is null or empty
			if (stateId == Guid.Empty)
				throw new ArgumentNullException(nameof(stateId));

			return await _stateContext.States.AnyAsync(s => s.Name.Equals(stateName) && s.Id == stateId);
		}

		public Task<bool> StateExistsAsync(Guid stateId)
		{
			throw new NotImplementedException();
		}

		public async Task<bool> AddCountryAsync(State state)
		{
			_stateContext.Add(state);
			return await SaveAsync();
		}

		public Task<bool> DeleteStateAsync(State state)
		{
			throw new NotImplementedException();
		}

		public async Task<bool> UpdateStateAsync(State state)
		{
			_stateContext.Update(state);
			return await SaveAsync();
		}

		public async Task<bool> SaveAsync() => await _stateContext.SaveChangesAsync() >= 0 ? true : false;
		
	}
}
