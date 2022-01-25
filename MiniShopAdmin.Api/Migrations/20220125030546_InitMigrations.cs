using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MiniShopAdmin.Api.Migrations
{
    public partial class InitMigrations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RenewPackage",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedTime = table.Column<DateTime>(nullable: false),
                    OperatorName = table.Column<string>(nullable: true),
                    Price = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RenewPackage", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "RenewPackage",
                columns: new[] { "Id", "CreatedTime", "ModifiedTime", "Name", "OperatorName", "Price" },
                values: new object[,]
                {
                    { 1, new DateTime(2022, 1, 25, 11, 5, 42, 990, DateTimeKind.Local).AddTicks(919), new DateTime(2022, 1, 25, 11, 5, 42, 994, DateTimeKind.Local).AddTicks(2763), "半年", null, 299m },
                    { 2, new DateTime(2022, 1, 25, 11, 5, 42, 998, DateTimeKind.Local).AddTicks(5570), new DateTime(2022, 1, 25, 11, 5, 42, 998, DateTimeKind.Local).AddTicks(5599), "一年", null, 499m },
                    { 3, new DateTime(2022, 1, 25, 11, 5, 42, 998, DateTimeKind.Local).AddTicks(8988), new DateTime(2022, 1, 25, 11, 5, 42, 998, DateTimeKind.Local).AddTicks(8996), "两年", null, 799m },
                    { 4, new DateTime(2022, 1, 25, 11, 5, 42, 999, DateTimeKind.Local).AddTicks(2828), new DateTime(2022, 1, 25, 11, 5, 42, 999, DateTimeKind.Local).AddTicks(2842), "三年", null, 1099m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_RenewPackage_Name",
                table: "RenewPackage",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RenewPackage");
        }
    }
}
