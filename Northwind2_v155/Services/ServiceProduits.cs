using Northwind2_v155.Entities;
using Northwind2_v155.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Northwind2_v155.Tools;

namespace Northwind2_v155.Services
{
    public interface IServiceProduits
	{
		Task<ServiceResult<List<Produit>>> ObtenirProduits(Guid? idCatégorie, int? idFournisseur);
		Task<ServiceResult<Produit?>> ObtenirProduit(int id);
		Task<ServiceResult<Produit?>> AjouterProduit(Produit produit);
		Task<ServiceResult<Produit?>> ModifierProduit(Produit produit);
		Task<ServiceResult<int>> ModifierProduit(int idProduit, JsonPatchDocument<Produit> patch);
		Task<ServiceResult<int>> ArrêterProduitsFournisseur(int idFournisseur);
	}

	public class ServiceProduits : ServiceBase, IServiceProduits
	{
		private readonly ContexteNorthwind _contexte;
		public ServiceProduits(ContexteNorthwind context) : base (context)
		{
			_contexte = context;
	}

		// Renvoie les produits de la catégorie et du fournisseur spécifiés
		// ou bien tous les produits
		public async Task<ServiceResult<List<Produit>>> ObtenirProduits(Guid? idCatégorie, int? idFournisseur)
		{
			var req = from p in _contexte.Produits
						 where (idCatégorie == null || p.IdCategorie == idCatégorie) &&
								 (idFournisseur == null || p.IdFournisseur == idFournisseur)
						 select p;

			var req2 = from p in _contexte.Produits.AsNoTracking().Include(p => p.Catégorie) select p;
			var res = await req.ToListAsync();
			return ResultOk(res);
		}

		// Renvoie le produit d'id spécifié, avec sa catégorie
		public async Task<ServiceResult<Produit?>> ObtenirProduit(int id)
		{
			var req = from p in _contexte.Produits
						 .Include(p => p.Catégorie)
						 where p.Id == id
						 select p;

			var res = await req.FirstOrDefaultAsync();
			return ResultOkOrNotFound(id, res);
		}

		// Crée un produit pour catégorie et un fournisseur déjà existants en base
		public async Task<ServiceResult<Produit?>> AjouterProduit(Produit produit)
		{
			produit.Catégorie = null!;
			_contexte.Produits.Add(produit);

			// Génère une valeur pour le jeton d'accès concurrentiel
			produit.Version = Guid.NewGuid();

			return await SaveAndResultCreatedAsync(produit);
		}

		// Modifie un produit ou le crée si son id n'est pas fourni
		public async Task<ServiceResult<Produit?>> ModifierProduit(Produit produit)
		{
			produit.Catégorie = null!;
			EntityEntry<Produit> ent = _contexte.Update(produit);

			// Génère une valeur pour le jeton d'accès concurrentiel
			produit.Version = Guid.NewGuid();

			// Renvoie le produit mis à jour ou créé avec son id généré par la base
			return await SaveAndResultOkAsync(ent.Entity);
		}

		// Applique les modifications décrites par un patch sur un produit d'id donné
		public async Task<ServiceResult<int>> ModifierProduit(int idProduit, JsonPatchDocument<Produit> patch)
		{
			// Récupère le produit en activant son suivi
			var req = from p in _contexte.Produits.AsTracking()
						 where p.Id == idProduit
						 select p;
			var produit = await req.FirstOrDefaultAsync();

			if (produit == null) return ResultNotFound<int>(idProduit);

			// Applique les modifications demandées sur ce produit
			patch.ApplyTo(produit);
			return await SaveAndResultOkAsync();
		}

		public async Task<ServiceResult<int>> ArrêterProduitsFournisseur(int idFournisseur)
		{
			// Modifie le champ Arrete sur tous les produits du fournisseur
			int nbModifs = await _contexte.Produits.Where(p => p.IdFournisseur == idFournisseur)
				.ExecuteUpdateAsync(setter => setter.SetProperty(p => p.Arrete, true));

			return ResultOk(nbModifs);
		}
	}
}
