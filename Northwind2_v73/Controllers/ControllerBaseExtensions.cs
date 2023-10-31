using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Northwind2_v73.Data;
using System.ComponentModel;

namespace Northwind2_v73.Controllers
{
	public static class ControllerBaseExtensions
	{
		// Renvoie une réponse HTTP d'erreur personnalisée à partir
		// d'une erreur de base de données ou de validation
		public static ActionResult CustomResponseForError(this ControllerBase controller, Exception e)
		{
			if (e is DbUpdateException dbe)
			{
				(int code, string message) = dbe.TranslateToHttpResponse();
				if (code == 409) return controller.Conflict(message);
				else if (code == 400) return controller.BadRequest(message);
				else throw e;
			}
			else if (e is ArgumentException)
			{
				return controller.BadRequest(e.Message);
			}
			else throw e;
		}
	}
}
