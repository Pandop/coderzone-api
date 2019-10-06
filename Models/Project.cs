using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoderzoneGrapQLAPI.Models
{
	public class Project
	{
		[Key]
		public Guid Id { get; set; }

		[Required]
		[StringLength(200, MinimumLength = 5, ErrorMessage = "Project Name must be between 5 and 200 characters")]
		public string Name { get; set; }

		[Required]
		[StringLength(2000, MinimumLength = 50, ErrorMessage = "Description must be between 50 and 2000 characters")]
		public string Description { get; set; }

		public string ImageUrl { get; set; }
		public Programmer Programmer { get; set; }
		//public string[] TechStacks { get; set; }
	}
}
