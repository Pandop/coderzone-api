using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoderzoneGrapQLAPI.helpers
{
	public class Coder : IdentityUser<Guid>
	{
		public Guid Owner { get; set; }

		[Required]
		public override string UserName { get; set; }

		[Email]
		public override string Email { get; set; }

		//[AuditIgnore]
		public override string PasswordHash { get; set; }

	}

	public class Group : IdentityRole<Guid>
	{
		public bool? HasBackendAccess { get; set; }
	}

	public class UserConfiguration : IEntityTypeConfiguration<Coder>
	{
		public void Configure(EntityTypeBuilder<Coder> builder)
		{
			builder
				.Property(e => e.Id)
				.HasDefaultValueSql("uuid_generate_v4()");
		}
	}

	public class GroupConfiguration : IEntityTypeConfiguration<Group>
	{
		public void Configure(EntityTypeBuilder<Group> builder)
		{
			builder
				.Property(e => e.Id)
				.HasDefaultValueSql("uuid_generate_v4()");
		}
	}
}
