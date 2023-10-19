using Microsoft.EntityFrameworkCore;
using Northwind2_v53.Data;
using Northwind2_v53.Services;

namespace Northwind2_v53
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
			builder.Services.AddDbContext<ContexteNorthwind>(opt => opt.UseSqlServer(connect));

			// Enregistre les services m�tier
			builder.Services.AddScoped<IServiceEmployes, ServiceEmployes>();

			// Enregistre les contr�leurs
			builder.Services.AddControllers();

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


			app.MapControllers();

			app.Run();
		}
	}
}