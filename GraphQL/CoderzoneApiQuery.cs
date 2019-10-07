using CoderzoneGrapQLAPI.GraphQL.Types;
using CoderzoneGrapQLAPI.Services;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoderzoneGrapQLAPI.GraphQL
{
	public class CoderzoneApiQuery : ObjectGraphType
	{
		public CoderzoneApiQuery(ICountryRepository country)
		{
			Field<ListGraphType<CountryType>>(
				"countries",
				resolve: context => country.GetCountriesAsync()
				);
		}
	}
}
