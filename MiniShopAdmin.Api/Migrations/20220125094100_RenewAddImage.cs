using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MiniShopAdmin.Api.Migrations
{
    public partial class RenewAddImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "RenewPackage",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "RenewPackage",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedTime", "ModifiedTime" },
                values: new object[] { new DateTime(2022, 1, 25, 17, 40, 57, 697, DateTimeKind.Local).AddTicks(8367), new DateTime(2022, 1, 25, 17, 40, 57, 700, DateTimeKind.Local).AddTicks(9891) });

            migrationBuilder.UpdateData(
                table: "RenewPackage",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedTime", "ModifiedTime" },
                values: new object[] { new DateTime(2022, 1, 25, 17, 40, 57, 705, DateTimeKind.Local).AddTicks(1225), new DateTime(2022, 1, 25, 17, 40, 57, 705, DateTimeKind.Local).AddTicks(1258) });

            migrationBuilder.UpdateData(
                table: "RenewPackage",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedTime", "ModifiedTime" },
                values: new object[] { new DateTime(2022, 1, 25, 17, 40, 57, 709, DateTimeKind.Local).AddTicks(1328), new DateTime(2022, 1, 25, 17, 40, 57, 709, DateTimeKind.Local).AddTicks(1364) });

            migrationBuilder.UpdateData(
                table: "RenewPackage",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedTime", "ModifiedTime" },
                values: new object[] { new DateTime(2022, 1, 25, 17, 40, 57, 709, DateTimeKind.Local).AddTicks(9261), new DateTime(2022, 1, 25, 17, 40, 57, 709, DateTimeKind.Local).AddTicks(9281) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "RenewPackage");

            migrationBuilder.UpdateData(
                table: "RenewPackage",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedTime", "ModifiedTime" },
                values: new object[] { new DateTime(2022, 1, 25, 11, 5, 42, 990, DateTimeKind.Local).AddTicks(919), new DateTime(2022, 1, 25, 11, 5, 42, 994, DateTimeKind.Local).AddTicks(2763) });

            migrationBuilder.UpdateData(
                table: "RenewPackage",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedTime", "ModifiedTime" },
                values: new object[] { new DateTime(2022, 1, 25, 11, 5, 42, 998, DateTimeKind.Local).AddTicks(5570), new DateTime(2022, 1, 25, 11, 5, 42, 998, DateTimeKind.Local).AddTicks(5599) });

            migrationBuilder.UpdateData(
                table: "RenewPackage",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedTime", "ModifiedTime" },
                values: new object[] { new DateTime(2022, 1, 25, 11, 5, 42, 998, DateTimeKind.Local).AddTicks(8988), new DateTime(2022, 1, 25, 11, 5, 42, 998, DateTimeKind.Local).AddTicks(8996) });

            migrationBuilder.UpdateData(
                table: "RenewPackage",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedTime", "ModifiedTime" },
                values: new object[] { new DateTime(2022, 1, 25, 11, 5, 42, 999, DateTimeKind.Local).AddTicks(2828), new DateTime(2022, 1, 25, 11, 5, 42, 999, DateTimeKind.Local).AddTicks(2842) });
        }
    }
}
