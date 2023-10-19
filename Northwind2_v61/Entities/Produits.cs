namespace Northwind2_v61.Entities
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
	}

	public class Client
	{
		public string Id { get; set; } = "";
		public Guid IdAdresse { get; set; }
		public string NomSociete { get; set; } = "";
		public string? NomContact { get; set; }
		public string? FonctionContact { get; set; }
	}

	public class Livreur
	{
		public int Id { get; set; }
		public string NomSociete { get; set; } = "";
		public string Telephone { get; set; } = "";
	}

	public class Commande
	{
		public int Id { get; set; }
		public Guid IdAdresse { get; set; }
		public string IdClient { get; set; } = "";
		public int IdEmploye { get; set; }
		public int IdLivreur { get; set; }
		public DateTime DateCommande { get; set; }
		public DateTime? DateLivMaxi { get; set; }
		public DateTime? DateLivraison { get; set; }
		public decimal? FraisLivraison { get; set; }
	}

	public class LigneCommande
	{
		public int IdCommande { get; set; }
		public int IdProduit { get; set; }
		public decimal PU { get; set; }
		public short Quantite { get; set; }
		public float TauxReduc { get; set; }
	}
}
