using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Northwind2_v121.Data;
using System.Net;

namespace Northwind2_v121.Middlewares
{
	public class CustomErrorResponseMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger _logger;

		public CustomErrorResponseMiddleware(RequestDelegate next, ILogger logger)
		{
			_next = next;
			_logger = logger;
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
				context.Response.StatusCode = pb.Status ?? 500;
				// Ecrit le détail du problème dans le corps de la réponse
				await context.Response.WriteAsJsonAsync(pb);
			}
			catch (Exception e)
			{
				// Journalise les autres exceptions avant de les réémettre
				_logger.LogError(e, "Erreur serveur non gérée.");
				throw;
			}
		}
	}
}
