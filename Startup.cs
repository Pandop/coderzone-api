using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoderzoneGrapQLAPI.DbSeeds;
using CoderzoneGrapQLAPI.GraphQL;
using CoderzoneGrapQLAPI.Services;
using CoderzoneGrapQLAPI.Api.Middlewares;
using GraphiQl;
using GraphQL;
using GraphQL.Http;
using GraphQL.Server;
using GraphQL.Types;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace CoderzoneGrapQLAPI
{
	public class Startup
	{
		public IHostingEnvironment _env { get; set; }
		public static IConfiguration Configuration { get; set; }

		//public Startup(IConfiguration configuration) { }
		public Startup(IHostingEnvironment env, IConfiguration configuration)
		{
			_env = env;
			Configuration = configuration;
		}


		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
		{
			// add mvc if needed
			services.AddMvc();

			// Setup DB Connections
			services.AddDbContext<CoderzoneApiDbContext>(options => options.UseSqlServer(Configuration["connectionStrings:CoderzoneDbConnectionString"]));

			// Register Users Repository

			// Register Profile Repository

			// Register Country Repository
			services.AddScoped<ICountryRepository, CountryRepository>();
			services.AddScoped<IProgrammerRepository, ProgrammerRepository>();
			services.AddScoped<IProfileRepository, ProfileRepository>();
			services.AddScoped<IProjectRepository, ProjectRepository>();

			//services.AddSingleton<ICountryRepository, CountryRepository>();
			//services.AddSingleton<CoderzoneApiQuery>();

			// Register State Repository

			// Register GraphQL			
			services.AddSingleton<IDocumentExecuter, DocumentExecuter>();
			//services.AddSingleton<IDocumentWriter, DocumentWriter>();
			//Register GraphQL resolver
			services.AddScoped<IDependencyResolver>(
				provider => new FuncDependencyResolver(provider.GetRequiredService)
			);
			//Register GraphQL Schema
			services.AddScoped<CoderzoneApiSchema>();

			// expose developmet exceptions
			services.AddGraphQL(o => { o.ExposeExceptions = _env.IsDevelopment(); })
					.AddGraphTypes(ServiceLifetime.Scoped)
					.AddDataLoader();

		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, CoderzoneApiDbContext context)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			// set up cors
			app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

			// use graphQL passing in the schema
			app.UseGraphiQl();
			app.UseGraphQL<CoderzoneApiSchema>("/graphql");

			// Seeding the DB
			context.SeedDataContext();

			//app.GraphQLMiddleware<CoderzoneApiSchema>();
			//app.UseGraphiQl("/graphiql");
			//app.UseGraphiQl();

			// set up as MVC if necessary
			//app.UseMvc();


		}
	}
}
