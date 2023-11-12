using Microsoft.AspNetCore.Mvc;
using Northwind2_v64.Entities;
using Northwind2_v64.Services;

namespace Northwind2_v64.Controllers
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

		// POST: api/Commandes/831/Lignes
		[HttpPost("{idCommande}/Lignes")]
		public async Task<ActionResult<Commande>> PostLigneCommande(int idCommande, LigneCommande ligne)
		{
			LigneCommande? res = await _serviceCmde.AjouterLigneCommande(idCommande, ligne);

			string uri = Url.Action(nameof(GetCommande), new { Id = res?.IdCommande }) ?? "";
			return Created(uri, res);
		}

	}
}
