using CoderzoneGrapQLAPI.Models;
using CoderzoneGrapQLAPI.Services;
using GraphQL;
using GraphQL.EntityFramework;
using GraphQL.Types;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CoderzoneGrapQLAPI.controllers
{
	public class CodersGraphQLContext
	{
		public CoderzoneApiDbContext DbContext { get; set; }
	}

	[Route("[controller]")]
	public class GraphQLController : Controller
	{
		private readonly ISchema _schema;
		private readonly CoderzoneApiDbContext dbContext;
		private readonly IDocumentExecuter _documentExecuter;

		public GraphQLController(IDocumentExecuter documentExecuter, ISchema schema, CoderzoneApiDbContext dbContext)
		{
			_schema = schema;
			this.dbContext = dbContext;
			_documentExecuter = documentExecuter;
		}
		
		[HttpPost]
		public async Task<ExecutionResult> Post([FromBody] GraphQLQueryType query)
		{			
			if (query == null) throw new ArgumentNullException(nameof(query));
	
			// construct query
			var executionOptions = new ExecutionOptions
			{
				Schema = _schema,
				Query = query.Query,
				OperationName = query.OperationName,
				Inputs = query.Variables?.ToInputs(),
				UserContext = new CodersGraphQLContext
				{
					DbContext = dbContext
				}
			};

			// prepare the query for sending back to client
			var result = await _documentExecuter.ExecuteAsync(executionOptions);

			// check for errors
			if (result.Errors?.Count() > 0)
				Response.StatusCode = (int)HttpStatusCode.BadRequest;

			// send result back to client
			return result;
		}

	}
}




//[HttpPost]
//public async Task<IActionResult> Post([FromBody] GraphQLQueryType query)
//{
//	if (query == null)
//		throw new ArgumentNullException(nameof(query));
//	//var inputs = query.Variables?.ToInputs();

//	var executionOptions = new ExecutionOptions
//	{
//		Schema = _schema,
//		Query = query.Query,
//		//OperationName = query.OperationName,
//		Inputs = query.Variables?.ToInputs()
//	};
//	var result = await _documentExecuter.ExecuteAsync(executionOptions);
//	if (result.Errors?.Count > 0)
//		return BadRequest(result);

//	return Ok(result);
//}
