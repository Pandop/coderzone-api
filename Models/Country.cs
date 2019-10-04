using System;
using System.Collections.Generic;

namespace CoderzoneGrapQLAPI.Models
{
	public class Country
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public ICollection<State> States { get; set; }
		public ICollection<User> Users { get; set; }
	}
}
