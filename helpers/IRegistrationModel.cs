using System.Collections.Generic;
using CsharpReference.Services;

namespace CoderzoneGrapQLAPI.helpers
{
	public interface IRegistrationModel<T>
		where T : Coder
	{
		string Email { get; set; }
		string Password { get; set; }

		IList<string> Groups { get; }

		T ToModel();
	}
}