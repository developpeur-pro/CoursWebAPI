using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Northwind2_v107.Entities;
using Northwind2_v107.Services;

namespace Northwind2_v107.Controllers
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
		public async Task<ActionResult<IEnumerable<Client>>> GetClients()
		{
			List<Client> Clients = await _serviceClients.ObtenirClients();
			return Ok(Clients);
		}

		// GET: api/Clients/DUMON
		[HttpGet("{id}")]
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
