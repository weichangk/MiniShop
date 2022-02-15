using Microsoft.EntityFrameworkCore.Migrations;

namespace MiniShop.Api.Migrations
{
    public partial class UpdateCategorie : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Categorie",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "Categorie");
        }
    }
}
