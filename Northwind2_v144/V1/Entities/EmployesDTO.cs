using Northwind2_v144.Entities;

namespace Northwind2_v144.V1.Entities
{
	public class EmployeDTO
	{
		public int Id { get; set; }
		public Guid IdAdresse { get; set; }
		public int? IdManager { get; set; }
		public string Nom { get; set; } = string.Empty;
		public string Prenom { get; set; } = string.Empty;
		public string? Fonction { get; set; } = string.Empty;
		public string? Civilite { get; set; }
		public DateTime? DateNaissance { get; set; }
		public DateTime? DateEmbauche { get; set; }
		public byte[]? Photo { get; set; }
		public string? Notes { get; set; }

		// Propriétés de navigation
		public virtual Adresse Adresse { get; set; } = new();
		public virtual List<Territoire> Territoires { get; set; } = new();
	}

	public class FormEmployeDTO
	{
		public int Id { get; set; }
		public Guid IdAdresse { get; set; }
		public int? IdManager { get; set; }
		public string Nom { get; set; } = string.Empty;
		public string Prenom { get; set; } = string.Empty;
		public string? Fonction { get; set; } = string.Empty;
		public string? Civilite { get; set; }
		public DateTime? DateNaissance { get; set; }
		public DateTime? DateEmbauche { get; set; }

		// Pour récupérer la photo et la biographie sous forme de fichiers
		public IFormFile? Photo { get; set; }
		public IFormFile? Notes { get; set; }

		public Adresse Adresse { get; set; } = null!;
	}
}
