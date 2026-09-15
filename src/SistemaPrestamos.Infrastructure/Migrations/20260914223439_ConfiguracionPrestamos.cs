using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaPrestamos.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ConfiguracionPrestamos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Morosidades_Prestamos_PrestamoId",
                table: "Morosidades");

            migrationBuilder.DropForeignKey(
                name: "FK_Pagos_Prestamos_PrestamoId",
                table: "Pagos");

            migrationBuilder.DropForeignKey(
                name: "FK_Prestamos_Garantes_GaranteId",
                table: "Prestamos");

            migrationBuilder.DropForeignKey(
                name: "FK_Prestamos_clientes_ClienteId",
                table: "Prestamos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Prestamos",
                table: "Prestamos");

            migrationBuilder.RenameTable(
                name: "Prestamos",
                newName: "prestamos");

            migrationBuilder.RenameIndex(
                name: "IX_Prestamos_GaranteId",
                table: "prestamos",
                newName: "IX_prestamos_GaranteId");

            migrationBuilder.RenameIndex(
                name: "IX_Prestamos_ClienteId",
                table: "prestamos",
                newName: "IX_prestamos_ClienteId");

            migrationBuilder.AlterColumn<decimal>(
                name: "TasaInteresSemanal",
                table: "prestamos",
                type: "numeric(10,6)",
                precision: 10,
                scale: 6,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<string>(
                name: "Estado",
                table: "prestamos",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<decimal>(
                name: "CapitalPendiente",
                table: "prestamos",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<decimal>(
                name: "CapitalInicial",
                table: "prestamos",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AddPrimaryKey(
                name: "PK_prestamos",
                table: "prestamos",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Morosidades_prestamos_PrestamoId",
                table: "Morosidades",
                column: "PrestamoId",
                principalTable: "prestamos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Pagos_prestamos_PrestamoId",
                table: "Pagos",
                column: "PrestamoId",
                principalTable: "prestamos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_prestamos_Garantes_GaranteId",
                table: "prestamos",
                column: "GaranteId",
                principalTable: "Garantes",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_prestamos_clientes_ClienteId",
                table: "prestamos",
                column: "ClienteId",
                principalTable: "clientes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Morosidades_prestamos_PrestamoId",
                table: "Morosidades");

            migrationBuilder.DropForeignKey(
                name: "FK_Pagos_prestamos_PrestamoId",
                table: "Pagos");

            migrationBuilder.DropForeignKey(
                name: "FK_prestamos_Garantes_GaranteId",
                table: "prestamos");

            migrationBuilder.DropForeignKey(
                name: "FK_prestamos_clientes_ClienteId",
                table: "prestamos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_prestamos",
                table: "prestamos");

            migrationBuilder.RenameTable(
                name: "prestamos",
                newName: "Prestamos");

            migrationBuilder.RenameIndex(
                name: "IX_prestamos_GaranteId",
                table: "Prestamos",
                newName: "IX_Prestamos_GaranteId");

            migrationBuilder.RenameIndex(
                name: "IX_prestamos_ClienteId",
                table: "Prestamos",
                newName: "IX_Prestamos_ClienteId");

            migrationBuilder.AlterColumn<decimal>(
                name: "TasaInteresSemanal",
                table: "Prestamos",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(10,6)",
                oldPrecision: 10,
                oldScale: 6);

            migrationBuilder.AlterColumn<string>(
                name: "Estado",
                table: "Prestamos",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<decimal>(
                name: "CapitalPendiente",
                table: "Prestamos",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "CapitalInicial",
                table: "Prestamos",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Prestamos",
                table: "Prestamos",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Morosidades_Prestamos_PrestamoId",
                table: "Morosidades",
                column: "PrestamoId",
                principalTable: "Prestamos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Pagos_Prestamos_PrestamoId",
                table: "Pagos",
                column: "PrestamoId",
                principalTable: "Prestamos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Prestamos_Garantes_GaranteId",
                table: "Prestamos",
                column: "GaranteId",
                principalTable: "Garantes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Prestamos_clientes_ClienteId",
                table: "Prestamos",
                column: "ClienteId",
                principalTable: "clientes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
