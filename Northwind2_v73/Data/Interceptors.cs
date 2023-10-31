using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;

namespace Northwind2_v73.Data
{
	public class CustomExceptionCommandInterceptor : DbCommandInterceptor
	{
		private const int ForeignKeyConstraint = 547;
		private const int NotNullConstraint = 515;
		private const int UniqueConstraint = 2601;
		private const int PrimaryKeyConstraint = 2627;
		private const int NumberFormat = 8115;
		private const int MaxLengthConstaint = 8152;
	
		// Renvoie une exception plus précise à partir du code d'erreur fourni pas l'exception interne
		private static Exception GetSpecificException(SqlException sqlEx)
		{
			//if (ex.InnerException is SqlException sqlEx)
			{
				switch (sqlEx.Number)
				{
					case ForeignKeyConstraint:
						return new System.Data.InvalidConstraintException("Contrainte de clé étrangère non respectée :\n" + sqlEx.Message);

					case UniqueConstraint:
						return new System.Data.ConstraintException("Contrainte Unique non respectée : \n" + sqlEx.Message);

					case PrimaryKeyConstraint:
						return new System.Data.ConstraintException("Contrainte de clé primaire non respectée : \n" + sqlEx.Message);

					case NotNullConstraint:
						return new FormatException("Valeur Null non autorisée : \n" + sqlEx.Message);

					case NumberFormat:
						return new FormatException("Valeur numérique incorrecte : \n" + sqlEx.Message);

					case MaxLengthConstaint:
						return new FormatException("Chaîne trop longue : \n" + sqlEx.Message);

					default:
						return sqlEx;
				}
			}

			//return new NotImplementedException("GetSpecificException : exceptions d'un autre SGBD à analyser.");
		}

		// Exécuté avant SaveChangesAsync
		public override ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result, CancellationToken cancellationToken = default)
		{
			return base.ReaderExecutingAsync(command, eventData, result, cancellationToken);
		}

		// Exécuté après SaveChangesAsync s'il n'y a pas eu d'erreur
		public override ValueTask<DbDataReader> ReaderExecutedAsync(DbCommand command, CommandExecutedEventData eventData, DbDataReader result, CancellationToken cancellationToken = default)
		{
			return base.ReaderExecutedAsync(command, eventData, result, cancellationToken);
		}
		
		public override void CommandFailed(DbCommand command, CommandErrorEventData eventData)
		{
			if (eventData.Exception is SqlException e)
				throw GetSpecificException(e);

			base.CommandFailed(command, eventData);
		}

		public override Task CommandFailedAsync(DbCommand command, CommandErrorEventData eventData, CancellationToken cancellationToken = default)
		{
			if (eventData.Exception is SqlException e)
				throw GetSpecificException(e);

			return base.CommandFailedAsync(command, eventData, cancellationToken);
		}
	}
}
