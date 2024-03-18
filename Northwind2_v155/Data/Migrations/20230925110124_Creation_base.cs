using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Northwind2_v155.Data.Migrations
{
    /// <inheritdoc />
    public partial class Creation_base : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Adresses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Rue = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Ville = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    CodePostal = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    Pays = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Region = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    Tel = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Adresses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Regions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Nom = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Employes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdAdresse = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdManager = table.Column<int>(type: "int", nullable: true),
                    Nom = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Prenom = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Fonction = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    Civilite = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    DateNaissance = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateEmbauche = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Photo = table.Column<byte[]>(type: "image", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employes_Adresses_IdAdresse",
                        column: x => x.IdAdresse,
                        principalTable: "Adresses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Employes_Employes_IdManager",
                        column: x => x.IdManager,
                        principalTable: "Employes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Territoires",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    IdRegion = table.Column<int>(type: "int", nullable: false),
                    Nom = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Territoires", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Territoires_Regions_IdRegion",
                        column: x => x.IdRegion,
                        principalTable: "Regions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Affectations",
                columns: table => new
                {
                    IdEmploye = table.Column<int>(type: "int", nullable: false),
                    IdTerritoire = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Affectations", x => new { x.IdEmploye, x.IdTerritoire });
                    table.ForeignKey(
                        name: "FK_Affectations_Employes_IdEmploye",
                        column: x => x.IdEmploye,
                        principalTable: "Employes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Affectations_Territoires_IdTerritoire",
                        column: x => x.IdTerritoire,
                        principalTable: "Territoires",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Affectations_IdTerritoire",
                table: "Affectations",
                column: "IdTerritoire");

            migrationBuilder.CreateIndex(
                name: "IX_Employes_IdAdresse",
                table: "Employes",
                column: "IdAdresse",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employes_IdManager",
                table: "Employes",
                column: "IdManager");

            migrationBuilder.CreateIndex(
                name: "IX_Territoires_IdRegion",
                table: "Territoires",
                column: "IdRegion");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Affectations");

            migrationBuilder.DropTable(
                name: "Employes");

            migrationBuilder.DropTable(
                name: "Territoires");

            migrationBuilder.DropTable(
                name: "Adresses");

            migrationBuilder.DropTable(
                name: "Regions");
        }
    }
}
