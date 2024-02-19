using Asp.Versioning;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Northwind2_v142.Entities;
using Northwind2_v142.Services;

namespace Northwind2_v142.Controllers
{
	[Route("api/[controller]")]
	[ApiVersion(1.0)]
	[ApiVersion(2.0)]
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
		[MapToApiVersion(1.0)]
		public async Task<ActionResult<IEnumerable<Client>>> GetClients()
		{
			List<Client> Clients = await _serviceClients.ObtenirClients();
			return Ok(Clients);
		}

		[HttpGet]
		[MapToApiVersion(2.0)]
		public async Task<ActionResult<IEnumerable<Client>>> GetClients2()
		{
			List<Client> Clients = await _serviceClients.ObtenirClients();
			return Ok(Clients);
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
		public async Task<ActionResult<Client>> GetClient(string id)
		{
			Client? Client = await _serviceClients.ObtenirClient(id);

			if (Client == null) return NotFound();
			return Ok(Client);
		}

		// PATCH: api/Clients/DUMON
		[HttpPatch("{id}")]
		public async Task<IActionResult> PatchClient(string id, JsonPatchDocument<Client> patch)
		{
			try
			{
				int nbMaj = await _serviceClients.ModifierClient(id, patch);
				if (nbMaj == 0) return BadRequest($"Aucune mise à jour réalisée");

				return Ok(nbMaj);
			}
			catch (Exception e)
			{
				return this.CustomResponseForError(e);
			}
		}
	}
}
