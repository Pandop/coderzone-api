using System;
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
using Microsoft.AspNetCore.Mvc;
using CoderzoneGrapQLAPI.helpers.utils;
using AspNet.Security.OpenIdConnect.Primitives;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.IdentityModel.Tokens.Jwt;
using CoderzoneGrapQLAPI.helpers;
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography.X509Certificates;

using Autofac;
using Autofac.Extensions.DependencyInjection;

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
			services.AddMvc(option => 
				{
					option.Filters.Add(new XsrfActionFilterAttribute());
					option.Filters.Add(new AuthenticationFilterAttribute());
				})
				.AddControllersAsServices()
				.SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
				.AddJsonOptions(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

			services.AddAntiforgery(options => options.HeaderName = "X-XSRF-TOKEN");

			// Setup DB Connections
			services.AddDbContext<CoderzoneApiDbContext>(options => options.UseSqlServer(Configuration["connectionStrings:CoderzoneDbConnectionString"]));

			// Register Users Repository
			// Register Identity Services
			services.AddIdentity<Coder, Group>(options => { options.User.AllowedUserNameCharacters += @"\*"; })
				.AddEntityFrameworkStores<CoderzoneApiDbContext>()
				.AddDefaultTokenProviders();

			services.Configure<IdentityOptions>(options =>
			{
				options.ClaimsIdentity.UserNameClaimType = OpenIdConnectConstants.Claims.Name;
				options.ClaimsIdentity.UserIdClaimType = OpenIdConnectConstants.Claims.Subject;
				options.ClaimsIdentity.RoleClaimType = OpenIdConnectConstants.Claims.Role;

				options.Password.RequireDigit = true;
			});

			var certSetting = Configuration.GetSection("CertificateSetting").Get<CertificateSetting>();

			services.AddOpenIddict()
				.AddCore(options =>
				{
					options.UseEntityFrameworkCore()
						.UseDbContext<CoderzoneApiDbContext>()
						.ReplaceDefaultEntities<Guid>();
				})
				.AddServer(options =>
				{
					options.UseMvc();
					options.EnableTokenEndpoint("/api/authorization/connect/token");

					X509Certificate2 cert = null;
					if (_env.IsDevelopment())
					{
						cert = new InRootFolderCertificateProvider(certSetting).ReadX509SigningCert();
					}
					else
					{
						// not for production, currently using the same as development testing.
						// todo: Create another Certificate Provider Inheriting BaseCertificateProvider, and override ReadX509SigningCert
						// to read cerficicate from another more secure place, e.g cerficate store, aws server...
						cert = new InRootFolderCertificateProvider(certSetting).ReadX509SigningCert();
					}

					if (cert == null)
					{
						// not for production, use x509 certificate and .AddSigningCertificate()
						options.AddEphemeralSigningKey();
					}
					else
					{
						options.AddSigningCertificate(cert);
					}

					// use jwt
					options.UseJsonWebTokens();
					options.AllowPasswordFlow();
					options.AllowRefreshTokenFlow();
					options.AcceptAnonymousClients();
					options.DisableHttpsRequirement();
				});

			JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
			JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear();

			services.AddAuthentication(options =>
			{
				options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
			})
				.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
				{
					options.LoginPath = "/api/authorization/login";
					options.LogoutPath = "/api/authorization/logout";
					options.SlidingExpiration = true;
					options.ExpireTimeSpan = TimeSpan.FromDays(7);
					options.Events.OnRedirectToLogin = redirectOptions =>
					{
						redirectOptions.Response.StatusCode = StatusCodes.Status401Unauthorized;
						return Task.CompletedTask;
					};
				})
				.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options => {
					options.Authority = certSetting.JwtBearerAuthority;
					options.Audience = certSetting.JwtBearerAudience;
					options.RequireHttpsMetadata = false;
					options.IncludeErrorDetails = true;
					options.TokenValidationParameters = new TokenValidationParameters()
					{
						NameClaimType = OpenIdConnectConstants.Claims.Subject,
						RoleClaimType = OpenIdConnectConstants.Claims.Role
					};
				});

			// Register Profile Repository

			// Register Country Repository
			services.AddScoped<ICountryRepository, CountryRepository>();
			services.AddScoped<ICountryRepository, CountryRepository>();
			services.AddScoped<IProgrammerRepository, ProgrammerRepository>();
			services.AddScoped<IProfileRepository, ProfileRepository>();
			services.AddScoped<IProjectRepository, ProjectRepository>();
			services.AddScoped<IStateRepository, StateRepository>();

			//services.AddSingleton<ICountryRepository, CountryRepository>();


			// Register State Repository

			// Register GraphQL			
			services.AddSingleton<IDocumentExecuter, DocumentExecuter>();
			services.AddSingleton<CoderzoneApiQuery>();
			services.AddSingleton<CoderzoneApiMutation>();
			services.AddSingleton<CountryType>();
			services.AddSingleton<CountryInputType>();
			services.AddSingleton<StateType>();
			services.AddSingleton<StateInputType>();
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
