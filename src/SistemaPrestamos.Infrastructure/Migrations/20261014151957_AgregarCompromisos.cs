using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaPrestamos.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AgregarCompromisos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "notas_compromiso",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ClienteId = table.Column<Guid>(type: "uuid", nullable: false),
                    Texto = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    FechaCompromiso = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "Pendiente"),
                    CreadoPor = table.Column<Guid>(type: "uuid", nullable: true),
                    FechaCumplimiento = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notas_compromiso", x => x.Id);
                    table.ForeignKey(
                        name: "FK_notas_compromiso_clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_notas_compromiso_ClienteId",
                table: "notas_compromiso",
                column: "ClienteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "notas_compromiso");
        }
    }
}
