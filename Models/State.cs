using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CoderzoneGrapQLAPI.Models
{
	public class State
	{
		[Key]
		public Guid Id { get; set; }

		[Required]
		[StringLength(100, MinimumLength = 2, ErrorMessage = "State must be between 2 and 100 characters")]
		public string Name { get; set; }

		[Display(Name = "Post Code")]
		[RegularExpression(@"^\d{5}(-\d{4})?$", ErrorMessage = "Invalid Post Code")]
		public string PostCode { get; set; }

		public Country Country { get; set; }
		public virtual ICollection<Programmer> Programmer { get; set; }

	}
}
