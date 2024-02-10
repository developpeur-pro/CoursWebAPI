using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TestNorthwind
{
	[TestClass]
	public class TestInit
	{
		public static DbContextOptions<ContexteNorthwind> ContextOptions { get; private set; } = null!;
		public static JsonSerializerOptions JsonOptions { get; private set; } = null!;

		[AssemblyInitialize]
		public static void InitialiserAssembly(TestContext context)
		{
			string SqlServerInstance = "(localdb)\\mssqllocaldb";
			string dbName = "Northwind2_Tests";

			// Restaure la sauvegarde de la base de données
			string backupPath = $@"D:\BDD\Backups\{dbName}.bak";
			using SqlConnection connection = new SqlConnection($"Server={SqlServerInstance};Trusted_Connection=True");
			connection.Open();

			string sql = $"RESTORE DATABASE {dbName} FROM DISK = '{backupPath}' WITH REPLACE;";
			using (SqlCommand command = new SqlCommand(sql, connection))
			{
				command.ExecuteNonQuery();
			}

			// Options de DbContext pour utiliser
			// la base SQL Server de test sans suivi des modifs
			string connectString = $"Server={SqlServerInstance};Database={dbName};Trusted_Connection=True";
			DbContextOptionsBuilder<ContexteNorthwind> builder = new();
			ContextOptions = builder.UseSqlServer(connectString)
				.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
				.Options;

			// Options de sérialiseur JSON pour couper les références circulaires
			// et utiliser un convertisseur pour la (dé)sérialisation des DateTime
			JsonOptions = new()
			{
				WriteIndented = true,
				ReferenceHandler = ReferenceHandler.IgnoreCycles,
				Converters = { new DateTimeJsonConverter() },
			};
		}
	}
}
