using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphiQl;
using GraphQL;
using GraphQL.Server;
using GraphQL.Types;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CoderzoneGrapQLAPI
{
	public class Startup
	{
		public IConfiguration CurrentEnvironment { get; set; }
		public IConfiguration Configuration { get; }

		public Startup(IHostingEnvironment env, IConfiguration configuration) { }
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}


		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
		{
			// add mvc 
			services.AddMvc();

			// Add GraphQL
			services.AddGraphQL();
			//services.AddSingleton<IDocumentExecuter, DocumentExecuter>();
			//services.AddSingleton<IDependencyResolver>(
			//	provider => new FuncDependencyResolver(provider.GetRequiredService)
			//);

			// Add the schema and query for graphql

			// Send our db context to graphql to use
			//var optionsBuilder = new DbContextOptionsBuilder<CsharpReferenceDBContext>().UseNpgsql(dbConnectionString);
			//using (var context = new CsharpReferenceDBContext(optionsBuilder.Options, null))
			//{
			//	EfGraphQLConventions.RegisterInContainer(services, context.Model);
			//}

			// add DBContext connection

			// add services Interfaces
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			// use graphQLHttp
			app.UseGraphiQl();


			app.UseMvc();

			// seed the databse
		}
	}
}
