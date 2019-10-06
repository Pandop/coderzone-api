using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoderzoneGrapQLAPI.Models
{
	public class WorkExperience
	{
		[Key]
		public Guid Id { get; set; }

		[Required]
		[StringLength(200, MinimumLength = 5, ErrorMessage = "Title must be between 5 and 200 characters")]
		public string Title { get; set; }

		[Required]
		[StringLength(2000, MinimumLength = 50, ErrorMessage = "Description must be between 50 and 2000 characters")]
		public string Description { get; set; }

		[Display(Name = "Started Date")]
		[DataType(DataType.Date)]
		public DateTime StartDate { get; set; }

		[Display(Name = "Finished Date")]
		[DataType(DataType.Date)]
		public DateTime EndDate { get; set; }

		public Programmer Programmer { get; set; }
	}
}
