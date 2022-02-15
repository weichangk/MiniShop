using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MiniShopAdmin.Api.Migrations
{
    public partial class updateRenewPackageRemark : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "RenewPackage",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedTime", "ModifiedTime", "Remark" },
                values: new object[] { new DateTime(2022, 2, 15, 23, 27, 33, 759, DateTimeKind.Local).AddTicks(2017), new DateTime(2022, 2, 15, 23, 27, 33, 761, DateTimeKind.Local).AddTicks(905), "续费请联系：18276743761（微信同号）" });

            migrationBuilder.UpdateData(
                table: "RenewPackage",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedTime", "ModifiedTime", "Remark" },
                values: new object[] { new DateTime(2022, 2, 15, 23, 27, 33, 764, DateTimeKind.Local).AddTicks(5981), new DateTime(2022, 2, 15, 23, 27, 33, 764, DateTimeKind.Local).AddTicks(6006), "续费请联系：18276743761（微信同号）" });

            migrationBuilder.UpdateData(
                table: "RenewPackage",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedTime", "ModifiedTime", "Remark" },
                values: new object[] { new DateTime(2022, 2, 15, 23, 27, 33, 764, DateTimeKind.Local).AddTicks(7952), new DateTime(2022, 2, 15, 23, 27, 33, 764, DateTimeKind.Local).AddTicks(7955), "续费请联系：18276743761（微信同号）" });

            migrationBuilder.UpdateData(
                table: "RenewPackage",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedTime", "ModifiedTime", "Remark" },
                values: new object[] { new DateTime(2022, 2, 15, 23, 27, 33, 764, DateTimeKind.Local).AddTicks(9755), new DateTime(2022, 2, 15, 23, 27, 33, 764, DateTimeKind.Local).AddTicks(9758), "续费请联系：18276743761（微信同号）" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "RenewPackage",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedTime", "ModifiedTime", "Remark" },
                values: new object[] { new DateTime(2022, 1, 26, 10, 39, 50, 989, DateTimeKind.Local).AddTicks(2558), new DateTime(2022, 1, 26, 10, 39, 50, 992, DateTimeKind.Local).AddTicks(3074), "希望是一个好东西，也许是最好的，好东西是不会消亡的" });

            migrationBuilder.UpdateData(
                table: "RenewPackage",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedTime", "ModifiedTime", "Remark" },
                values: new object[] { new DateTime(2022, 1, 26, 10, 39, 50, 994, DateTimeKind.Local).AddTicks(4229), new DateTime(2022, 1, 26, 10, 39, 50, 994, DateTimeKind.Local).AddTicks(4249), "希望是一个好东西，也许是最好的，好东西是不会消亡的" });

            migrationBuilder.UpdateData(
                table: "RenewPackage",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedTime", "ModifiedTime", "Remark" },
                values: new object[] { new DateTime(2022, 1, 26, 10, 39, 50, 994, DateTimeKind.Local).AddTicks(7038), new DateTime(2022, 1, 26, 10, 39, 50, 994, DateTimeKind.Local).AddTicks(7046), "希望是一个好东西，也许是最好的，好东西是不会消亡的" });

            migrationBuilder.UpdateData(
                table: "RenewPackage",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedTime", "ModifiedTime", "Remark" },
                values: new object[] { new DateTime(2022, 1, 26, 10, 39, 50, 994, DateTimeKind.Local).AddTicks(9333), new DateTime(2022, 1, 26, 10, 39, 50, 994, DateTimeKind.Local).AddTicks(9339), "希望是一个好东西，也许是最好的，好东西是不会消亡的" });
        }
    }
}
