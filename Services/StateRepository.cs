using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoderzoneGrapQLAPI.Models;

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

		public Task<bool> IsDuplicateStateName(Guid stateId, string stateName)
		{
			throw new NotImplementedException();
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

		public Task<bool> UpdateStateAsync(State state)
		{
			throw new NotImplementedException();
		}

		public async Task<bool> SaveAsync() => await _stateContext.SaveChangesAsync() >= 0 ? true : false;
		
	}
}
