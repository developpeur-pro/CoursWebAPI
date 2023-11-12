using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Northwind2_v72.Data;
using Microsoft.AspNetCore.Diagnostics;

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
				// On gère les exceptions émises par la base en renvoyant une réponse adaptée

				ProblemDetails pb = ex.ConvertToProblemDetails();
				context.Response.ContentType = "application/json";
				context.Response.StatusCode = pb.Status??500;
				// Ecrit le détail du problème dans le corps de la réponse
				await context.Response.WriteAsJsonAsync(pb);
			}
		}
	}
}
