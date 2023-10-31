using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Northwind2_v72.Data;

namespace Northwind2_v72.Controllers
{
	public static class ControllerBaseExtensions
	{
		// Renvoie une réponse HTTP d'erreur personnalisée à partir d'une erreur de base de données
		public static ActionResult CustomResponseForDbError(this ControllerBase controller, DbUpdateException e)
		{
			(int code, string message) = e.TranslateToHttpResponse();
			if (code == 409) return controller.Conflict(message);
			else if (code == 400) return controller.BadRequest(message);
			else throw e;
		}
	}
}
