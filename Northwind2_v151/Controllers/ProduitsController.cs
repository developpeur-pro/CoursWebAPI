using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Northwind2_v151.Entities;
using Northwind2_v151.Services;

namespace Northwind2_v151.Controllers
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
		public async Task<ActionResult<IEnumerable<Produit>>> GetProduits([FromQuery] Guid? idCategorie, [FromQuery] int? idFournisseur = null)
		{
			List<Produit> Produits = await _serviceProduits.ObtenirProduits(idCategorie, idFournisseur);

			return Ok(Produits);
		}

		// GET: api/Produits/5
		[HttpGet("{id}")]
		public async Task<ActionResult<Produit>> GetProduit(int id)
		{
			Produit? Produit = await _serviceProduits.ObtenirProduit(id);

			if (Produit == null) return NotFound();

			return Ok(Produit);
		}

		// POST: api/Produits
		[HttpPost]
		public async Task<ActionResult<Produit>> PostProduit(Produit produit)
		{
			try
			{
				Produit? Produit = await _serviceProduits.AjouterProduit(produit);

				string uri = Url.Action(nameof(GetProduit), new { id = Produit?.Id }) ?? "";
				return Created(uri, Produit);
			}
			catch (Exception e)
			{
				return this.CustomResponseForError(e);
			}
		}

		// PUT: api/Produits/77
		[HttpPut]
		public async Task<IActionResult> PutProduit(Produit produit)
		{
			try
			{
				Produit p = await _serviceProduits.ModifierProduit(produit);
				return Ok(p);
				//return NoContent();
			}
			catch (Exception e)
			{
				// Gestion spécifique des modifications et suppressions concurrentes
				if (e is DbUpdateConcurrencyException)
				{
					Produit? rep = await _serviceProduits.ObtenirProduit(produit.Id);
					if (rep == null)
						return NotFound($"Le produit a été supprimé par un autre utilisateur.");
				}

				return this.CustomResponseForError(e);
			}
		}

		// PATCH: api/Produits/4
		[HttpPatch("{idProduit}")]
		public async Task<IActionResult> PatchProduit(int idProduit, JsonPatchDocument<Produit> patch)
		{
			try
			{
				int nbMaj = await _serviceProduits.ModifierProduit(idProduit, patch);
				if (nbMaj == 0) return BadRequest($"Aucune mise à jour réalisée");

				return NoContent();
			}
			catch (Exception e)
			{
				return this.CustomResponseForError(e);
			}
		}
	}
}
