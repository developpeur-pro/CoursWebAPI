namespace TestNorthwindWithServiceResult
{
    [TestClass]
	public class TestProduits
	{
		private IServiceProduits _service = null!;
		private Produit _nouveauProduit = null!;

		[TestInitialize]
		public void InitialiserTest()
		{
			// Initialise un DbContext et le service métier
			_service = new ServiceProduits(new ContexteNorthwind(TestInit.ContextOptions));

			// Initialise un  nouveau produit
			_nouveauProduit = new()
			{
				IdCategorie = new Guid("C350B4D1-68AF-4A92-B2AB-54D7FB597A40"),
				IdFournisseur = 1,
				Nom = "Crême au chocolat 1L",
				PU = 7m,
				UnitesEnStock = 25,
				NiveauReappro = 5,
			};
		}

		[TestMethod]
		public async Task AjouterProduitNomTropLong()
		{
			_nouveauProduit.Nom = "Un nom trop long qui dépasse les 40 caractères";
			var res = await _service.AjouterProduit(_nouveauProduit);

			Assert.AreEqual(ResultKinds.InvalidData, res.ResultKind);
			Assert.IsTrue(res.Errors.TryGetErrors("Contrainte MaxLength", out _));
			Assert.IsNull(res.Data);
		}
	}
}