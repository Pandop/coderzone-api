using CoderzoneGrapQLAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoderzoneGrapQLAPI.Services
{
	public class CoderzoneApiDbContext : DbContext
	{
		public CoderzoneApiDbContext(DbContextOptions<CoderzoneApiDbContext> options) : base(options)
		{
			// run migration at app starts
			Database.Migrate();
		}

		// Tell EF what tables what tables it need to set up
		public virtual DbSet<Profile> Profiles { get; set; }
		public virtual DbSet<Country> Countries { get; set; }
		public virtual DbSet<State> States { get; set; }
		public virtual DbSet<Skill> Skills { get; set; }
		public virtual DbSet<Project> Projects { get; set; }
		public virtual DbSet<Qualification> Qualifications { get; set; }
		public virtual DbSet<WorkExperience> WorkExperiences { get; set; }
		public virtual DbSet<Programmer> Programmers { get; set; }
		//public virtual DbSet<Recommendation> Recommendation { get; set; }

		// Set up many to many relationships if needed
		//protected override void OnModelCreating(ModelBuilder modelBuilder){}
	}
}
