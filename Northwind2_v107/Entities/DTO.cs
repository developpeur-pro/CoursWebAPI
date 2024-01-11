namespace Northwind2_v107.Entities
{
	public class FormEmploye
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

		// Pour récupérer la photo et la biographie sous forme de fichiers
		public IFormFile? Photo { get; set; }
		public IFormFile? Notes { get; set; }

		public Adresse Adresse { get; set; } = null!;
	}
}
