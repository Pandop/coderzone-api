using CoderzoneGrapQLAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoderzoneGrapQLAPI.Services
{
	public interface IStateRepository
	{
		Task<ICollection<State>> GetStatesAsync();
		Task<State> GetStateAsync(Guid stateId);
		Task<Country> GetCountryForStateAsync(Guid countryId);
		Task<User> GetUserFromStateAsync(Guid userId);
		Task<User> GetAllUserFromStateAsync(Guid stateId);
		Task<bool> StateExistsAsync(Guid stateId);
		Task<bool> IsDuplicateStateName(Guid stateId, string stateName);
	}
}
