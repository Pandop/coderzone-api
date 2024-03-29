﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoderzoneGrapQLAPI.Api.DbSeeds;
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
using CoderzoneGrapQLAPI.GraphQL.Types;
using GraphQL.DataLoader;
using GraphQL.Execution;
using CoderzoneGrapQLAPI.GraphQL.Mutations;

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
			services.AddScoped<ICountryRepository, CountryRepository>();
			services.AddScoped<IProgrammerRepository, ProgrammerRepository>();
			services.AddScoped<IProfileRepository, ProfileRepository>();
			services.AddScoped<IProjectRepository, ProjectRepository>();
			//services.AddScoped<IStateRepository, StateRepository>();

			//services.AddSingleton<ICountryRepository, CountryRepository>();


			// Register State Repository

			// Register GraphQL			
			services.AddSingleton<IDocumentExecuter, DocumentExecuter>();
			services.AddSingleton<CoderzoneApiQuery>();
			services.AddSingleton<CoderzoneApiMutation>();
			services.AddSingleton<CountryType>();
			services.AddSingleton<CountryInputType>();
			services.AddSingleton<StateType>();
			services.AddSingleton<ProgrammerType>();
			services.AddSingleton<ProfileType>();
			services.AddSingleton<SkillType>();
			services.AddSingleton<WorkExperienceType>();
			services.AddSingleton<QualificationType>();
			services.AddSingleton<ProjectType>();

			// Add GraphQLexpose developmet exceptions
			//services.AddSingleton<IDocumentWriter, DocumentWriter>();
			//services.AddSingleton<IDataLoaderContextAccessor, DataLoaderContextAccessor>();
			services.AddSingleton<IDataLoaderContextAccessor>(new DataLoaderContextAccessor());
			//services.AddSingleton<IDocumentExecutionListener, DataLoaderDocumentListener>();
			services.AddSingleton<DataLoaderDocumentListener>();

			services.AddGraphQL(o => { o.EnableMetrics = true; o.ExposeExceptions = _env.IsDevelopment(); })
					.AddGraphTypes(ServiceLifetime.Singleton)
					.AddDataLoader();

			//Register GraphQL resolver
			//services.AddScoped<IDependencyResolver>(
			//	provider => new FuncDependencyResolver(provider.GetRequiredService)
			//);

			var sp = services.BuildServiceProvider();
			services.AddSingleton<ISchema>(new CoderzoneApiSchema(new FuncDependencyResolver(type => sp.GetService(type))));
			//
			//services.AddSingleton<ISchema, CoderzoneApiSchema>();
			//Register GraphQL Schema
			//services.AddScoped<CoderzoneApiSchema>();
			//services.AddScoped<CoderzoneApiSchema>();

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

			//app.UseGraphQL<CoderzoneApiSchema>();
			//app.GraphQLMiddleware<CoderzoneApiSchema>();

			// set up as MVC if necessary
			app.UseMvc();

			// Seed the database
			context.SeedDataContext();


		}
	}
}
