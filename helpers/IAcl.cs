using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using CoderzoneGrapQLAPI.helpers;

namespace CsharpReference.Services
{
	public interface IAcl
	{
		string Group { get; }

		Expression<Func<TModel, bool>> GetReadConditions<TModel>(Coder user, SecurityContext context)
			where TModel : IOwnerAbstractModel, new();
		Expression<Func<TModel, bool>> GetUpdateConditions<TModel>(Coder user, SecurityContext context)
			where TModel : IOwnerAbstractModel, new();
		Expression<Func<TModel, bool>> GetDeleteConditions<TModel>(Coder user, SecurityContext context)
			where TModel : IOwnerAbstractModel, new();
		bool GetCreate(Coder user, IEnumerable<IAbstractModel> models, SecurityContext context);
		bool GetUpdate(Coder user, IEnumerable<IAbstractModel> models, SecurityContext context);
		bool GetDelete(Coder user, IEnumerable<IAbstractModel> models, SecurityContext context);
	}
}