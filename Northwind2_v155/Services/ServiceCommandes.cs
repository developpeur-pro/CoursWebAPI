using Northwind2_v155.Entities;
using Northwind2_v155.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NuGet.Packaging;
using Northwind2_v155.Tools;

namespace Northwind2_v155.Services
{
    public interface IServiceCommandes
	{
		Task<ServiceResult<List<Commande>>> ObtenirCommandes(int? idEmployé, string? idClient);
		Task<ServiceResult<Commande?>> ObtenirCommande(int id);
		Task<ServiceResult<Commande?>> AjouterCommande(Commande cde);
		Task<ServiceResult<int>> SupprimerCommande(int idCommande);
		Task<ServiceResult<int>> SupprimerCommande2(int idCommande);
		Task<ServiceResult<int>> SupprimerCommande3(int idCommande);
		Task<ServiceResult<int>> ModifierCommande(Commande cde);
		Task<ServiceResult<int>> ModifierCommande2(int idCommande, float tauxReduc);

		Task<ServiceResult<LigneCommande?>> AjouterLigneCommande(int idCommande, LigneCommande ligne);
		Task<ServiceResult<int>> SupprimerLigneCommande(int idCommande, int idProduit);
		Task<ServiceResult<int>> SupprimerLigneCommande2(int idCommande, int idProduit);
		Task<ServiceResult<int>> ModifierLigneCommande(int idCommande, LigneCommande ligne);
		Task<ServiceResult<int>> ModifierLigneCommande2(int idCommande, LigneCommande ligne);
		Task<ServiceResult<LigneCommande?>> ModifierLigneCommande3(int idCommande, int idProduit, short quantité);
	}

	public class ServiceCommandes : ServiceBase, IServiceCommandes
	{
		private readonly ContexteNorthwind _contexte;
		public ServiceCommandes(ContexteNorthwind context) : base(context)
		{
			_contexte = context;
		}

		#region Commandes
		// Renvoie les commandes liées à un employé et un client s'ils sont renseignés
		// sinon toutes les commandes
		public async Task<ServiceResult<List<Commande>>> ObtenirCommandes(int? idEmployé = null, string? idClient = null)
		{
			var req = from c in _contexte.Commandes
						 where (idEmployé == null || c.IdEmploye == idEmployé) &&
								 (idClient == null || c.IdClient == idClient)
						 select c;

			var commandes = await req.ToListAsync();
			return ResultOk(commandes);
		}

		// Renvoie la commande d'id spécifié, avec ses lignes
		public async Task<ServiceResult<Commande?>> ObtenirCommande(int id)
		{
			var req = from c in _contexte.Commandes
						 .Include(c => c.Lignes)
						 where c.Id == id
						 select c;

			var cde = await req.FirstOrDefaultAsync();

			// Renvoie un résultat OK contenant la commande,
			// ou bien un résultat NotFound si l'id n'a pas été trouvé
			return ResultOkOrNotFound(id, cde);
		}

		// Crée une commande pour un employé, une adresse, un client et
		// un livreur déjà existants en base
		public async Task<ServiceResult<Commande?>> AjouterCommande(Commande cde)
		{
			// On remet les propriétés de navigation correspondantes à null
			cde.Employe = null!;
			cde.Adresse = null!;
			cde.Livreur = null!;
			foreach (var ligne in cde.Lignes) ligne.Produit = null!;

			// Contrôle les données reçues
			var erreursCde = await ControlerCommande(cde);

			// S'il y a au moins une erreur, on renvoie un résultat Invalid contenant les erreurs
			if (erreursCde.Any()) return ResultInvalidData<Commande?>(erreursCde);

			_contexte.Commandes.Add(cde);

			// Enregistre et renvoie un résultat Created contenant les données créées
			// ou un résultat d'erreur si une erreur et survenue à l'enregistrement
			return await SaveAndResultCreatedAsync(cde);
		}

