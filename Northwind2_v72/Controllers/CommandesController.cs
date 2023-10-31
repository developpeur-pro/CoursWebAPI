using Microsoft.AspNetCore.Mvc;
using Northwind2_v72.Entities;
using Northwind2_v72.Services;

namespace Northwind2_v72.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CommandesController : ControllerBase
	{
		private readonly IServiceCommandes _serviceCmde;

		public CommandesController(IServiceCommandes service)
		{
			_serviceCmde = service;
		}

		// GET: api/Commandes
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Commande>>> GetCommandes([FromQuery] int? idEmploye, [FromQuery] string? idClient = null)
		{
			List<Commande> commandes = await _serviceCmde.ObtenirCommandes(idEmploye, idClient);

			return Ok(commandes);
		}

		// GET: api/Commandes/5
		[HttpGet("{id}")]
		public async Task<ActionResult<Commande>> GetCommande(int id)
		{
			Commande? commande = await _serviceCmde.ObtenirCommande(id);

			if (commande == null) return NotFound();

			return Ok(commande);
		}

		// POST: api/Commandes
		[HttpPost]
		public async Task<ActionResult<Commande>> PostCommande(Commande cmde)
		{
			Commande? commande = await _serviceCmde.AjouterCommande(cmde);

			string uri = Url.Action(nameof(GetCommande), new { id = commande?.Id }) ?? "";
			return Created(uri, commande);
		}

		// POST: api/Commandes/830/Lignes
		[HttpPost("{idCommande}/Lignes")]
		public async Task<ActionResult<Commande>> PostLigneCommande(int idCommande, LigneCommande ligne)
		{
			LigneCommande? res = await _serviceCmde.AjouterLigneCommande(idCommande, ligne);

			string uri = Url.Action(nameof(GetCommande), new { Id = res?.IdCommande }) ?? "";
			return Created(uri, res);
		}

		/*
		// POST: api/Clients
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPost]
		public async Task<ActionResult<Client>> PostClient(Client client)
		{

			if (_context.Clients == null)
			{
				return Problem("Entity set 'ContexteNorthwind.Clients'  is null.");
			}
			_context.Clients.Add(client);
			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateException)
			{
				if (ClientExists(client.Id))
				{
					return Conflict();
				}
				else
				{
					throw;
				}
			}

			return CreatedAtAction("GetClient", new { id = client.Id }, client);
		}


		  // PUT: api/Clients/5
		  // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		  [HttpPut("{id}")]
		  public async Task<IActionResult> PutClient(string id, Client client)
		  {
				if (id != client.Id)
				{
					 return BadRequest();
				}

				_context.Entry(client).State = EntityState.Modified;

				try
				{
					 await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					 if (!ClientExists(id))
					 {
						  return NotFound();
					 }
					 else
					 {
						  throw;
					 }
				}

				return NoContent();
		  }

		  // DELETE: api/Clients/5
		  [HttpDelete("{id}")]
		  public async Task<IActionResult> DeleteClient(string id)
		  {
				if (_context.Clients == null)
				{
					 return NotFound();
				}
				var client = await _context.Clients.FindAsync(id);
				if (client == null)
				{
					 return NotFound();
				}

				_context.Clients.Remove(client);
				await _context.SaveChangesAsync();

				return NoContent();
		  }

		  private bool ClientExists(string id)
		  {
				return (_context.Clients?.Any(e => e.Id == id)).GetValueOrDefault();
		  }

		*/
	}
}
