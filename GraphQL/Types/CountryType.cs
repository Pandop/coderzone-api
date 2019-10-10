using CoderzoneGrapQLAPI.Models;
using CoderzoneGrapQLAPI.Services;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoderzoneGrapQLAPI.GraphQL.Types
{
	public class CountryType : ObjectGraphType<Country>
	{
		public CountryType(ICountryRepository country)
		{
			Field(c => c.Id, type: typeof(IdGraphType)).Description("Id of the Country");
			Field(c => c.Name);
			Field<ListGraphType<StateType>>(
				"states",
				resolve: context => country.GetStatesForCountryAsync(context.Source.Id)
				); 
		}
	}
}
