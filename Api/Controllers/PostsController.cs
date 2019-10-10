using CoderzoneGrapQLAPI.Models;
using GraphQL;
using GraphQL.Types;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoderzoneGrapQLAPI.controllers
{
	[Route("[controller]")]
	public class PostsController:Controller
	{
		private readonly ISchema _schema;
		private readonly IDocumentExecuter _documentExecuter;
		public PostsController(IDocumentExecuter documentExecuter, ISchema schema)
		{
			_schema = schema;
			_documentExecuter = documentExecuter;
		}
		[HttpPost]
		public async Task<IActionResult> Post([FromBody] QueryType query)
		{
			if (query == null)
				throw new ArgumentNullException(nameof(query));
			var inputs = query.Variables?.ToInputs();
			var executionOptions = new ExecutionOptions
			{
				Schema = _schema,
				Query = query.Query,
				OperationName=query.OperationName,
				Inputs = inputs
			};
			var result = await _documentExecuter.ExecuteAsync(executionOptions);
			if (result.Errors.Count() > 0)
				return BadRequest(result);

			return Ok(result);
		}
	}
}
