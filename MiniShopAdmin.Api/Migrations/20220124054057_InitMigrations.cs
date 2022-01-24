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
                    { 1, new DateTime(2022, 1, 24, 13, 40, 56, 73, DateTimeKind.Local).AddTicks(8431), new DateTime(2022, 1, 24, 13, 40, 56, 76, DateTimeKind.Local).AddTicks(5401), "半年", null, 299m },
                    { 2, new DateTime(2022, 1, 24, 13, 40, 56, 78, DateTimeKind.Local).AddTicks(2492), new DateTime(2022, 1, 24, 13, 40, 56, 78, DateTimeKind.Local).AddTicks(2512), "一年", null, 499m },
                    { 3, new DateTime(2022, 1, 24, 13, 40, 56, 78, DateTimeKind.Local).AddTicks(4836), new DateTime(2022, 1, 24, 13, 40, 56, 78, DateTimeKind.Local).AddTicks(4841), "两年", null, 799m },
                    { 4, new DateTime(2022, 1, 24, 13, 40, 56, 78, DateTimeKind.Local).AddTicks(7544), new DateTime(2022, 1, 24, 13, 40, 56, 78, DateTimeKind.Local).AddTicks(7553), "三年", null, 1099m }
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
