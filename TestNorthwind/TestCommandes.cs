using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.Json;

namespace TestNorthwind
{
	[TestClass]
	public class TestCommandes
	{
		private IServiceCommandes _service = null!;
		private Commande _nouvelleCde = null!;
		private LigneCommande _ligne = null!;

		[TestInitialize]
		public void InitialiserTest()
		{
			// Initialise un DbContext et le service métier
			_service = new ServiceCommandes(new ContexteNorthwind(TestInit.ContextOptions));

			// Initialise une nouvelle commande
			_nouvelleCde = new Commande()
			{
				IdAdresse = new Guid("167B6047-052C-49A0-9196-38404E6E335C"),
				IdClient = "BONAP",
				IdEmploye = 5,
				IdLivreur = 2,
				DateCommande = DateTime.Today,
				FraisLivraison = 6m,
			};

			// Initialise une ligne de commande
			_ligne = new LigneCommande()
			{
				IdProduit = 10,
				PU = 31.9m,
				Quantite = 1
			};
		}

		[TestMethod]
		public async Task ObtenirCommande()
		{
			int idCde = 1;
			int nbLignes = 3;

			Commande? cde = await _service.ObtenirCommande(idCde);

			Assert.IsNotNull(cde);
			Assert.AreEqual(nbLignes, cde.Lignes.Count);
		}

		[DataTestMethod]
		[DataRow(6, "VINET", 1)]
		[DataRow(6, null, 67)]
		[DataRow(null, "VINET", 5)]
		public async Task ObtenirCommandes(int? idEmployé, string? idClient, int nbCommandes)
		{
			List<Commande> cdes = await _service.ObtenirCommandes(idEmployé, idClient);

			Assert.AreEqual(nbCommandes, cdes.Count);
		}

		[TestMethod]
		[ExpectedException(typeof(ValidationRulesException))]
		public async Task AjouterCommandeTropAncienne()
		{
			_nouvelleCde.DateCommande = DateTime.Today.AddDays(-4);
			await _service.AjouterCommande(_nouvelleCde);
		}

		[TestMethod]
		[ExpectedException(typeof(ValidationRulesException))]
		public async Task AjouterCommandeAvecFraisTropElevés()
		{
			_nouvelleCde.FraisLivraison = 3000m;
			await _service.AjouterCommande(_nouvelleCde);
		}

		[TestMethod]
		public async Task AjouterCommandeAvecLigne()
		{
			_nouvelleCde.Lignes.Add(_ligne);
			await _service.AjouterCommande(_nouvelleCde);

			// Si la commande a bien été ajoutée, son Id a été généré par la base
			Assert.IsTrue(_nouvelleCde.Id > 0);
		}

		[TestMethod]
		public async Task ModifierCommandeAvecLigne()
		{
			// Initialise une commande déjà existante dans la base
			// en modifiant certaines de ses propriétés
			Commande cde = new Commande()
			{
				Id = 66,
				IdAdresse = new Guid("D14C5109-ABCF-433A-9774-04366590543A"),
				IdClient = "QUICK",
				IdEmploye = 2,
				IdLivreur = 2,
				DateCommande = DateTime.Today,  // valeur initiale : 24/09/2021
				DateLivMaxi = DateTime.Today.AddDays(10), // valeur initiale : 22/10/2021
				DateLivraison = null, // valeur initiale : 04/10/2021
				FraisLivraison = 8.00m, // valeur initiale : 1.96,
				Lignes = new List<LigneCommande>() {
					new LigneCommande()
					{
						IdCommande = 66,
						IdProduit = 36,
						PU = 16.00m, // valeur initiale : 15.20
						Quantite = 12
					}
				}
			};

			// Enregistre les modifications en base, puis récupère la commande modifiée
			await _service.ModifierCommande(cde);
			Commande? cdeLue = await _service.ObtenirCommande(cde.Id);

			// Pour comparer les 2 objets, on les sérialise en JSON
			string json1 = JsonSerializer.Serialize(cde, TestInit.JsonOptions);
			string json2 = JsonSerializer.Serialize(cdeLue, TestInit.JsonOptions);

			Assert.AreEqual(json1, json2);
		}

		[TestMethod]
		public async Task SupprimerCommande()
		{
			int idCde = 2;
			int nbSuppr = await _service.SupprimerCommande(idCde);

			Assert.AreEqual(1, nbSuppr);
		}
	}
}