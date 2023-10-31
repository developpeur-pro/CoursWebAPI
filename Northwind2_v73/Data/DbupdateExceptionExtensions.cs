using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Northwind2_v73.Data
{
	public static class DbUpdateExceptionExtensions
	{
		private const int ForeignKeyConstraint = 547;
		private const int NotNullConstraint = 515;
		private const int UniqueConstraint = 2601;
		private const int PrimaryKeyConstraint = 2627;
		private const int NumericValue = 8115;
		private const int MaxLengthConstaint = 8152;

		// Traduit une DbUpdateException en un code HTTP et un message d'erreur adaptés
		public static (int, string) TranslateToHttpResponse(this DbUpdateException ex)
		{
			if (ex.InnerException is SqlException sqlEx)
			{
				switch (sqlEx.Number)
				{
					case ForeignKeyConstraint:
						return (400, "La requête fait référence à un enregistrement qui n'existe pas dans la base" +
							" ou bien tente de supprimer un enregistrement référencé ailleurs dans la base");

					case UniqueConstraint:
					case PrimaryKeyConstraint:
						return (409, "Un enregistrement de même identifiant existe déjà dans la base.");

					case NotNullConstraint:
						return (400, "La valeur Null a été fournie pour un champ qui ne peut pas être Null.");

					case NumericValue:
						return (400, "Une valeur numérique incorrecte a été fournie.");

					case MaxLengthConstaint:
						return (400, "Une chaîne trop longue a été fournie.");

					default:
						return (500, "Erreur dans la base de données.");
				}
			}
			else
			{
				throw new NotImplementedException("Traduction des erreurs en réponses HTTP non implémentée pour ce SGBD");
			}
		}
	}
}
