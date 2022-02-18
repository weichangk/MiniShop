using Microsoft.EntityFrameworkCore.Migrations;

namespace MiniShop.Api.Migrations
{
    public partial class UpdateSupplier : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Code",
                table: "Supplier",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "Supplier");
        }
    }
}
