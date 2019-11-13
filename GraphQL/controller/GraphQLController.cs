using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using CoderzoneGrapQLAPI.helpers;
using CsharpReference.Services;
using GraphQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json.Linq;

namespace CoderzoneGrapQLAPI.GraphQL.controller
{
	[Route("/api/graphql")]
	[Authorize]
	[ApiController]
	public class GraphQLController : Controller
	{
		private readonly GraphQlService _graphQlService;
		private readonly UserService _userService;

		public GraphQLController(GraphQlService graphQlService, UserService userService)
		{
			_graphQlService = graphQlService;
			_userService = userService;
		}

		/// <summary>
		/// Executor for GraphQL queries
		/// </summary>
		/// <param name="body"></param>
		/// <param name="cancellation"></param>
		/// <returns>The results for the GraphQL query</returns>
		[HttpPost]
		[Authorize]
		public async Task<ExecutionResult> Post([BindRequired, FromBody] PostBody body,	CancellationToken cancellation)
		{
			var user = await _userService.GetUserFromClaim(User);
			ExecutionResult result = await _graphQlService.Execute(body.Query, body.OperationName, body.Variables, user, cancellation);
			if (result.Errors?.Count > 0)
			{
				Response.StatusCode = (int)HttpStatusCode.BadRequest;
			}

			return result;
		}

		public class PostBody
		{
			public string OperationName;
			public string Query;
			public JObject Variables;
		}

		[HttpGet]
		[Authorize]
		public async Task<ExecutionResult> Get([FromQuery] string query, [FromQuery] string variables, [FromQuery] string operationName, CancellationToken cancellation)
		{
			var jObject = ParseVariables(variables);
			var user = await _userService.GetUserFromClaim(User);
			ExecutionResult result = await _graphQlService.Execute(query, operationName, jObject, user, cancellation);
			if (result.Errors?.Count > 0)
			{
				Response.StatusCode = (int)HttpStatusCode.BadRequest;
			}
			return result;
		}

		static JObject ParseVariables(string variables)
		{
			if (variables == null)
			{
				return null;
			}

			try
			{
				return JObject.Parse(variables);
			}
			catch (Exception exception)
			{
				throw new Exception("Could not parse variables.", exception);
			}
		}
	}
}
