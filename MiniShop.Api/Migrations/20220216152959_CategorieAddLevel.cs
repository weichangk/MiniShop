using Microsoft.EntityFrameworkCore.Migrations;

namespace MiniShop.Api.Migrations
{
    public partial class CategorieAddLevel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Code",
                table: "Categorie",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Level",
                table: "Categorie",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ParentCode",
                table: "Categorie",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Level",
                table: "Categorie");

            migrationBuilder.DropColumn(
                name: "ParentCode",
                table: "Categorie");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Categorie",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(int));
        }
    }
}
