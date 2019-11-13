using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CoderzoneGrapQLAPI.Services;
using CsharpReference.Services;
using GraphQL;
using GraphQL.Types;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json.Linq;

namespace CoderzoneGrapQLAPI.helpers
{
	public class GraphQLCsharpReferenceContext
	{
		public CoderzoneApiDbContext DbContext { get; set; }
		public Coder Coder { get; set; }
		public IList<string> UserGroups { get; set; }
		public SecurityService SecurityService { get; set; }
		public UserManager<Coder> UserManager { get; set; }
		public UserService UserService { get; set; }
		public CrudService CrudService { get; set; }
		public IdentityService IdentityService { get; set; }
		//public AuditService AuditService { get; set; }
	}

	public class GraphQlService
	{
		private readonly IDocumentExecuter _executer;
		private readonly ISchema _schema;
		private readonly CoderzoneApiDbContext _dataContext;
		private readonly SecurityService _securityService;
		private readonly UserManager<Coder> _userManager;
		private readonly UserService _userService;
		private readonly CrudService _crudService;
		private readonly IdentityService _identityService;
		//private readonly AuditService _auditService;

		public GraphQlService(
			ISchema schema,
			IDocumentExecuter executer,
			CoderzoneApiDbContext dataContext,
			SecurityService securityService,
			UserManager<Coder> userManager,
			UserService userService,
			CrudService crudService,
			IdentityService identityService
			//AuditService auditService
			)
		{
			_schema = schema;
			_executer = executer;
			_dataContext = dataContext;
			_securityService = securityService;
			_userManager = userManager;
			_userService = userService;
			_crudService = crudService;
			_identityService = identityService;
			//_auditService = auditService;
		}

		public async Task<ExecutionResult> Execute(
			string query,
			string operationName,
			JObject variables,
			Coder user,
			CancellationToken cancellation)
		{
			var executionOptions = new ExecutionOptions
			{
				Schema = _schema,
				Query = query,
				OperationName = operationName,
				Inputs = variables?.ToInputs(),
				UserContext = new GraphQLCsharpReferenceContext
				{
					DbContext = _dataContext,
					Coder = user,
					UserGroups = await _userManager.GetRolesAsync(user),
					SecurityService = _securityService,
					CrudService = _crudService,
					IdentityService = _identityService,
					UserManager = _userManager,
					UserService = _userService,
					//AuditService = _auditService,
				},
				CancellationToken = cancellation,
#if (DEBUG)
				ExposeExceptions = true,
				EnableMetrics = true,
#endif
			};

			var result = await _executer.ExecuteAsync(executionOptions)
				.ConfigureAwait(false);

			return result;
		}
	}
}
