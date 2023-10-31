using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Northwind2_v72.Data;

namespace Northwind2_v72.Middlewares
{
	public class CustomErrorResponseMiddleware
	{
		private readonly RequestDelegate _next;

		public CustomErrorResponseMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task Invoke(HttpContext context)
		{
			try
			{
				await _next(context);
			}
			catch (DbUpdateException ex)
			{
				// Gère les exceptions émises par la base en renvoyant une réponse 4XX adaptée
				(int code, string message) = ex.TranslateToHttpResponse();
				context.Response.StatusCode = code;
				context.Response.ContentType = "text/plain; charset=utf-8";
				await context.Response.WriteAsync(message);
			}
		}
	}
}
