using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Northwind2_v155.Entities;
using Northwind2_v155.Services;
using Northwind2_v155.Tools;

namespace Northwind2_v155.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ClientsController : ControllerBase
	{
		private readonly IServiceClients _serviceClients;

		public ClientsController(IServiceClients service)
		{
			_serviceClients = service;
		}

		// GET: api/Clients
		[HttpGet]
		public async Task<ActionResult<List<Client>>> GetClients()
		{
			var res = await _serviceClients.ObtenirClients();
			return res.ConvertToObjectResult();
		}

		// GET: api/Clients/DUMON
		/// <summary>
		/// Renvoie le client d'id spécifié
		/// </summary>
		/// <remarks></remarks>
		/// <param name="id">identifiant du client</param>
		/// <response code="200">renvoie le client d'id donné</response>
		/// <response code="404">client non trouvé</response>
		[HttpGet("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<Client?>> GetClient(string id)
		{
			var res = await _serviceClients.ObtenirClient(id);
			return res.ConvertToObjectResult();
		}

		// PATCH: api/Clients/DUMON
		[HttpPatch("{id}")]
		public async Task<ActionResult<int>> PatchClient(string id, JsonPatchDocument<Client> patch)
		{
			var res = await _serviceClients.ModifierClient(id, patch);
			return res.ConvertToObjectResult();
		}
	}
}
