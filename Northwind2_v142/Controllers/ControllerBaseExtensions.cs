using Microsoft.AspNetCore.JsonPatch.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Northwind2_v142.Data;
using Northwind2_v142.Services;
using System.ComponentModel;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace Northwind2_v142.Controllers
{
	public static class ControllerBaseExtensions
	{
		// Renvoie une réponse HTTP personnalisée pour les erreurs
		public static ActionResult CustomResponseForError(this ControllerBase controller, Exception e)
		{
			if (e is DbUpdateConcurrencyException)
			{
				return controller.Problem("L'entité ou au moins l'une de ses entités filles n'existe pas en base.",
					null, (int)HttpStatusCode.NotFound, "Aucune modification enregistrée en base.");
			}
			else if (e is DbUpdateException || e is SqlException)
			{
				ProblemDetails pb = e.ConvertToProblemDetails();
				return controller.Problem(pb.Detail, null, pb.Status, pb.Title);
			}
			else if (e is JsonPatchException)
			{
				return controller.Problem("Le patch contient des opérations non prises en charge pour cette ressource.",
					null, (int)HttpStatusCode.BadRequest, "PATCH non appliqué");
			}
			else if (e is ValidationRulesException vre)
			{
				ValidationProblemDetails vpd = new(vre.Errors);
				return controller.ValidationProblem(vpd);
			}
			else throw e;
		}
	}
}
