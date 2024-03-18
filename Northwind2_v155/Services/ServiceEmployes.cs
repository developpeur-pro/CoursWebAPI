using Microsoft.EntityFrameworkCore;
using Northwind2_v155.Data;
using Northwind2_v155.Entities;
using Northwind2_v155.Tools;

namespace Northwind2_v155.Services
{
    public interface IServiceEmployes
	{
		Task<ServiceResult<List<Employe>>> ObtenirEmployés(string? rechercheNom);
		Task<ServiceResult<Employe?>> ObtenirEmployé(int id);
		Task<ServiceResult<Region?>> ObtenirRégion(int id);

		Task<ServiceResult<Employe?>> AjouterEmployé(Employe empl);
		Task<ServiceResult<Affectation?>> AjouterAffectation(Affectation affect);
	}

	public class ServiceEmployes : ServiceBase, IServiceEmployes
	{
		private readonly ContexteNorthwind _contexte;
		public ServiceEmployes(ContexteNorthwind context) : base(context)
		{
			_contexte = context;
		}

		// Liste d'employés avec recherche par morceau du nom
		public async Task<ServiceResult<List<Employe>>> ObtenirEmployés(string? rechercheNom)
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

			var emp = await req.ToListAsync();
			return ResultOk(emp);
		}

		// Récupère un employé par son Id
		public async Task<ServiceResult<Employe?>> ObtenirEmployé(int id)
		{
			var req = from e in _contexte.Employés
						 .Include(e => e.Adresse)
						 .Include(e => e.Territoires).ThenInclude(t => t.Région)
						 where (e.Id == id)
						 select e;

			var emp = await req.FirstOrDefaultAsync();
			return ResultOkOrNotFound(id, emp);
		}

		// Récupère une région et ses territoires
		public async Task<ServiceResult<Region?>> ObtenirRégion(int id)
		{
			var req = from r in _contexte.Régions.Include(r => r.Territoires)
						 where r.Id == id
						 select r;

			var region = await req.FirstOrDefaultAsync();
			return ResultOkOrNotFound(id, region);
		}

		// Ajoute un employé
		public async Task<ServiceResult<Employe?>> AjouterEmployé(Employe empl)
		{
			// Ajoute l'employé dans le DbSet
			_contexte.Employés.Add(empl);

			// Enregistre l'employé dans la base et récupère son Id
			return await SaveAndResultCreatedAsync(empl);
		}

		public async Task<ServiceResult<Affectation?>> AjouterAffectation(Affectation affect)
		{
			_contexte.Affectations.Add(affect);
			return await SaveAndResultCreatedAsync(affect);
		}
	}
}
