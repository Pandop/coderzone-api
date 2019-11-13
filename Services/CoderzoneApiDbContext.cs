﻿using CoderzoneGrapQLAPI.Models;
using CsharpReference.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace CoderzoneGrapQLAPI.Services
{
	public class CoderzoneApiDbContext : DbContext
	{
		public CoderzoneApiDbContext(DbContextOptions<CoderzoneApiDbContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
		{
			// run migration at app starts
			Database.Migrate();
			//_helper.SetConfig(this);
			SessionUser = httpContextAccessor?.HttpContext?.User?.FindFirst("UserId")?.Value;
			SessionId = httpContextAccessor?.HttpContext?.TraceIdentifier;
		}

		public string SessionUser { get; }
		public string SessionId { get; }
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
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder
				.Entity<Programmer>()
				.HasOne(c => c.Profile)
				.WithOne(p => p.Programmer);

			modelBuilder
				.Entity<Profile>()
				.HasOne(c => c.Programmer)
				.WithOne(p => p.Profile)
				.HasForeignKey<Profile>(p => p.ProgrammerId);

		}

		public DbSet<T> GetDbSet<T>(string name = null) where T : class, IAbstractModel
		{
			return GetType().GetProperty(name ?? typeof(T).Name).GetValue(this, null) as DbSet<T>;
		}

		public IQueryable GetOwnerDbSet(string name)
		{
			return GetType().GetProperty(name).GetValue(this, null) as IQueryable;
		}
	}
}
