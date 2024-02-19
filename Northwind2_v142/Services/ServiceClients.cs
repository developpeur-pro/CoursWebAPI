using Northwind2_v142.Entities;
using Northwind2_v142.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.AspNetCore.JsonPatch.Adapters;
using Microsoft.AspNetCore.JsonPatch.Operations;

namespace Northwind2_v142.Services
{
	public interface IServiceClients
	{
		Task<List<Client>> ObtenirClients();
		Task<Client?> ObtenirClient(string id);
		Task<int> ModifierClient(string idClient, JsonPatchDocument<Client> patch);
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

		// Applique les modifications décrites par un patch sur un client d'id donné
		public async Task<int> ModifierClient(string idClient, JsonPatchDocument<Client> patch)
		{
			// Récupère le client et son adrese en activant le suivi des modifs
			var req = from c in _contexte.Clients.Include(c => c.Adresse).AsTracking()
						 where c.Id == idClient
						 select c;
			var client = await req.FirstOrDefaultAsync();

			if (client == null) return 0;
			
			// Applique les modifications demandées sur ce client
			patch.ApplyTo(client);
			client.Commandes = null!;

			return await _contexte.SaveChangesAsync();
		}
	}
}
