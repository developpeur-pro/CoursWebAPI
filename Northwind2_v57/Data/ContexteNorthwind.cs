using Microsoft.EntityFrameworkCore;
using Northwind2_v57.Entities;

namespace Northwind2_v57.Data
{
	public class ContexteNorthwind : DbContext
	{
		public ContexteNorthwind(DbContextOptions<ContexteNorthwind> options)
			 : base(options)
		{
		}

		public virtual DbSet<Adresse> Adresses { get; set; }
		public virtual DbSet<Employe> Employés { get; set; }
		public virtual DbSet<Region> Régions { get; set; }
		public virtual DbSet<Territoire> Territoires { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Adresse>(entity =>
			{
				entity.HasKey(e => e.Id);
				entity.Property(e => e.Id).ValueGeneratedNever();
				entity.Property(e => e.Ville).HasMaxLength(40);
				entity.Property(e => e.Pays).HasMaxLength(40);
				entity.Property(e => e.Tel).HasMaxLength(20).IsUnicode(false);
				entity.Property(e => e.CodePostal).HasMaxLength(20).IsUnicode(false);
				entity.Property(e => e.Region).HasMaxLength(40);
				entity.Property(e => e.Rue).HasMaxLength(100);
			}) ;

			modelBuilder.Entity<Employe>(entity =>
			{
				entity.ToTable("Employes");
				entity.HasKey(e => e.Id);

				entity.Property(e => e.Prenom).HasMaxLength(40);
				entity.Property(e => e.Nom).HasMaxLength(40);
				entity.Property(e => e.Notes).HasMaxLength(1000);
				entity.Property(e => e.Photo).HasColumnType("image");
				entity.Property(e => e.Fonction).HasMaxLength(40);
				entity.Property(e => e.Civilite).HasMaxLength(40);

				// Relation de la table Employe sur elle-même 
				entity.HasOne<Employe>().WithMany().HasForeignKey(d => d.IdManager);

				// Relation Employe - Adresse de cardinalités 0,1 - 1,1
				entity.HasOne(e => e.Adresse).WithOne().HasForeignKey<Employe>(d => d.IdAdresse)
						.OnDelete(DeleteBehavior.NoAction);
			});

			modelBuilder.Entity<Affectation>(entity =>
			{
				entity.ToTable("Affectations");
				entity.HasKey(e => new { e.IdEmploye, e.IdTerritoire });

				entity.Property(e => e.IdTerritoire).HasMaxLength(20).IsUnicode(false);
				entity.HasOne<Employe>().WithMany().HasForeignKey(d => d.IdEmploye);
				entity.HasOne<Territoire>().WithMany().HasForeignKey(d => d.IdTerritoire);
			});

			modelBuilder.Entity<Region>(entity =>
			{
				entity.ToTable("Regions");
				entity.HasKey(e => e.Id);

				entity.Property(e => e.Id).ValueGeneratedNever();
				entity.Property(e => e.Nom).HasMaxLength(40);
			});

			modelBuilder.Entity<Territoire>(entity =>
			{
				entity.ToTable("Territoires");
				entity.HasKey(e => e.Id);

				entity.Property(e => e.Id).HasMaxLength(20).IsUnicode(false);
				entity.Property(e => e.Nom).HasMaxLength(40);

				// Relation avec la région utilisant une propriété de navigation
				entity.HasOne(t => t.Région).WithMany(r => r.Territoires)
						.HasForeignKey(d => d.IdRegion).OnDelete(DeleteBehavior.NoAction);

				// Crée la relation N-N avec Employe en utilisant l'entité Affectation comme entité d'association
				// ainsi qu'une propriété de navigation
				entity.HasMany<Employe>().WithMany(e => e.Territoires).UsingEntity<Affectation>(				
					l => l.HasOne<Employe>().WithMany().HasForeignKey(a => a.IdEmploye),
					r => r.HasOne<Territoire>().WithMany().HasForeignKey(a => a.IdTerritoire));
			});

		}
	}
}
