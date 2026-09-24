using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaPrestamos.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Notificaciones : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "reglas_notificacion",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nombre = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    Evento = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    Canal = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Hora = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    DiasSemana = table.Column<int>(type: "integer", nullable: false),
                    Plantilla = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Activa = table.Column<bool>(type: "boolean", nullable: false),
                    UltimaEjecucion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reglas_notificacion", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "envios_notificacion",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ReglaId = table.Column<Guid>(type: "uuid", nullable: true),
                    Evento = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    Canal = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    ClienteId = table.Column<Guid>(type: "uuid", nullable: true),
                    PrestamoId = table.Column<Guid>(type: "uuid", nullable: true),
                    UsuarioId = table.Column<Guid>(type: "uuid", nullable: true),
                    Destinatario = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Detalle = table.Column<string>(type: "text", nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_envios_notificacion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_envios_notificacion_reglas_notificacion_ReglaId",
                        column: x => x.ReglaId,
                        principalTable: "reglas_notificacion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_envios_notificacion_ReglaId",
                table: "envios_notificacion",
                column: "ReglaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "envios_notificacion");

            migrationBuilder.DropTable(
                name: "reglas_notificacion");
        }
    }
}
