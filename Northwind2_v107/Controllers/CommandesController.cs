using Microsoft.AspNetCore.Mvc;
using Northwind2_v107.Entities;
using Northwind2_v107.Services;

namespace Northwind2_v107.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CommandesController : ControllerBase
	{
		private readonly IServiceCommandes _serviceCmde;
		private readonly ILogger<EmployesController> _logger;

		public CommandesController(IServiceCommandes service, ILogger<EmployesController> logger)
		{
			_serviceCmde = service;
			_logger = logger;
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
			try
			{
				Commande? commande = await _serviceCmde.AjouterCommande(cmde);

				string uri = Url.Action(nameof(GetCommande), new { id = commande?.Id }) ?? "";
				return Created(uri, commande);
			}
			catch (Exception e)
			{
				return this.CustomResponseForError(e);
			}
		}

		// DELETE: api/Commandes/831
		[HttpDelete("{idCommande}")]
		public async Task<IActionResult> DeleteCommande(int idCommande)
		{
			try
			{
				int nbSuppr = await _serviceCmde.SupprimerCommande(idCommande);
				if (nbSuppr == 0) return NotFound();
				return NoContent();
			}
			catch (Exception e)
			{
				return this.CustomResponseForError(e);
			}
		}

		// PUT: api/Commandes
		[HttpPut]
		public async Task<ActionResult> PutCommande(Commande cmde)
		{
			try
			{
				await _serviceCmde.ModifierCommande(cmde);
				return NoContent();
			}
			catch (Exception e)
			{
				return this.CustomResponseForError(e);
			}
		}

		// POST: api/Commandes/831/Lignes
		[HttpPost("{idCommande}/Lignes")]
		public async Task<ActionResult<LigneCommande>> PostLigneCommande(int idCommande, LigneCommande ligne)
		{
			try
			{
				LigneCommande? res = await _serviceCmde.AjouterLigneCommande(idCommande, ligne);

				string uri = Url.Action(nameof(GetCommande), new { Id = res?.IdCommande }) ?? "";
				return Created(uri, res);
			}
			catch (Exception e)
			{
				return this.CustomResponseForError(e);
			}
		}

		// DELETE: api/Commandes/831/Lignes/77
		[HttpDelete("{idCommande}/Lignes/{idProduit}")]
		public async Task<IActionResult> DeleteLigneCommande(int idCommande, int idProduit)
		{
			try
			{
				await _serviceCmde.SupprimerLigneCommande(idCommande, idProduit);
				return NoContent();
			}
			catch (Exception e)
			{
				return this.CustomResponseForError(e);
			}
		}

		// PUT: api/Commandes/831/Lignes
		[HttpPut("{idCommande}/Lignes")]
		public async Task<IActionResult> PutLigneCommande(int idCommande, LigneCommande ligne)
		{
			try
			{
				//await _serviceCmde.ModifierLigneCommande(idCommande, ligne);
				await _serviceCmde.ModifierLigneCommande2(idCommande, ligne);
				return NoContent();

				//LigneCommande res = await _serviceCmde.ModifierLigneCommande3(idCommande, ligne.IdProduit, ligne.Quantite);
				//return Ok(res);

			}
			catch (Exception e)
			{
				return this.CustomResponseForError(e);
			}
		}
	}
}
