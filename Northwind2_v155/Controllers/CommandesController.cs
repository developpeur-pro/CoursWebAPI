using Microsoft.AspNetCore.Mvc;
using Northwind2_v155.Entities;
using Northwind2_v155.Services;
using Northwind2_v155.Tools;
using System.Security.Claims;

namespace Northwind2_v155.Controllers
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
		public async Task<ActionResult<List<Commande>>> GetCommandes([FromQuery] int? idEmploye, [FromQuery] string? idClient = null)
		{
			var res = await _serviceCmde.ObtenirCommandes(idEmploye, idClient);
			return res.ConvertToObjectResult();
		}

		// GET: api/Commandes/5
		[HttpGet("{id}")]
		public async Task<ActionResult<Commande?>> GetCommande(int id)
		{
			var res = await _serviceCmde.ObtenirCommande(id);
			return res.ConvertToObjectResult();
		}

		// POST: api/Commandes
		[HttpPost]
		public async Task<ActionResult<Commande?>> PostCommande(Commande cmde)
		{
			// Récupère l'id de l'utilisateur
			string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (int.TryParse(userId, out int id))
				cmde.IdEmploye = id;

			var res = await _serviceCmde.AjouterCommande(cmde);

			string uri = Url.Action(nameof(GetCommande), new { id = res.Data?.Id }) ?? "";
			return res.ConvertToObjectResult(uri);
		}

		// DELETE: api/Commandes/831
		[HttpDelete("{idCommande}")]
		public async Task<ActionResult<int>> DeleteCommande(int idCommande)
		{
			var res = await _serviceCmde.SupprimerCommande(idCommande);
			return res.ConvertToObjectResult();
		}
		// PUT: api/Commandes
		[HttpPut]
		public async Task<ActionResult<int>> PutCommande(Commande cmde)
		{
			var res = await _serviceCmde.ModifierCommande(cmde);
			return res.ConvertToObjectResult();
		}

		// POST: api/Commandes/831/Lignes
		[HttpPost("{idCommande}/Lignes")]
		public async Task<ActionResult<LigneCommande?>> PostLigneCommande(int idCommande, LigneCommande ligne)
		{
				var res = await _serviceCmde.AjouterLigneCommande(idCommande, ligne);
				string uri = Url.Action(nameof(GetCommande), new { Id = res.Data?.IdCommande }) ?? "";
				return res.ConvertToObjectResult(uri);
		}

		// DELETE: api/Commandes/831/Lignes/77
		[HttpDelete("{idCommande}/Lignes/{idProduit}")]
		public async Task<ActionResult<int>> DeleteLigneCommande(int idCommande, int idProduit)
		{
			var res = await _serviceCmde.SupprimerLigneCommande2(idCommande, idProduit);
			return res.ConvertToObjectResult();
		}

		// PUT: api/Commandes/831/Lignes
		[HttpPut("{idCommande}/Lignes")]
		public async Task<ActionResult<int>> PutLigneCommande(int idCommande, LigneCommande ligne)
		{
			var res = await _serviceCmde.ModifierLigneCommande(idCommande, ligne);
			//var res = await _serviceCmde.ModifierLigneCommande2(idCommande, ligne);
			//var res = await _serviceCmde.ModifierLigneCommande3(idCommande, ligne.IdProduit, ligne.Quantite);
			return res.ConvertToObjectResult();
		}
	}
}
