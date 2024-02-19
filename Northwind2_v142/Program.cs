
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

			// Enregistre les contr�leurs et ajoute une option de s�rialisation
			// pour interrompre les r�f�rences circulaires infinies
			builder.Services.AddControllers()
				.AddNewtonsoftJson(opt => 
				opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)				
				.AddJsonOptions(opt =>
				opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

			// Ajoute le service d'authentification par porteur de jetons JWT
			builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
               // url d'acc�s au serveur d'identit� 
               options.Authority = builder.Configuration["IdentityServerUrl"];
               options.TokenValidationParameters.ValidateAudience = false;

               // Tol�rance sur la dur�e de validit� du jeton
               options.TokenValidationParameters.ClockSkew = TimeSpan.Zero;
            });

         // Ajoute le service d'autorisation
         builder.Services.AddAuthorization(options =>
         {
            // Sp�cifie que tout utilisateur de l'API doit �tre authentifi�
            options.FallbackPolicy = new AuthorizationPolicyBuilder()
               .RequireAuthenticatedUser()
					//.RequireClaim("Salari�")
					.Build();

				// Seuls les utilisateurs ayant la fonction Directeur
				// et appartenant au service Commerce ou RH peuvent g�rer des employ�s
				options.AddPolicy("G�rerEmploy�s",
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
				options.GroupNameFormat = "'v'VVVV"; // format du num�ro de version
				options.SubstituteApiVersionInUrl = true;
			});

			// D�finit les num�ros de version
			var versions = new[] { new ApiVersion(1.0), new ApiVersion(2.0) };

			// Cr�e les docs de d�finitions d'API
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
				// Middleware serveur de d�finition d'API
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