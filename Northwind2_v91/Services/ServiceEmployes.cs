using Microsoft.EntityFrameworkCore;
using Northwind2_v91.Data;
using Northwind2_v91.Entities;

namespace Northwind2_v91.Services
{
	public interface IServiceEmployes
	{
		Task<List<Employe>> ObtenirEmployés(string? rechercheNom);
		//Task<List<Employe>> ObtenirEmployés(string? rechercheNom, DateTime? dateEmbaucheMax);
		Task<Employe?> ObtenirEmployé(int id);
		Task<Region?> ObtenirRégion(int id);

		Task<Employe> AjouterEmployé(Employe empl);
		Task<Affectation> AjouterAffectation(Affectation affect);
	}

	public class ServiceEmployes : IServiceEmployes
	{
		private readonly ContexteNorthwind _contexte;

		public ServiceEmployes(ContexteNorthwind context)
		{
			_contexte = context;
		}

		// Liste d'employés avec recherche par morceau du nom
		public async Task<List<Employe>> ObtenirEmployés(string? rechercheNom)
		{
			var req = from e in _contexte.Employés
						 where rechercheNom == null || e.Nom.Contains(rechercheNom)
						 select new Employe
						 {
							 Id = e.Id,
							 Civilite = e.Civilite,
							 Nom = e.Nom,
							 Prenom = e.Prenom,
							 Fonction = e.Fonction,
							 DateEmbauche = e.DateEmbauche
						 };

			return await req.ToListAsync();
		}

		// Recherche par morceau du nom + date d'embauche maximale
		/*public async Task<List<Employe>> ObtenirEmployés(string? rechercheNom, DateTime? dateEmbaucheMax)
		{
			var req = from e in _contexte.Employés
						 where (rechercheNom == null || e.Nom.Contains(rechercheNom)) &&
							 (dateEmbaucheMax == null || e.DateEmbauche <= dateEmbaucheMax)
						 select new Employe
						 {
							 Id = e.Id,
							 Civilite = e.Civilite,
							 Nom = e.Nom,
							 Prenom = e.Prenom,
							 Fonction = e.Fonction,
							 DateEmbauche = e.DateEmbauche
						 };

			return await req.ToListAsync();
		}*/

		// Recherche + tri
		/*public async Task<List<Employe>> ObtenirEmployés(string? rechercheNom, DateTime? dateEmbaucheMax)
		{
			var req = from e in _contexte.Employés
						 where (rechercheNom == null || e.Nom.Contains(rechercheNom)) &&
							  (dateEmbaucheMax == null || e.DateEmbauche <= dateEmbaucheMax)
						 select new Employe
						 {
							 Id = e.Id,
							 Civilite = e.Civilite,
							 Nom = e.Nom,
							 Prenom = e.Prenom,
							 Fonction = e.Fonction,
							 DateEmbauche = e.DateEmbauche
						 };

			// Tri par date d'embauche décroissante
			if (dateEmbaucheMax != null)
				req = req.OrderByDescending(e => e.DateEmbauche);
			else
				req = req.OrderBy(e => e.Nom).ThenBy(e => e.Prenom);

			return await req.ToListAsync();
		}*/

		// Récupère un employé par son Id
		public async Task<Employe?> ObtenirEmployé(int id)
		{
			var req = from e in _contexte.Employés
						 .Include(e => e.Adresse)
						 .Include(e => e.Territoires).ThenInclude(t => t.Région)
						 where (e.Id == id)
						 select e;

			return await req.FirstOrDefaultAsync();
		}

		// Récupère une région et ses territoires
		public async Task<Region?> ObtenirRégion(int id)
		{
			var req = from r in _contexte.Régions.Include(r => r.Territoires)
						 where r.Id == id
						 select r;

			return await req.FirstOrDefaultAsync();
		}

		// Ajoute un employé
		public async Task<Employe> AjouterEmployé(Employe empl)
		{
			// Ajoute l'employé dans le DbSet
			_contexte.Employés.Add(empl);

			// Enregistre l'employé dans la base et affecte son Id
			await _contexte.SaveChangesAsync();

			return empl; // Renvoie l'employé avec son Id renseigné
		}

		public async Task<Affectation> AjouterAffectation(Affectation affect)
		{
			_contexte.Affectations.Add(affect);
			await _contexte.SaveChangesAsync();
			return affect;
		}
	}
}
