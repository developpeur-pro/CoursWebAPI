using Microsoft.EntityFrameworkCore;
using Northwind2_v53.Data;
using Northwind2_v53.Entities;

namespace Northwind2_v53.Services
{
	public interface IServiceEmployes
	{
		Task<List<Employe>> ObtenirEmployes();
		Task<Employe?> ObtenirEmploye(int id);
	}

	public class ServiceEmployes : IServiceEmployes
	{
		private readonly ContexteNorthwind _contexte;

		public ServiceEmployes(ContexteNorthwind context)
		{
			_contexte = context;
		}

		public async Task<List<Employe>> ObtenirEmployes()
		{
			return await _contexte.Employés.ToListAsync();
		}

		public async Task<Employe?> ObtenirEmploye(int id)
		{
			return await _contexte.Employés.FindAsync(id);
		}
	}
}
