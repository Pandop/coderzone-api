using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoderzoneGrapQLAPI.Services;
using Microsoft.AspNetCore.Identity;

namespace CoderzoneGrapQLAPI.helpers
{
	public class SecurityContext
	{
		public CoderzoneApiDbContext DbContext { get; set; }
		public UserManager<Coder> UserManager { get; set; }
		public IList<string> Groups { get; set; }
	}
}
