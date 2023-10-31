using System.ComponentModel.DataAnnotations;

namespace Northwind2_v73.Entities
{
	#region Employe et Adresse avec validation automatique
	/*
	public class Employe
	{
		public int Id { get; set; }
		public Guid IdAdresse { get; set; }
		public int? IdManager { get; set; }

		[Required(ErrorMessage = "Le nom doit être renseigné")]
		public string Nom { get; set; } = string.Empty;

		[Required(ErrorMessage = "Le prénom doit être renseignée")]
		public string Prenom { get; set; } = string.Empty;

		public string? Fonction { get; set; }
		public string? Civilite { get; set; }
		public DateTime? DateNaissance { get; set; }
		public DateTime? DateEmbauche { get; set; }
		public byte[]? Photo { get; set; }

		[MaxLength(1000, ErrorMessage = "La biographie ne doit pas dépasser 1000 caractères")]
		public string? Notes { get; set; }

		// Propriétés de navigation
		public virtual Adresse Adresse { get; set; } = new();
		public virtual List<Territoire> Territoires { get; set; } = new();

		// Méthode de validation
		public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			if (DateEmbauche != null && DateNaissance != null &&
				DateEmbauche < DateNaissance.Value.AddYears(18))
			{
				yield return new ValidationResult("La personne doit avoir au moins 18 ans pour être embauchée");
			}
		}
	}

	public class Adresse
	{
		public Guid Id { get; set; }

		[Required(ErrorMessage = "La rue doit être renseignée")]
		public string Rue { get; set; } = string.Empty;

		[Required(ErrorMessage = "La ville doit être renseignée")]
		public string Ville { get; set; } = string.Empty;

		[Required(ErrorMessage = "Le code postal doit être renseigné")]
		public string CodePostal { get; set; } = string.Empty;

		[Required(ErrorMessage = "Le pays doit être renseigné")]
		public string Pays { get; set; } = string.Empty;

		public string? Region { get; set; }

		[Phone(ErrorMessage = "Le N° ne doit contenir que des chiffres et éventuellement les caractères suivants : + - . ( ) et espace")]
		public string? Tel { get; set; }
	}
	*/
	#endregion

	#region Employe et Adresse sans validation automatique
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
	#endregion

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
