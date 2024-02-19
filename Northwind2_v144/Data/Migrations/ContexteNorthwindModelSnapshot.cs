﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Northwind2_v144.Data;

#nullable disable

namespace Northwind2_v144.Data.Migrations
{
    [DbContext(typeof(ContexteNorthwind))]
    partial class ContexteNorthwindModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Northwind2_v144.Entities.Adresse", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CodePostal")
                        .IsRequired()
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)");

                    b.Property<string>("Pays")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<string>("Region")
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<string>("Rue")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Tel")
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)");

                    b.Property<string>("Ville")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.HasKey("Id");

                    b.ToTable("Adresses");
                });

            modelBuilder.Entity("Northwind2_v144.Entities.Affectation", b =>
                {
                    b.Property<int>("IdEmploye")
                        .HasColumnType("int");

                    b.Property<string>("IdTerritoire")
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)");

                    b.HasKey("IdEmploye", "IdTerritoire");

                    b.HasIndex("IdTerritoire");

                    b.ToTable("Affectations", (string)null);
                });

            modelBuilder.Entity("Northwind2_v144.Entities.Categorie", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<string>("Nom")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("Northwind2_v144.Entities.Client", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)");

                    b.Property<string>("FonctionContact")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<Guid>("IdAdresse")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("NomContact")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("NomSociete")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("IdAdresse");

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("Northwind2_v144.Entities.Commande", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DateCommande")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateLivMaxi")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateLivraison")
                        .HasColumnType("datetime2");

                    b.Property<decimal?>("FraisLivraison")
                        .HasColumnType("decimal(6,2)");

                    b.Property<Guid>("IdAdresse")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("IdClient")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.Property<int>("IdEmploye")
                        .HasColumnType("int");

                    b.Property<int>("IdLivreur")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("IdAdresse");

                    b.HasIndex("IdClient");

                    b.HasIndex("IdEmploye");

                    b.HasIndex("IdLivreur");

                    b.ToTable("Commandes");
                });

            modelBuilder.Entity("Northwind2_v144.Entities.Employe", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Civilite")
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<DateTime?>("DateEmbauche")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateNaissance")
                        .HasColumnType("datetime2");

                    b.Property<string>("Fonction")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<Guid>("IdAdresse")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("IdManager")
                        .HasColumnType("int");

                    b.Property<string>("Nom")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<string>("Notes")
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<byte[]>("Photo")
                        .HasColumnType("image");

                    b.Property<string>("Prenom")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.HasKey("Id");

                    b.HasIndex("IdAdresse")
                        .IsUnique();

                    b.HasIndex("IdManager");

                    b.ToTable("Employes", (string)null);
                });

            modelBuilder.Entity("Northwind2_v144.Entities.Fournisseur", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("FonctionContact")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<Guid>("IdAdresse")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("NomContact")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("NomSociete")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("UrlSiteWeb")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("IdAdresse");

                    b.ToTable("Fournisseurs");
                });

            modelBuilder.Entity("Northwind2_v144.Entities.LigneCommande", b =>
                {
                    b.Property<int>("IdCommande")
                        .HasColumnType("int");

                    b.Property<int>("IdProduit")
                        .HasColumnType("int");

                    b.Property<decimal>("PU")
                        .HasColumnType("decimal(8,2)");

                    b.Property<short>("Quantite")
                        .HasColumnType("smallint");

                    b.Property<float>("TauxReduc")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("real")
                        .HasDefaultValue(0f);

                    b.HasKey("IdCommande", "IdProduit");

                    b.HasIndex("IdProduit");

                    b.ToTable("LignesCommandes");
                });

            modelBuilder.Entity("Northwind2_v144.Entities.Livreur", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("NomSociete")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<string>("Telephone")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("Id");

                    b.ToTable("Livreurs");
                });

            modelBuilder.Entity("Northwind2_v144.Entities.Produit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("Arrete")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<Guid>("IdCategorie")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("IdFournisseur")
                        .HasColumnType("int");

                    b.Property<short>("NiveauReappro")
                        .HasColumnType("smallint");

                    b.Property<string>("Nom")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<decimal>("PU")
                        .HasColumnType("decimal(8,2)");

                    b.Property<short>("UnitesEnStock")
                        .HasColumnType("smallint");

                    b.HasKey("Id");

                    b.HasIndex("IdCategorie");

                    b.HasIndex("IdFournisseur");

                    b.ToTable("Produits");
                });

            modelBuilder.Entity("Northwind2_v144.Entities.Region", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("Nom")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.HasKey("Id");

                    b.ToTable("Regions", (string)null);
                });

            modelBuilder.Entity("Northwind2_v144.Entities.Territoire", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)");

                    b.Property<int>("IdRegion")
                        .HasColumnType("int");

                    b.Property<string>("Nom")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.HasKey("Id");

                    b.HasIndex("IdRegion");

                    b.ToTable("Territoires", (string)null);
                });

            modelBuilder.Entity("Northwind2_v144.Entities.Affectation", b =>
                {
                    b.HasOne("Northwind2_v144.Entities.Employe", null)
                        .WithMany()
                        .HasForeignKey("IdEmploye")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Northwind2_v144.Entities.Territoire", null)
                        .WithMany()
                        .HasForeignKey("IdTerritoire")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Northwind2_v144.Entities.Client", b =>
                {
                    b.HasOne("Northwind2_v144.Entities.Adresse", "Adresse")
                        .WithMany()
                        .HasForeignKey("IdAdresse")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Adresse");
                });

            modelBuilder.Entity("Northwind2_v144.Entities.Commande", b =>
                {
                    b.HasOne("Northwind2_v144.Entities.Adresse", "Adresse")
                        .WithMany()
                        .HasForeignKey("IdAdresse")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Northwind2_v144.Entities.Client", null)
                        .WithMany("Commandes")
                        .HasForeignKey("IdClient")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Northwind2_v144.Entities.Employe", "Employe")
                        .WithMany()
                        .HasForeignKey("IdEmploye")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Northwind2_v144.Entities.Livreur", "Livreur")
                        .WithMany()
                        .HasForeignKey("IdLivreur")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Adresse");

                    b.Navigation("Employe");

                    b.Navigation("Livreur");
                });

            modelBuilder.Entity("Northwind2_v144.Entities.Employe", b =>
                {
                    b.HasOne("Northwind2_v144.Entities.Adresse", "Adresse")
                        .WithOne()
                        .HasForeignKey("Northwind2_v144.Entities.Employe", "IdAdresse")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Northwind2_v144.Entities.Employe", null)
                        .WithMany()
                        .HasForeignKey("IdManager");

                    b.Navigation("Adresse");
                });

            modelBuilder.Entity("Northwind2_v144.Entities.Fournisseur", b =>
                {
                    b.HasOne("Northwind2_v144.Entities.Adresse", null)
                        .WithMany()
                        .HasForeignKey("IdAdresse")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });

            modelBuilder.Entity("Northwind2_v144.Entities.LigneCommande", b =>
                {
                    b.HasOne("Northwind2_v144.Entities.Commande", null)
                        .WithMany("Lignes")
                        .HasForeignKey("IdCommande")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Northwind2_v144.Entities.Produit", "Produit")
                        .WithMany()
                        .HasForeignKey("IdProduit")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Produit");
                });

            modelBuilder.Entity("Northwind2_v144.Entities.Produit", b =>
                {
                    b.HasOne("Northwind2_v144.Entities.Categorie", "Catégorie")
                        .WithMany()
                        .HasForeignKey("IdCategorie")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Northwind2_v144.Entities.Fournisseur", null)
                        .WithMany()
                        .HasForeignKey("IdFournisseur")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Catégorie");
                });

            modelBuilder.Entity("Northwind2_v144.Entities.Territoire", b =>
                {
                    b.HasOne("Northwind2_v144.Entities.Region", "Région")
                        .WithMany("Territoires")
                        .HasForeignKey("IdRegion")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Région");
                });

            modelBuilder.Entity("Northwind2_v144.Entities.Client", b =>
                {
                    b.Navigation("Commandes");
                });

            modelBuilder.Entity("Northwind2_v144.Entities.Commande", b =>
                {
                    b.Navigation("Lignes");
                });

            modelBuilder.Entity("Northwind2_v144.Entities.Region", b =>
                {
                    b.Navigation("Territoires");
                });
#pragma warning restore 612, 618
        }
    }
}
