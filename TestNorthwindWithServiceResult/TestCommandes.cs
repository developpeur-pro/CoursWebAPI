using System.Text.Json;

namespace TestNorthwindWithServiceResult
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

		[DataTestMethod]
		[DataRow(1, 3)]
		[DataRow(47, 5)]
		public async Task ObtenirCommandeExistante(int idCde, int nbLignes)
		{
			ResultKinds resKind = ResultKinds.Ok;

			var res = await _service.ObtenirCommande(idCde);

			Assert.AreEqual(resKind, res.ResultKind);
			Assert.IsNotNull(res.Data);
			Assert.AreEqual(nbLignes, res.Data?.Lignes.Count);
		}

		public async Task ObtenirCommandeInexistante()
		{
			int idCde = 1;
			ResultKinds resKind = ResultKinds.NotFound;

			var res = await _service.ObtenirCommande(idCde);

			Assert.AreEqual(resKind, res.ResultKind);
			Assert.IsNull(res.Data);
			Assert.AreEqual(1, res.Errors.Count);
		}

		[DataTestMethod]
		[DataRow(6, "VINET", 1)]
		[DataRow(6, null, 67)]
		[DataRow(null, "VINET", 5)]
		public async Task ObtenirCommandes(int? idEmployé, string? idClient, int nbCommandes)
		{
			var res = await _service.ObtenirCommandes(idEmployé, idClient);

			Assert.AreEqual(nbCommandes, res.Data.Count);
		}

		[TestMethod]
		public async Task AjouterCommandeTropAncienne()
		{
			_nouvelleCde.DateCommande = DateTime.Today.AddDays(-4);
			var res = await _service.AjouterCommande(_nouvelleCde);

			Assert.AreEqual(ResultKinds.InvalidData, res.ResultKind);
			Assert.IsTrue(res.Errors.Any());
			Assert.IsNull(res.Data);
		}

		[TestMethod]
		public async Task AjouterCommandeAvecFraisTropElevés()
		{
			_nouvelleCde.FraisLivraison = 3000m;
			var res = await _service.AjouterCommande(_nouvelleCde);

			Assert.AreEqual(ResultKinds.InvalidData, res.ResultKind);
			Assert.IsTrue(res.Errors.Any());
			Assert.IsNull(res.Data);
		}

		[TestMethod]
		public async Task AjouterCommandeAvecLigne()
		{
			_nouvelleCde.Lignes.Add(_ligne);
			var res = await _service.AjouterCommande(_nouvelleCde);

			Assert.AreEqual(ResultKinds.Created, res.ResultKind);
			Assert.IsFalse(res.Errors.Any());
			Assert.IsNotNull(res.Data);
			// Si la commande a bien été ajoutée, son Id a été généré par la base
			Assert.IsTrue(res.Data.Id > 0);
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
						Quantite = 12,
						Version = new Byte[] { 0,0,0,0,0,0,0x8,0x8B }
					}
				}
			};

			// Enregistre les modifications en base
			var resModif = await _service.ModifierCommande(cde);
			Assert.AreEqual(ResultKinds.Ok, resModif.ResultKind);

			//Récupère la commande modifiée
			var resObt = await _service.ObtenirCommande(cde.Id);

			// Pour comparer les 2 objets, on les sérialise en JSON
			string json1 = JsonSerializer.Serialize(cde, TestInit.JsonOptions);
			string json2 = JsonSerializer.Serialize(resObt.Data, TestInit.JsonOptions);

			Assert.AreEqual(json1, json2);
		}

		[TestMethod]
		public async Task SupprimerCommande()
		{
			int idCde = 2;
			var res = await _service.SupprimerCommande(idCde);

			Assert.AreEqual(ResultKinds.Ok, res.ResultKind);
			Assert.AreEqual(1, res.Data);
		}
	}
}