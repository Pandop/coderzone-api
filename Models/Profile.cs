using System;
using System.Collections.Generic;

namespace CoderzoneGrapQLAPI.Models
{
	public class Profile
	{
		public Guid Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Avatar { get; set; }
		public string Description { get; set; }
		public string City { get; set; }
		public string Street { get; set; }
		public int StreetNumber { get; set; }
		public DateTime DatePublished { get; set; }
		public DateTime CreatedAt { get; set; }		
		public User	User { get; set; }
		public ICollection<string> TechStack { get; set; }
		public ICollection<string> Skills { get; set; }
	}
}
