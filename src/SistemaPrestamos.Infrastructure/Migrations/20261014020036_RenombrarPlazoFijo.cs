using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaPrestamos.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenombrarPlazoFijo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CuotaDiaria",
                table: "prestamos",
                newName: "CuotaFija");

            migrationBuilder.RenameColumn(
                name: "EsDiaria",
                table: "periodos_interes",
                newName: "EsCuotaFija");

            migrationBuilder.AddColumn<string>(
                name: "FrecuenciaCuota",
                table: "prestamos",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "Diaria");

            // Compat: la primera versión guardaba "Diaria".
            migrationBuilder.Sql(
                "UPDATE prestamos SET \"ModalidadCobro\" = 'PlazoFijo', " +
                "\"FrecuenciaCuota\" = 'Diaria' " +
                "WHERE \"ModalidadCobro\" = 'Diaria';");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FrecuenciaCuota",
                table: "prestamos");

            migrationBuilder.RenameColumn(
                name: "CuotaFija",
                table: "prestamos",
                newName: "CuotaDiaria");

            migrationBuilder.RenameColumn(
                name: "EsCuotaFija",
                table: "periodos_interes",
                newName: "EsDiaria");
        }
    }
}
