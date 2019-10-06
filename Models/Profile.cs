using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CoderzoneGrapQLAPI.Models
{
	public class Profile
	{
		[Key]
		public Guid Id { get; set; }

		[Required]
		[StringLength(200, MinimumLength = 2, ErrorMessage = "First Name must be between 2 and 200 characters")]
		public string FirstName { get; set; }

		[Required]
		[StringLength(200, MinimumLength = 2, ErrorMessage = "Last Name must be between 2 and 200 characters")]
		public string LastName { get; set; }

		public string Avatar { get; set; }

		[Required]
		[StringLength(2000, MinimumLength = 50, ErrorMessage = "Bio must be between 50 and 2000 characters")]
		public string Bio { get; set; }

		[Required]
		[StringLength(200, MinimumLength = 5, ErrorMessage = "City Name must be between 5 and 200 characters")]
		public string City { get; set; }

		[Required]
		[StringLength(300, MinimumLength = 5, ErrorMessage = "Street Name must be between 5 and 300 characters")]
		public string Street { get; set; }

		[Required]
		public int Number { get; set; }

		[DataType(DataType.Date)]
		public DateTime DatePublished { get; set; }

		[DataType(DataType.Date)]
		public DateTime CreatedAt { get; set; }

		[DataType(DataType.Date)]
		public DateTime UpdatedAt { get; set; }

		//public Programmer Programmer { get; set; }
	}
}
