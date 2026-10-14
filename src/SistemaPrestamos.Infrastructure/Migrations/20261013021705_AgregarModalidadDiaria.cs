using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaPrestamos.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AgregarModalidadDiaria : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "CuotaDiaria",
                table: "prestamos",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaFin",
                table: "prestamos",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModalidadCobro",
                table: "prestamos",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "Semanal");

            migrationBuilder.AddColumn<decimal>(
                name: "MontoTotal",
                table: "prestamos",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "PlazoCuotas",
                table: "prestamos",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EsDiaria",
                table: "periodos_interes",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CuotaDiaria",
                table: "prestamos");

            migrationBuilder.DropColumn(
                name: "FechaFin",
                table: "prestamos");

            migrationBuilder.DropColumn(
                name: "ModalidadCobro",
                table: "prestamos");

            migrationBuilder.DropColumn(
                name: "MontoTotal",
                table: "prestamos");

            migrationBuilder.DropColumn(
                name: "PlazoCuotas",
                table: "prestamos");

            migrationBuilder.DropColumn(
                name: "EsDiaria",
                table: "periodos_interes");
        }
    }
}
