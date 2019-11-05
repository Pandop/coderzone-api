using CoderzoneGrapQLAPI.Models;
using GraphQL.Types;

namespace CoderzoneGrapQLAPI.GraphQL.Types
{
	public class ProjectType: ObjectGraphType<Project>
	{
		public ProjectType()
		{
			Field(t => t.Id, type: typeof(IdGraphType)).Description("Id of Project");
			Field(t => t.Name);
			Field(t => t.Description);
			Field(t => t.ImageUrl);
		}
	}
}