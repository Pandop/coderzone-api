using IdentityModel;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoderzoneGrapQLAPI.Models
{
	public class User : IdentityUser<Guid>
	{
		public override string PasswordHash { get; set; }
		public Profile Profile { get; set; }
		public State State { get; set; }
		public Country Country { get; set; }
	}
}
