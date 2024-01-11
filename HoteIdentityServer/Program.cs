using Duende.IdentityServer.Models;
using HoteIdentityServer.Data;
using HoteIdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
	 options.UseSqlServer(connectionString));
// Capture les exceptions de base de données résolvables par migration
// et envoie une r�ponse HTML invitant à migrer la base (à utiliser en mode dev uniquement)
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Ajoute des services d'identités communs :
// - une interface utilisateur par défaut,
// - des fournisseurs de jetons pour générer des jetons afin de réinitialiser les mots de passe,
// modifier l'e-mail et modifier le N° de tel, et pour l'authentification à 2 facteurs.
// - configure l'authentification pour utiliser les cookies d'identité
builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
	 .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddRazorPages();


// Ajoute et configure le service IdentityServer
builder.Services.AddIdentityServer(options =>
		options.Authentication.CoordinateClientLifetimesWithUserSession = true)

	 // Crée des identités
	 .AddInMemoryIdentityResources(new IdentityResource[] {
			new IdentityResources.OpenId(),
			new IdentityResources.Profile(),
	 })

	 // Configure une appli cliente
	 .AddInMemoryClients(new Client[] {
			new Client
			{
				ClientId = "Client1",
				ClientSecrets = { new Secret("Secret1".Sha256()) },
				AllowedGrantTypes = GrantTypes.Code,
            
            // Urls auxquelles envoyer les jetons
            RedirectUris = { "https://localhost:7189/signin-oidc" },
            // Urls de redirection après déconnexion
            PostLogoutRedirectUris = { "https://localhost:7189/signout-callback-oidc" },
            // Url pour envoyer une demande de déconnexion au serveur d'identité
            FrontChannelLogoutUri = "https://localhost:7189/signout-oidc",

				// Etendues d'API autorisées
				AllowedScopes = { "openid", "profile" },

				// ----------------------------------------------------------
				// Paramétrage des jetons et de leur rafraîchissement

				// Durée de validité du jeton d'identité (en secondes)
				IdentityTokenLifetime = 30,
				// Durée de validité du jeton d'accès à l'API
				AccessTokenLifetime = 40,

            // Autorise le client à utiliser un jeton d'actualisation
				AllowOfflineAccess = true,

				// Durée de validité maximale du jeton d'actualisation
				AbsoluteRefreshTokenLifetime = 3600,

				// Réinitialise la durée de validité du jeton d'actualisation
				// à chaque actualisation du jeton d'accès
				RefreshTokenExpiration = TokenExpiration.Sliding, 
				// Durée de validité glissante du jeton d'actualisation
				SlidingRefreshTokenLifetime = 60,
			}
	 })
	 // Indique d'utiliser ASP.Net Core Identity pour la gestion des profils et revendications
	 .AddAspNetIdentity<ApplicationUser>();

// Ajoute la journalisation au niveau debug des événements émis par Duende
builder.Services.AddLogging(options =>
{
	options.AddFilter("Duende", LogLevel.Debug);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseMigrationsEndPoint();
}
else
{
	app.UseExceptionHandler("/Error");
	// Ajoute un en-tête de réponse qui informe les navigateurs que le site ne doit être accessible qu'en utilisant HTTPS,
	// et que toute tentative future d'y accéder via HTTP doit être automatiquement convertie en HTTPS.
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Ajoute le middleware d'authentification avec IdentityServer dans le pipeline
app.UseIdentityServer();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
