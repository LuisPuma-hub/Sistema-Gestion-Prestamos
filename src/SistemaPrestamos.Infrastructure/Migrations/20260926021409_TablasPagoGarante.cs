using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaPrestamos.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class TablasPagoGarante : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Garantes_clientes_ClienteId",
                table: "Garantes");

            migrationBuilder.DropForeignKey(
                name: "FK_Pagos_prestamos_PrestamoId",
                table: "Pagos");

            migrationBuilder.DropForeignKey(
                name: "FK_prestamos_Garantes_GaranteId",
                table: "prestamos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Pagos",
                table: "Pagos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Garantes",
                table: "Garantes");

            migrationBuilder.RenameTable(
                name: "Pagos",
                newName: "pagos");

            migrationBuilder.RenameTable(
                name: "Garantes",
                newName: "garantes");

            migrationBuilder.RenameIndex(
                name: "IX_Pagos_PrestamoId",
                table: "pagos",
                newName: "IX_pagos_PrestamoId");

            migrationBuilder.RenameIndex(
                name: "IX_Garantes_ClienteId",
                table: "garantes",
                newName: "IX_garantes_ClienteId");

            migrationBuilder.AlterColumn<string>(
                name: "Observaciones",
                table: "pagos",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "MontoInteres",
                table: "pagos",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<decimal>(
                name: "MontoCapital",
                table: "pagos",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<decimal>(
                name: "Monto",
                table: "pagos",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<string>(
                name: "Comprobante",
                table: "pagos",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Telefono",
                table: "garantes",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Nombres",
                table: "garantes",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Direccion",
                table: "garantes",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Apellidos",
                table: "garantes",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddPrimaryKey(
                name: "PK_pagos",
                table: "pagos",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_garantes",
                table: "garantes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_garantes_clientes_ClienteId",
                table: "garantes",
                column: "ClienteId",
                principalTable: "clientes",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_pagos_prestamos_PrestamoId",
                table: "pagos",
                column: "PrestamoId",
                principalTable: "prestamos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_prestamos_garantes_GaranteId",
                table: "prestamos",
                column: "GaranteId",
                principalTable: "garantes",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_garantes_clientes_ClienteId",
                table: "garantes");

            migrationBuilder.DropForeignKey(
                name: "FK_pagos_prestamos_PrestamoId",
                table: "pagos");

            migrationBuilder.DropForeignKey(
                name: "FK_prestamos_garantes_GaranteId",
                table: "prestamos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_pagos",
                table: "pagos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_garantes",
                table: "garantes");

            migrationBuilder.RenameTable(
                name: "pagos",
                newName: "Pagos");

            migrationBuilder.RenameTable(
                name: "garantes",
                newName: "Garantes");

            migrationBuilder.RenameIndex(
                name: "IX_pagos_PrestamoId",
                table: "Pagos",
                newName: "IX_Pagos_PrestamoId");

            migrationBuilder.RenameIndex(
                name: "IX_garantes_ClienteId",
                table: "Garantes",
                newName: "IX_Garantes_ClienteId");

            migrationBuilder.AlterColumn<string>(
                name: "Observaciones",
                table: "Pagos",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "MontoInteres",
                table: "Pagos",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "MontoCapital",
                table: "Pagos",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "Monto",
                table: "Pagos",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AlterColumn<string>(
                name: "Comprobante",
                table: "Pagos",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Telefono",
                table: "Garantes",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "Nombres",
                table: "Garantes",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Direccion",
                table: "Garantes",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Apellidos",
                table: "Garantes",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Pagos",
                table: "Pagos",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Garantes",
                table: "Garantes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Garantes_clientes_ClienteId",
                table: "Garantes",
                column: "ClienteId",
                principalTable: "clientes",
                principalColumn: "Id");

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
        }
    }
}
