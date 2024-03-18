using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Northwind2_v155.Entities;
using Northwind2_v155.Services;
using Northwind2_v155.Tools;

namespace Northwind2_v155.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProduitsController : ControllerBase
	{
		private readonly IServiceProduits _serviceProduits;

		public ProduitsController(IServiceProduits service)
		{
			_serviceProduits = service;
		}

		// GET: api/Produits?idCategorie=B69E53B1-56E3-49E8-B53C-439975278ACD&idFournisseur=2
		[HttpGet]
		public async Task<ActionResult<List<Produit>>> GetProduits([FromQuery] Guid? idCategorie, [FromQuery] int? idFournisseur = null)
		{
			var res = await _serviceProduits.ObtenirProduits(idCategorie, idFournisseur);
			return res.ConvertToObjectResult();
		}

		// GET: api/Produits/5
		[HttpGet("{id}")]
		public async Task<ActionResult<Produit?>> GetProduit(int id)
		{
			var res = await _serviceProduits.ObtenirProduit(id);
			return res.ConvertToObjectResult();
		}

		// POST: api/Produits
		[HttpPost]
		public async Task<ActionResult<Produit?>> PostProduit(Produit produit)
		{
			var res = await _serviceProduits.AjouterProduit(produit);

			string uri = Url.Action(nameof(GetProduit), new { id = res.Data?.Id }) ?? "";
			return res.ConvertToObjectResult();
		}

		// PUT: api/Produits/77
		[HttpPut]
		public async Task<ActionResult<Produit?>> PutProduit(Produit produit)
		{
			var res = await _serviceProduits.ModifierProduit(produit);

			// Gestion spécifique des modifications et suppressions concurrentes
			if (res.ResultKind == ResultKinds.Concurrency)
			{
				var res2 = await _serviceProduits.ObtenirProduit(produit.Id);
				if (res2.ResultKind == ResultKinds.NotFound)
					return res2.ConvertToObjectResult();
			}

			return res.ConvertToObjectResult();
		}

		// PATCH: api/Produits/4
		[HttpPatch("{idProduit}")]
		public async Task<ActionResult<int>> PatchProduit(int idProduit, JsonPatchDocument<Produit> patch)
		{
			var res = await _serviceProduits.ModifierProduit(idProduit, patch);
			return res.ConvertToObjectResult();
		}
	}
}
