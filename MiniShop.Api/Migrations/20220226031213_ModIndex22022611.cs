using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MiniShop.Api.Migrations
{
    public partial class ModIndex22022611 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Shop_ShopId_StoreId",
                table: "Shop");

            migrationBuilder.DropColumn(
                name: "StoreId",
                table: "Shop");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Item",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Store_ShopId_StoreId",
                table: "Store",
                columns: new[] { "ShopId", "StoreId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Shop_ShopId",
                table: "Shop",
                column: "ShopId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Item_ShopId_Code",
                table: "Item",
                columns: new[] { "ShopId", "Code" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Store_ShopId_StoreId",
                table: "Store");

            migrationBuilder.DropIndex(
                name: "IX_Shop_ShopId",
                table: "Shop");

            migrationBuilder.DropIndex(
                name: "IX_Item_ShopId_Code",
                table: "Item");

            migrationBuilder.AddColumn<Guid>(
                name: "StoreId",
                table: "Shop",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Item",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Shop_ShopId_StoreId",
                table: "Shop",
                columns: new[] { "ShopId", "StoreId" },
                unique: true);
        }
    }
}
