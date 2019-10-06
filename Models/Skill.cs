using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoderzoneGrapQLAPI.Models
{
	public class Skill
	{
		[Key]
		public Guid Id { get; set; }

		[Required]
		[StringLength(200, MinimumLength = 5, ErrorMessage = "Skill must be between 5 and 200 characters")]
		public string Name { get; set; }

		public Programmer Programmer { get; set; }
	}
}
