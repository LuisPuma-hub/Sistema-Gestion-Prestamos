using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaPrestamos.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Morosidad : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Morosidades_prestamos_PrestamoId",
                table: "Morosidades");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Morosidades",
                table: "Morosidades");

            migrationBuilder.DropIndex(
                name: "IX_Morosidades_PrestamoId",
                table: "Morosidades");

            migrationBuilder.RenameTable(
                name: "Morosidades",
                newName: "morosidades");

            migrationBuilder.AlterColumn<string>(
                name: "Observaciones",
                table: "morosidades",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_morosidades",
                table: "morosidades",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_morosidades_PrestamoId",
                table: "morosidades",
                column: "PrestamoId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_morosidades_prestamos_PrestamoId",
                table: "morosidades",
                column: "PrestamoId",
                principalTable: "prestamos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_morosidades_prestamos_PrestamoId",
                table: "morosidades");

            migrationBuilder.DropPrimaryKey(
                name: "PK_morosidades",
                table: "morosidades");

            migrationBuilder.DropIndex(
                name: "IX_morosidades_PrestamoId",
                table: "morosidades");

            migrationBuilder.RenameTable(
                name: "morosidades",
                newName: "Morosidades");

            migrationBuilder.AlterColumn<string>(
                name: "Observaciones",
                table: "Morosidades",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Morosidades",
                table: "Morosidades",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Morosidades_PrestamoId",
                table: "Morosidades",
                column: "PrestamoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Morosidades_prestamos_PrestamoId",
                table: "Morosidades",
                column: "PrestamoId",
                principalTable: "prestamos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
