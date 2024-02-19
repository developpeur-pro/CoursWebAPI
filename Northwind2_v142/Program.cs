
using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Northwind2_v142.Data;
using Northwind2_v142.Middlewares;
using Northwind2_v142.Services;
using NSwag.AspNetCore;
using Serilog;
using System;
using System.Text.Json.Serialization;

namespace Northwind2_v142
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Récupère la chaîne de connexion à la base dans les paramètres
			string? connect = builder.Configuration.GetConnectionString("Northwind2Connect");

			// Add services to the container.
			// Enregistre la classe de contexte de données comme service
			// en lui indiquant la connexion à utiliser, et désactive le suivi des modifications
			builder.Services.AddDbContext<ContexteNorthwind>(opt => opt
				.UseSqlServer(connect)
				.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
				.EnableSensitiveDataLogging());

			// Enregistre les services métier
			builder.Services.AddScoped<IServiceEmployes, ServiceEmployes>();
			builder.Services.AddScoped<IServiceCommandes, ServiceCommandes>();
			builder.Services.AddScoped<IServiceProduits, ServiceProduits>();
			builder.Services.AddScoped<IServiceClients, ServiceClients>();

			// Enregistre les contrôleurs et ajoute une option de sérialisation
			// pour interrompre les références circulaires infinies
			builder.Services.AddControllers()
				.AddNewtonsoftJson(opt => 
				opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)				
				.AddJsonOptions(opt =>
				opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

			// Ajoute le service d'authentification par porteur de jetons JWT
			builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
               // url d'accès au serveur d'identité 
               options.Authority = builder.Configuration["IdentityServerUrl"];
               options.TokenValidationParameters.ValidateAudience = false;

               // Tolérance sur la durée de validité du jeton
               options.TokenValidationParameters.ClockSkew = TimeSpan.Zero;
            });

         // Ajoute le service d'autorisation
         builder.Services.AddAuthorization(options =>
         {
            // Spécifie que tout utilisateur de l'API doit être authentifié
            options.FallbackPolicy = new AuthorizationPolicyBuilder()
               .RequireAuthenticatedUser()
					//.RequireClaim("Salarié")
					.Build();

				// Seuls les utilisateurs ayant la fonction Directeur
				// et appartenant au service Commerce ou RH peuvent gérer des employés
				options.AddPolicy("GérerEmployés",
						  p => p.RequireClaim("Fonction", "Directeur"));
			});

			// Enregistre le service de versionnage
			builder.Services.AddApiVersioning(options => {
				options.ApiVersionReader = ApiVersionReader.Combine(
					new UrlSegmentApiVersionReader(),
					new QueryStringApiVersionReader(),
					new MediaTypeApiVersionReader("version"),
					new HeaderApiVersionReader("x-api-version"));
				options.AssumeDefaultVersionWhenUnspecified = true;
			})
			.AddMvc()
			.AddApiExplorer(options =>
			{
				options.GroupNameFormat = "'v'VVVV"; // format du numéro de version
				options.SubstituteApiVersionInUrl = true;
			});

			// Définit les numéros de version
			var versions = new[] { new ApiVersion(1.0), new ApiVersion(2.0) };

			// Crée les docs de définitions d'API
			foreach (ApiVersion vers in versions)
			{
				builder.Services.AddOpenApiDocument(options =>
				{
					string version = vers.ToString("'v'VVVV");
					options.DocumentName = version;
					options.ApiGroupNames = new[] { version };
					options.Title = "API Northwind";
					options.Description = "La description de l'API";
					options.Version = version;
				});
			}

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				// Middleware serveur de définition d'API
				app.UseOpenApi();

				// Interface web pour la doc
				app.UseSwaggerUi(options =>
				{
					foreach (ApiVersion vers in versions)
					{
						string version = vers.ToString("'v'VVVV");
						var route = new SwaggerUiRoute(version, $"/swagger/{version}/swagger.json");
						options.SwaggerRoutes.Add(route);
					}
				});
			}

			app.UseHttpsRedirection();
			app.UseAuthorization();

			var endpointBuilder = app.MapControllers();
			if (app.Environment.IsDevelopment())
				endpointBuilder.AllowAnonymous();

			app.Run();
		}
	}
}