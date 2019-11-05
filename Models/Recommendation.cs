using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoderzoneGrapQLAPI.Models
{
	public class Recommendation
	{
		[Key]
		public Guid Id { get; set; }
		//public Programmer Programmer { get; set; }
		//public Profile Profile { get; set; }
		public string Description { get; set; }
	}
}
