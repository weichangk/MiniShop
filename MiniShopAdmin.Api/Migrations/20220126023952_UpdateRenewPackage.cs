using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MiniShopAdmin.Api.Migrations
{
    public partial class UpdateRenewPackage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Months",
                table: "RenewPackage",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Remark",
                table: "RenewPackage",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "RenewPackage",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedTime", "Image", "ModifiedTime", "Months", "Remark" },
                values: new object[] { new DateTime(2022, 1, 26, 10, 39, 50, 989, DateTimeKind.Local).AddTicks(2558), "https://gw.alipayobjects.com/zos/rmsportal/gLaIAoVWTtLbBWZNYEMg.png", new DateTime(2022, 1, 26, 10, 39, 50, 992, DateTimeKind.Local).AddTicks(3074), 6, "希望是一个好东西，也许是最好的，好东西是不会消亡的" });

            migrationBuilder.UpdateData(
                table: "RenewPackage",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedTime", "Image", "ModifiedTime", "Months", "Remark" },
                values: new object[] { new DateTime(2022, 1, 26, 10, 39, 50, 994, DateTimeKind.Local).AddTicks(4229), "https://gw.alipayobjects.com/zos/rmsportal/iXjVmWVHbCJAyqvDxdtx.png", new DateTime(2022, 1, 26, 10, 39, 50, 994, DateTimeKind.Local).AddTicks(4249), 12, "希望是一个好东西，也许是最好的，好东西是不会消亡的" });

            migrationBuilder.UpdateData(
                table: "RenewPackage",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedTime", "Image", "ModifiedTime", "Months", "Remark" },
                values: new object[] { new DateTime(2022, 1, 26, 10, 39, 50, 994, DateTimeKind.Local).AddTicks(7038), "https://gw.alipayobjects.com/zos/rmsportal/iZBVOIhGJiAnhplqjvZW.png", new DateTime(2022, 1, 26, 10, 39, 50, 994, DateTimeKind.Local).AddTicks(7046), 24, "希望是一个好东西，也许是最好的，好东西是不会消亡的" });

            migrationBuilder.UpdateData(
                table: "RenewPackage",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedTime", "Image", "ModifiedTime", "Months", "Remark" },
                values: new object[] { new DateTime(2022, 1, 26, 10, 39, 50, 994, DateTimeKind.Local).AddTicks(9333), "https://gw.alipayobjects.com/zos/rmsportal/uMfMFlvUuceEyPpotzlq.png", new DateTime(2022, 1, 26, 10, 39, 50, 994, DateTimeKind.Local).AddTicks(9339), 36, "希望是一个好东西，也许是最好的，好东西是不会消亡的" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Months",
                table: "RenewPackage");

            migrationBuilder.DropColumn(
                name: "Remark",
                table: "RenewPackage");

            migrationBuilder.UpdateData(
                table: "RenewPackage",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedTime", "Image", "ModifiedTime" },
                values: new object[] { new DateTime(2022, 1, 25, 17, 40, 57, 697, DateTimeKind.Local).AddTicks(8367), null, new DateTime(2022, 1, 25, 17, 40, 57, 700, DateTimeKind.Local).AddTicks(9891) });

            migrationBuilder.UpdateData(
                table: "RenewPackage",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedTime", "Image", "ModifiedTime" },
                values: new object[] { new DateTime(2022, 1, 25, 17, 40, 57, 705, DateTimeKind.Local).AddTicks(1225), null, new DateTime(2022, 1, 25, 17, 40, 57, 705, DateTimeKind.Local).AddTicks(1258) });

            migrationBuilder.UpdateData(
                table: "RenewPackage",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedTime", "Image", "ModifiedTime" },
                values: new object[] { new DateTime(2022, 1, 25, 17, 40, 57, 709, DateTimeKind.Local).AddTicks(1328), null, new DateTime(2022, 1, 25, 17, 40, 57, 709, DateTimeKind.Local).AddTicks(1364) });

            migrationBuilder.UpdateData(
                table: "RenewPackage",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedTime", "Image", "ModifiedTime" },
                values: new object[] { new DateTime(2022, 1, 25, 17, 40, 57, 709, DateTimeKind.Local).AddTicks(9261), null, new DateTime(2022, 1, 25, 17, 40, 57, 709, DateTimeKind.Local).AddTicks(9281) });
        }
    }
}
