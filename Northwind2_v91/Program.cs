
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Northwind2_v91.Data;
using Northwind2_v91.Middlewares;
using Northwind2_v91.Services;
using Serilog;
using System;
using System.Text.Json.Serialization;

namespace Northwind2_v91
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// R�cup�re la cha�ne de connexion � la base dans les param�tres
			string? connect = builder.Configuration.GetConnectionString("Northwind2Connect");

			// Add services to the container.
			// Enregistre la classe de contexte de donn�es comme service
			// en lui indiquant la connexion � utiliser, et d�sactive le suivi des modifications
			builder.Services.AddDbContext<ContexteNorthwind>(opt => opt
				.UseSqlServer(connect)
				.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
				.EnableSensitiveDataLogging());

			// Enregistre les services m�tier
			builder.Services.AddScoped<IServiceEmployes, ServiceEmployes>();
			builder.Services.AddScoped<IServiceCommandes, ServiceCommandes>();
			builder.Services.AddScoped<IServiceProduits, ServiceProduits>();
			builder.Services.AddScoped<IServiceClients, ServiceClients>();

			// Utilise Serilog comme unique fournisseur de journalisation
			/*var logger = new LoggerConfiguration()
				 .ReadFrom.Configuration(builder.Configuration)
				 .Enrich.FromLogContext()
				 .CreateLogger();
			builder.Logging.ClearProviders();
			builder.Logging.AddSerilog(logger);*/

			// Enregistre les contr�leurs et ajoute une option de s�rialisation
			// pour interrompre les r�f�rences circulaires infinies
			builder.Services.AddControllers()
				.AddNewtonsoftJson()				
				.AddJsonOptions(opt =>
				opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();
			app.UseAuthorization();

			// Middleware personnalis� de gestion d'erreurs
			//app.UseMiddleware<CustomErrorResponseMiddleware>(app.Logger);

			app.MapControllers();

			app.Run();
		}
	}
}