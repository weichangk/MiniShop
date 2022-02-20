using Microsoft.EntityFrameworkCore.Migrations;

namespace MiniShop.Api.Migrations
{
    public partial class UpdateEntityIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Unit_Name",
                table: "Unit");

            migrationBuilder.DropIndex(
                name: "IX_Categorie_Name",
                table: "Categorie");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Unit",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255) CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Categorie",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255) CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Unit_Code",
                table: "Unit",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Supplier_Code",
                table: "Supplier",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categorie_Code",
                table: "Categorie",
                column: "Code",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Unit_Code",
                table: "Unit");

            migrationBuilder.DropIndex(
                name: "IX_Supplier_Code",
                table: "Supplier");

            migrationBuilder.DropIndex(
                name: "IX_Categorie_Code",
                table: "Categorie");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Unit",
                type: "varchar(255) CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Categorie",
                type: "varchar(255) CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Unit_Name",
                table: "Unit",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categorie_Name",
                table: "Categorie",
                column: "Name",
                unique: true);
        }
    }
}