		private async Task<ErrorsDictionary> ControlerCommande(Commande cde)
		{
			ErrorsDictionary erreurs = new();

			if (cde.DateCommande < DateTime.Today.AddDays(-3))
				erreurs.Add("DateCommande", "La date de commande doit être > date du jour - 3 jours.");

			if (cde.FraisLivraison < 0 || cde.FraisLivraison > 2000)
				erreurs.Add("Frais", "Les frais de livraison doivent être compris entre 0 et 2000 €");

			if (erreurs.Any()) return erreurs;

			foreach (var ligne in cde.Lignes)
			{
				var errLigne = await ControlerLigneCommande(ligne);
				if (errLigne.Any()) erreurs.Merge(errLigne);
			}

			return erreurs;
		}

		// Supprime une commande et ses lignes grâce à la suppression en cascade
		public async Task<ServiceResult<int>> SupprimerCommande(int idCommande)
		{
			Commande commande = new() { Id = idCommande };
			_contexte.Entry(commande).State = EntityState.Deleted;
			return await SaveAndResultOkAsync();
		}

		// Supprime une commande et ses lignes via des requêtes SQL de masse
		public async Task<ServiceResult<int>> SupprimerCommande2(int idComm)
		{
			using (var transaction = _contexte.Database.BeginTransaction())
			{
				await _contexte.LignesCommandes.Where(c => c.IdCommande == idComm).ExecuteDeleteAsync();
				int nbSuppr = await _contexte.Commandes.Where(c => c.Id == idComm).ExecuteDeleteAsync();
				transaction.Commit();
				return ResultOk(nbSuppr);
			}
		}

		// Supprime une commande et ses lignes en chargeant tout d'abord les entités
		public async Task<ServiceResult<int>> SupprimerCommande3(int idCommande)
		{
			// Récupère la commande et ses lignes sans les rattacher au suivi
			var req = from c in _contexte.Commandes.Include(c => c.Lignes)
						 where c.Id == idCommande
						 select c;

			var cde = await req.FirstOrDefaultAsync();
			if (cde == null) return ResultNotFound<int>(idCommande);

			// Passe la commande et ses lignes à l'état Deleted
			//_contexte.Remove(cde);
			_contexte.Entry(cde).State = EntityState.Deleted;
			foreach (var ligne in cde.Lignes)
			{
				_contexte.Entry(ligne).State = EntityState.Deleted;
			}

			return await SaveAndResultOkAsync();
		}

		// Modifie une commande avec ses lignes
		public async Task<ServiceResult<int>> ModifierCommande(Commande cde)
		{
			// Contrôle les données reçues
			var erreursCde = await ControlerCommande(cde);
			if (erreursCde.Any()) return ResultInvalidData<int>(erreursCde);

			// Passe la commande et ses lignes à l'état Modified
			_contexte.Entry(cde).State = EntityState.Modified;
			foreach (var ligne in cde.Lignes)
			{
				_contexte.Entry(ligne).State = EntityState.Modified;
			}

			return await SaveAndResultOkAsync();
		}

		// Met à jour le taux de réduction sur toutes les lignes d'une commande
		public async Task<ServiceResult<int>> ModifierCommande2(int idCommande, float tauxReduc)
		{
			int nbmodifs = await _contexte.LignesCommandes.Where(lc => lc.IdCommande == idCommande)
				.ExecuteUpdateAsync(setter => setter.SetProperty(l => l.TauxReduc, tauxReduc));

			return ResultOk(nbmodifs);
		}
		#endregion

		#region Lignes de commandes
		// Crée une ligne de commande pour une commande donnée
		public async Task<ServiceResult<LigneCommande?>> AjouterLigneCommande(int idCommande, LigneCommande ligne)
		{
			ligne.IdCommande = idCommande;
			ligne.Produit = null!;

			// Contrôle les données reçues
			var erreurs = await ControlerLigneCommande(ligne);
			if (erreurs.Any()) return ResultInvalidData<LigneCommande?>(erreurs);

			_contexte.LignesCommandes.Add(ligne);
			return await SaveAndResultCreatedAsync(ligne);
		}

