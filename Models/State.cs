using System;
using System.Collections.Generic;

namespace CoderzoneGrapQLAPI.Models
{
	public class State
	{
		public Guid Id { get; set; }
		public string Name { get; set; }		
		public string PostCode { get; set; }
		public Country Country { get; set; }
		public ICollection<User> Users { get; set; }

	}
}
