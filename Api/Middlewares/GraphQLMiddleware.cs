using GraphQL;
using GraphQL.Http;
using GraphQL.Types;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CoderzoneGrapQLAPI.Api.Middlewares
{
	public class GraphQLRequest
	{
		private JObject _variables;
		public string OperationName { get; set; }
		public string Query { get; set; }
		public JObject Variables
		{
			get => _variables ?? new JObject(new { });
			set => _variables = value;
		}

	}
	public static class GraphQLMiddlewareExtension
	{
		public static void GraphQLMiddleware<TSchema>(this IApplicationBuilder appBuilder) where TSchema: ISchema
		{
			appBuilder.UseMiddleware<GraphQLMiddlware<TSchema>>();
		}
	}

	public class GraphQLMiddlware<TSchema> where TSchema : ISchema
	{
		private readonly RequestDelegate _next;
		private readonly IDocumentExecuter _executor;
		private readonly IDocumentWriter _writer;
		private TSchema Schema { get; }

		public GraphQLMiddlware(RequestDelegate next, TSchema schema,IDocumentExecuter executer,IDocumentWriter writer)
		{
			_next = next;
			Schema = schema;
			_executor = executer;
			_writer = writer;
		}
		public async Task InvokeAsync(HttpContext context)
		{
			// Method is not "POST" & path is not "/graphql"
			if(!context.Request.Method.Equals("post", StringComparison.OrdinalIgnoreCase) && !context.Request.Path.Equals("/graphql"))
			{
				await _next(context);
				return;
			}
			var request = Deserialize<GraphQLRequest>(context.Request.Body);
			var result = await _executor.ExecuteAsync(new ExecutionOptions
			{
				Schema = Schema,
				Query = request.Query,
				OperationName = request.OperationName,
				Inputs=request.Variables.ToInputs()
			});
			context.Response.ContentType = "application/json";
			context.Response.StatusCode = (int)(result.Errors?.Any() == true ? HttpStatusCode.BadRequest : HttpStatusCode.OK);
			//await context.Response.WriteAsync(_writer.Write(result));
			//await context.Response.WriteAsync(_writer.Write(result));
		}

		private static T Deserialize<T>(Stream stream)
		{
			using(var reader = new StreamReader(stream))
			{
				using (var JsonReader = new JsonTextReader(reader))
				{
					return new JsonSerializer().Deserialize<T>(JsonReader);
				}
			}
		}
	}
}
