using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaPrestamos.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AnulacionDetalleReset : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AnuladoPor",
                table: "pagos",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Detalle",
                table: "pagos",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Estado",
                table: "pagos",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "Registrado");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaAnulacion",
                table: "pagos",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MotivoAnulacion",
                table: "pagos",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "password_resets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uuid", nullable: false),
                    Token = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ExpiraEn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Usado = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_password_resets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_password_resets_usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_password_resets_Token",
                table: "password_resets",
                column: "Token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_password_resets_UsuarioId",
                table: "password_resets",
                column: "UsuarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "password_resets");

            migrationBuilder.DropColumn(
                name: "AnuladoPor",
                table: "pagos");

            migrationBuilder.DropColumn(
                name: "Detalle",
                table: "pagos");

            migrationBuilder.DropColumn(
                name: "Estado",
                table: "pagos");

            migrationBuilder.DropColumn(
                name: "FechaAnulacion",
                table: "pagos");

            migrationBuilder.DropColumn(
                name: "MotivoAnulacion",
                table: "pagos");
        }
    }
}
