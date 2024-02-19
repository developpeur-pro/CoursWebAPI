using Northwind2_v144.Entities;
using Northwind2_v144.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.AspNetCore.JsonPatch.Adapters;
using Microsoft.AspNetCore.JsonPatch.Operations;

namespace Northwind2_v144.V2.Services
{
	public interface IServiceClients
	{
		Task<List<Client>> ObtenirClients();
		Task<Client?> ObtenirClient(string id);
		Task<int> ModifierClient(Client client);
	}

	public class ServiceClients : IServiceClients
	{
		private readonly ContexteNorthwind _contexte;

		public ServiceClients(ContexteNorthwind context)
		{
			_contexte = context;
		}

		// Renvoie tous les clients
		public async Task<List<Client>> ObtenirClients()
		{
			return await _contexte.Clients.ToListAsync();
		}

		// Renvoie le client d'id spécifié, avec son adresse
		public async Task<Client?> ObtenirClient(string id)
		{
			var req = from c in _contexte.Clients.Include(c => c.Adresse)
						 where c.Id == id
						 select c;

			return await req.FirstOrDefaultAsync();
		}

		// Crée un Client pour catégorie et un fournisseur déjà existants en base
		public async Task<Client?> AjouterClient(Client Client)
		{
			Client.Adresse = null!;
			_contexte.Clients.Add(Client);
			await _contexte.SaveChangesAsync();
			return Client;
		}

		// Modifie un client et son adresse
		public async Task<int> ModifierClient(Client client)
		{
			client.Commandes = null!;

			_contexte.Update(client);
			return await _contexte.SaveChangesAsync();
		}
	}
}
