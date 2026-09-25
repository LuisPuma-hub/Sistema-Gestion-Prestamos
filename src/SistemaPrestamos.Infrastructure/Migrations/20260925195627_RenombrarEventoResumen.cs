using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaPrestamos.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenombrarEventoResumen : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                "UPDATE \"reglas_notificacion\" SET \"Evento\" = 'ResumenDiario' WHERE \"Evento\" = 'ResumenCobrador';");
            migrationBuilder.Sql(
                "UPDATE \"envios_notificacion\" SET \"Evento\" = 'ResumenDiario' WHERE \"Evento\" = 'ResumenCobrador';");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                "UPDATE \"reglas_notificacion\" SET \"Evento\" = 'ResumenCobrador' WHERE \"Evento\" = 'ResumenDiario';");
            migrationBuilder.Sql(
                "UPDATE \"envios_notificacion\" SET \"Evento\" = 'ResumenCobrador' WHERE \"Evento\" = 'ResumenDiario';");
        }
    }
}
