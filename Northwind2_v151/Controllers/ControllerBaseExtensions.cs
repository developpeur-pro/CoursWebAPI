using Microsoft.AspNetCore.Mvc;
using Northwind2_v151.Data;
using Northwind2_v151.Services;

namespace Northwind2_v151.Controllers
{
	public static class ControllerBaseExtensions
	{
		// Renvoie une réponse HTTP personnalisée pour les erreurs
		public static ActionResult CustomResponseForError(this ControllerBase controller, Exception e)
		{
			if (e is ValidationRulesException vre)
			{
				ValidationProblemDetails vpd = new(vre.Errors);
				return controller.ValidationProblem(vpd);
			}
			else
			{
				ProblemDetails pb = e.ConvertToProblemDetails();
				return controller.Problem(pb.Detail, null, pb.Status, pb.Title);
			}
		}
	}
}
