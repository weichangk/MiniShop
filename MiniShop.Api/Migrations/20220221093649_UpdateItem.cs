using Microsoft.EntityFrameworkCore.Migrations;

namespace MiniShop.Api.Migrations
{
    public partial class UpdateItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Item",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Item");
        }
    }
}
