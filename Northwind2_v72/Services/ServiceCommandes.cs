using Northwind2_v72.Entities;
using Northwind2_v72.Data;
using Microsoft.EntityFrameworkCore;

namespace Northwind2_v72.Services
{
	public interface IServiceCommandes
	{
		Task<List<Commande>> ObtenirCommandes(int? idEmployé, string ? idClient);
		Task<Commande?> ObtenirCommande(int id);
		Task<Commande?> AjouterCommande(Commande cde);
		Task<LigneCommande?> AjouterLigneCommande(int idCommande, LigneCommande ligne);
	}

	public class ServiceCommandes : IServiceCommandes
	{
		private readonly ContexteNorthwind _contexte;

		public ServiceCommandes(ContexteNorthwind context)
		{
			_contexte = context;
		}

		// Renvoie les commandes liées à un employé et un client s'ils sont renseignés
		// sinon toutes les commandes
		public async Task<List<Commande>> ObtenirCommandes(int? idEmployé = null, string ? idClient = null)
		{
			var req = from c in _contexte.Commandes
						 where (idEmployé == null || c.IdEmploye == idEmployé) &&
								 (idClient == null || c.IdClient == idClient)
						 select c;

			return await req.ToListAsync();
		}

		// Renvoie la commande d'id spécifié, avec ses lignes
		public async Task<Commande?> ObtenirCommande(int id)
		{
			var req = from c in _contexte.Commandes
						 .Include(c => c.Lignes)
						 where c.Id == id
						 select c;

			return await req.FirstOrDefaultAsync();
		}

		// Crée une commande pour un employé, une adresse, un client et
		// un livreur déjà existants en base
		public async Task<Commande?> AjouterCommande(Commande cde)
		{
			// On remet les propriétés de navigation correspondantes à null
			cde.Employe = null!;
			cde.Adresse = null!;
			cde.Livreur = null!;
			foreach (var ligne in cde.Lignes) ligne.Produit = null!;
			
			_contexte.Commandes.Add(cde);
			await _contexte.SaveChangesAsync();

			return cde;
		}

		// Crée une ligne de commande pour une commande donnée
		public async Task<LigneCommande?> AjouterLigneCommande(int idCommande, LigneCommande ligne)
		{
			ligne.IdCommande = idCommande;
			ligne.Produit = null!;

			_contexte.LignesCommandes.Add(ligne);
			await _contexte.SaveChangesAsync();

			return ligne;
		}
	}
}
