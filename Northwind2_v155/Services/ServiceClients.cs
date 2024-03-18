using Northwind2_v155.Entities;
using Northwind2_v155.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.AspNetCore.JsonPatch.Adapters;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Northwind2_v155.Tools;

namespace Northwind2_v155.Services
{
    public interface IServiceClients
	{
		Task<ServiceResult<List<Client>>> ObtenirClients();
		Task<ServiceResult<Client?>> ObtenirClient(string id);
		Task<ServiceResult<int>> ModifierClient(string idClient, JsonPatchDocument<Client> patch);
	}

	public class ServiceClients : ServiceBase, IServiceClients
	{
		private readonly ContexteNorthwind _contexte;

		public ServiceClients(ContexteNorthwind context) : base(context)
		{
			_contexte = context;
		}

		// Renvoie tous les clients
		public async Task<ServiceResult<List<Client>>> ObtenirClients()
		{
			var clients = await _contexte.Clients.ToListAsync();

			return ResultOk(clients);
		}

		// Renvoie le client d'id spécifié, avec son adresse
		public async Task<ServiceResult<Client?>> ObtenirClient(string id)
		{
			var req = from c in _contexte.Clients.Include(c => c.Adresse)
						 where c.Id == id
						 select c;

			Client? client = await req.FirstOrDefaultAsync();

			return ResultOkOrNotFound(id, client);
		}

		// Crée un client pour une catégorie et un fournisseur déjà existants en base
		public async Task<ServiceResult<Client?>> AjouterClient(Client client)
		{
			client.Adresse = null!;
			_contexte.Clients.Add(client);

			return await SaveAndResultCreatedAsync(client);
		}

		// Applique les modifications décrites par un patch sur un client d'id donné
		public async Task<ServiceResult<int>> ModifierClient(string idClient, JsonPatchDocument<Client> patch)
		{
			// Récupère le client et son adrese en activant le suivi des modifs
			var req = from c in _contexte.Clients.Include(c => c.Adresse).AsTracking()
						 where c.Id == idClient
						 select c;
			var client = await req.FirstOrDefaultAsync();

			ServiceResult<int> result = new();

			if (client == null) return ResultNotFound<int>(idClient);

			// Applique les modifications demandées sur ce client
			patch.ApplyTo(client);
			client.Commandes = null!;

			return await SaveAndResultOkAsync();
		}
	}
}
