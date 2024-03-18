using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Northwind2_v155.Data;
using Northwind2_v155.Services;
using System.Text.Json.Serialization;

namespace Northwind2_v155
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

			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			//builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddOpenApiDocument(options =>
			{
				options.Title = "API Northwind";
				options.Description = "<strong>API Northwind2 pour formation ASP.Net Core.<br/>Code dispo sur <a href='https://github.com/developpeur-pro/CoursWebAPI'>ce référentiel GitHub</a></strong>";
				options.Version = "v1";
			});

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

         var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				// Middleware serveur de définition d'API
				app.UseOpenApi();

				// Interface web pour la doc
				app.UseSwaggerUi();
			}

			app.UseHttpsRedirection();
			app.UseAuthorization();

			var endpointBuilder = app.MapControllers();
			//if (app.Environment.IsDevelopment())
				endpointBuilder.AllowAnonymous();

			app.Run();
		}
	}
}