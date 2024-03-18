using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Northwind2_v155.Data.Migrations
{
    /// <inheritdoc />
    public partial class JetonsAccesConcurrentiels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "Version",
                table: "Produits",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<byte[]>(
                name: "Version",
                table: "LignesCommandes",
                type: "rowversion",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Version",
                table: "Produits");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "LignesCommandes");
        }
    }
}
