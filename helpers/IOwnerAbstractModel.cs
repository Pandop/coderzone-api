using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CoderzoneGrapQLAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CsharpReference.Services
{
	public class EntityForeignKey : Attribute
	{
		public string Name { get; }
		public string OppositeName { get; }
		public Type OppositeEntity { get; }

		public EntityForeignKey(string name, string oppositeName, Type oppositeEntity)
		{
			Name = name;
			OppositeName = oppositeName;
			OppositeEntity = oppositeEntity;
		}
	}
	public interface IAbstractModel
	{
		Guid Id { get; set; }
		DateTime Created { get; set; }
		DateTime Modified { get; set; }
	}

	public static class AbstractModelExtensions
	{
		public static bool ValidateObjectFields(this object abstractModel, List<ValidationResult> errors)
		{
			var context = new ValidationContext(abstractModel, serviceProvider: null, items: null);
			return Validator.TryValidateObject(abstractModel, context, errors, validateAllProperties: true);
		}
	}

	public interface IOwnerAbstractModel : IAbstractModel
	{
		Guid Owner { get; set; }
		IEnumerable<IAcl> Acls { get; }

		int CleanReference<T>(string reference, IEnumerable<T> models, CoderzoneApiDbContext dbContext)
			where T : IOwnerAbstractModel;
	}

	//public class AbstractModelConfiguration
	//{
	//	public static void Configure<T>(EntityTypeBuilder<T> builder)
	//		where T : class, IAbstractModel
	//	{
	//		// Configuration for a POSTGRES database
	//		builder
	//			.Property(e => e.Id)
	//			.HasDefaultValueSql("uuid_generate_v4()");
	//		builder
	//			.Property(e => e.Created)
	//			.HasDefaultValueSql("CURRENT_TIMESTAMP");
	//		builder
	//			.Property(e => e.Modified)
	//			.HasDefaultValueSql("CURRENT_TIMESTAMP");
	//	}
	//}
}