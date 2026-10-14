using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaPrestamos.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CompromisoPorPrestamo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PrestamoId",
                table: "notas_compromiso",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_notas_compromiso_PrestamoId",
                table: "notas_compromiso",
                column: "PrestamoId");

            migrationBuilder.AddForeignKey(
                name: "FK_notas_compromiso_prestamos_PrestamoId",
                table: "notas_compromiso",
                column: "PrestamoId",
                principalTable: "prestamos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_notas_compromiso_prestamos_PrestamoId",
                table: "notas_compromiso");

            migrationBuilder.DropIndex(
                name: "IX_notas_compromiso_PrestamoId",
                table: "notas_compromiso");

            migrationBuilder.DropColumn(
                name: "PrestamoId",
                table: "notas_compromiso");
        }
    }
}
