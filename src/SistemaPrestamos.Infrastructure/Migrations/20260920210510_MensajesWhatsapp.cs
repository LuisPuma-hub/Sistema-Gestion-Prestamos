using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaPrestamos.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MensajesWhatsapp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "mensajes_whatsapp",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ClienteId = table.Column<Guid>(type: "uuid", nullable: true),
                    PrestamoId = table.Column<Guid>(type: "uuid", nullable: true),
                    TipoPlantilla = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    NumeroDestino = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Contenido = table.Column<string>(type: "text", nullable: false),
                    Estado = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    IdentificadorExterno = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    FechaEnvio = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mensajes_whatsapp", x => x.Id);
                    table.ForeignKey(
                        name: "FK_mensajes_whatsapp_clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "clientes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_mensajes_whatsapp_prestamos_PrestamoId",
                        column: x => x.PrestamoId,
                        principalTable: "prestamos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_mensajes_whatsapp_ClienteId",
                table: "mensajes_whatsapp",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_mensajes_whatsapp_PrestamoId",
                table: "mensajes_whatsapp",
                column: "PrestamoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "mensajes_whatsapp");
        }
    }
}
