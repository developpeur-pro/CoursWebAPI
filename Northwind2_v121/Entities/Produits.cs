namespace Northwind2_v121.Entities
{
	public class Categorie
	{
		public Guid Id { get; set; }
		public string Nom { get; set; } = "";
		public string? Description { get; set; }
	}

	public class Fournisseur
	{
		public int Id { get; set; }
		public Guid IdAdresse { get; set; }
		public string NomSociete { get; set; } = "";
		public string? NomContact { get; set; }
		public string? FonctionContact { get; set; }
		public string? UrlSiteWeb { get; set; }
	}
	
	public class Produit
	{
		public int Id { get; set; }
		public Guid IdCategorie { get; set; }
		public int IdFournisseur { get; set; }
		public string Nom { get; set; } = "";
		public decimal	PU { get; set; }
		public short UnitesEnStock { get; set; }
		public short NiveauReappro { get; set; }
		public bool Arrete { get; set; }

		public Categorie Catégorie { get; set; } = new();
	}
}
