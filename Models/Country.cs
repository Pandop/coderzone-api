using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CoderzoneGrapQLAPI.Models
{
	public class Country
	{
		[Key]
		public Guid Id { get; set; }

		[Required]
		[StringLength(250, MinimumLength = 5, ErrorMessage = "First Name must be between 5 and 250 characters")]
		public string Name { get; set; }

		public virtual ICollection<State> States { get; set; }
		public virtual ICollection<Programmer> Programmers { get; set; }
	}
}
