namespace Northwind2_v64.Entities
{
	public class Employe
	{
		public int Id { get; set; }
		public Guid IdAdresse { get; set; }
		public int? IdManager { get; set; }
		public string Nom { get; set; } = string.Empty;
		public string Prenom { get; set; } = string.Empty;
		public string? Fonction { get; set; }
		public string? Civilite { get; set; }
		public DateTime? DateNaissance { get; set; }
		public DateTime? DateEmbauche { get; set; }
		public byte[]? Photo { get; set; }
		public string? Notes { get; set; }

		// Propriétés de navigation
		public virtual Adresse Adresse { get; set; } = new();
		public virtual List<Territoire> Territoires { get; set; } = new();
	}

	public class Adresse
	{
		public Guid Id { get; set; }
		public string Rue { get; set; } = string.Empty;
		public string Ville { get; set; } = string.Empty;
		public string CodePostal { get; set; } = string.Empty;
		public string Pays { get; set; } = string.Empty;

		public string? Region { get; set; }
		public string? Tel { get; set; }
	}

	public class Affectation
	{
		public int IdEmploye { get; set; }
		public string IdTerritoire { get; set; } = string.Empty;
	}

	public class Territoire
	{
		public string Id { get; set; } = string.Empty;
		public int IdRegion { get; set; }
		public string Nom { get; set; } = string.Empty;

		// Propriété de navigation
		public virtual Region Région { get; set; } = null!;
	}

	public class Region
	{
		public int Id { get; set; }
		public string Nom { get; set; } = string.Empty;

		// Propriétés de navigation
		public virtual List<Territoire> Territoires { get; set; } = new();
	}
}
