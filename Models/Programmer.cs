using IdentityModel;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoderzoneGrapQLAPI.Models
{
	public class Programmer : IdentityUser<Guid>
	{
		[Key]
		public override Guid Id { get; set; }
		public override string PasswordHash { get; set; }
		public State State { get; set; }
		public Country Country { get; set; }
		public Profile Profile { get; set; }
		public virtual ICollection<Project> Projects { get; set; }
		public virtual ICollection<WorkExperience> WorkExperiences { get; set; }
		public virtual ICollection<Qualification> Qualifications { get; set; }
		public virtual ICollection<Skill> Skills { get; set; }
		//public virtual ICollection<ProgrammerCategory> ProgrammerCategories { get; set; }
		//Add
	}
}
