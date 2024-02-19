using Northwind2_v142.Entities;
using Northwind2_v142.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.AspNetCore.JsonPatch.Adapters;
using Microsoft.AspNetCore.JsonPatch.Operations;

namespace Northwind2_v142.Services
{
	public interface IServiceProduits
	{
		Task<List<Produit>> ObtenirProduits(Guid? idCatégorie, int? idFournisseur);
		Task<Produit?> ObtenirProduit(int id);
		Task<Produit?> AjouterProduit(Produit produit);
		Task<Produit> ModifierProduit(Produit produit);
		Task<int> ModifierProduit(int idProduit, JsonPatchDocument<Produit> patch);
		Task<int> ArrêterProduitsFournisseur(int idFournisseur);
	}

	public class ServiceProduits : IServiceProduits
	{
		private readonly ContexteNorthwind _contexte;
		private readonly ILogger<ServiceProduits> _logger;

		public ServiceProduits(ContexteNorthwind context, ILogger<ServiceProduits> logger)
		{
			_contexte = context;
			_logger = logger;
		}

		// Renvoie les produits de la catégorie et du fournisseur spécifiés
		// ou bien tous les produits
		public async Task<List<Produit>> ObtenirProduits(Guid? idCatégorie, int? idFournisseur)
		{
			var req = from p in _contexte.Produits
						 where (idCatégorie == null || p.IdCategorie == idCatégorie) &&
								 (idFournisseur == null || p.IdFournisseur == idFournisseur)
						 select p;

			var req2 = from p in _contexte.Produits.AsNoTracking().Include(p => p.Catégorie) select p;
			return await req.ToListAsync();
		}

		// Renvoie le produit d'id spécifié, avec sa catégorie
		public async Task<Produit?> ObtenirProduit(int id)
		{
			var req = from p in _contexte.Produits
						 .Include(p => p.Catégorie)
						 where p.Id == id
						 select p;

			return await req.FirstOrDefaultAsync();
		}

		// Crée un produit pour catégorie et un fournisseur déjà existants en base
		public async Task<Produit?> AjouterProduit(Produit produit)
		{
			produit.Catégorie = null!;

			// TODO : Contrôle les données reçues

			_contexte.Produits.Add(produit);
			await _contexte.SaveChangesAsync();
			return produit;
		}

		// Modifie un produit ou le crée si son id n'est pas fourni
		public async Task<Produit> ModifierProduit(Produit produit)
		{
			produit.Catégorie = null!;

			EntityEntry<Produit> ent = _contexte.Update(produit);
			await _contexte.SaveChangesAsync();

			// Renvoie le produit mis à jour ou créé avec son id généré par la base
			return ent.Entity;
		}

		// Applique les modifications décrites par un patch sur un produit d'id donné
		public async Task<int> ModifierProduit(int idProduit, JsonPatchDocument<Produit> patch)
		{
			// Récupère le produit en activant son suivi
			var req = from p in _contexte.Produits.AsTracking()
						 where p.Id == idProduit
						 select p;
			var produit = await req.FirstOrDefaultAsync();

			if (produit == null) return 0;

			// Applique les modifications demandées sur ce produit
			patch.ApplyTo(produit);
			return await _contexte.SaveChangesAsync();
		}

		public async Task<int> ArrêterProduitsFournisseur(int idFournisseur)
		{
			// Modifie le champ Arrete sur tous les produits du fournisseur
			int nbModifs = await _contexte.Produits.Where(p => p.IdFournisseur == idFournisseur)
				.ExecuteUpdateAsync(setter => setter.SetProperty(p => p.Arrete, true));

			return nbModifs;
		}
	}
}