		// Supprime une ligne de commande
		public async Task<ServiceResult<int>> SupprimerLigneCommande(int idCommande, int idProduit)
		{
			LigneCommande ligne = new()
			{
				IdCommande = idCommande,
				IdProduit = idProduit,
				//Produit = null!
			};

			//_contexte.Remove(ligne);
			//_contexte.Attach(ligne).State = EntityState.Deleted;
			_contexte.Entry(ligne).State = EntityState.Deleted;

			return await SaveAndResultOkAsync();
		}

		// Supprime une ligne de commande en vérifiant tout d'abord qu'elle existe
		public async Task<ServiceResult<int>> SupprimerLigneCommande2(int idCommande, int idProduit)
		{
			LigneCommande? ligne = await _contexte.LignesCommandes.FindAsync(idCommande, idProduit);
			if (ligne == null) return ResultNotFound<int>(new { idCommande, idProduit });
			else ligne.Produit = null!;

			_contexte.Remove(ligne);
			return await SaveAndResultOkAsync();
		}

		public async Task<ServiceResult<int>> ModifierLigneCommande(int idCommande, LigneCommande ligne)
		{
			ligne.IdCommande = idCommande;
			// Remet les propriétés de navigation à null
			ligne.Produit = null!;

			// Contrôle les données reçues
			var erreurs = await ControlerLigneCommande(ligne);
			if (erreurs.Any()) return ResultInvalidData<int>(erreurs);

			// Rattache l'entité au suivi en passant son état à Modified
			_contexte.Update(ligne);

			return await SaveAndResultOkAsync();
			// Lève une DbUpdateConcurrencyException si la ligne n'existe pas en base
		}

		public async Task<ServiceResult<int>> ModifierLigneCommande2(int idCommande, LigneCommande ligne)
		{
			ligne.IdCommande = idCommande;

			// Contrôle les données reçues
			var erreurs = await ControlerLigneCommande(ligne);
			if (erreurs.Any()) return ResultInvalidData<int>(erreurs);

			// Rattache l'entité au suivi, sans ses filles, en passant son état à Modified
			EntityEntry<LigneCommande> ent = _contexte.Entry(ligne);
			ent.State = EntityState.Modified;

			// Empêche la modification du prix unitaire de la ligne
			ent.Property(l => l.PU).IsModified = false;

			return await SaveAndResultOkAsync();
		}

		public async Task<ServiceResult<LigneCommande?>> ModifierLigneCommande3(int idCommande, int idProduit, short quantité)
		{
			// Récupère la ligne présente en base avec suivi des modifs
			var req = from l in _contexte.LignesCommandes.AsTracking()
						 where l.IdCommande == idCommande && l.IdProduit == idProduit
						 select l;

			LigneCommande? ligne = await req.SingleOrDefaultAsync();
			if (ligne == null) throw new DbUpdateConcurrencyException();

			// Modifie les propriétés souhaitées
			ligne.Quantite = quantité;
			ligne.Produit = null!;

			// Contrôle la ligne
			var erreurs = await ControlerLigneCommande(ligne);
			if (erreurs.Any()) return ResultInvalidData<LigneCommande?>(erreurs);

			// Enregistre les modifications
			return await SaveAndResultOkAsync(ligne);
		}

		private async Task<ErrorsDictionary> ControlerLigneCommande(LigneCommande ligne)
		{
			Produit? produit = await _contexte.Produits.FindAsync(ligne.IdProduit);

			ErrorsDictionary erreurs = new();

			if (produit == null || produit.Arrete)
				erreurs.Add("Produit.Arrete", $"Le produit {ligne.IdProduit} n'existe pas ou a été arrêté.");

			if (produit != null && produit.UnitesEnStock < ligne.Quantite)
				erreurs.Add("Produit.UnitesEnStock",
					$"La quantité en stock ({produit.UnitesEnStock}) du produit {ligne.IdProduit} est insuffisante.");

			return erreurs;
		}
		#endregion
	}
}
