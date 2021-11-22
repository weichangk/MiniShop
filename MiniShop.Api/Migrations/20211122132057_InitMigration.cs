using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MiniShop.Api.Migrations
{
    public partial class InitMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ShopId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ShopId = table.Column<Guid>(nullable: false),
                    CategorieId = table.Column<int>(nullable: false),
                    Code = table.Column<string>(maxLength: 32, nullable: false),
                    Name = table.Column<string>(maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Shops",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 32, nullable: false),
                    Contacts = table.Column<string>(maxLength: 32, nullable: false),
                    Phone = table.Column<string>(maxLength: 32, nullable: false),
                    Email = table.Column<string>(maxLength: 32, nullable: true),
                    Address = table.Column<string>(maxLength: 64, nullable: true),
                    ValidDate = table.Column<DateTime>(nullable: false),
                    CreateDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shops", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ShopId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 32, nullable: true),
                    Phone = table.Column<string>(maxLength: 32, nullable: true),
                    Email = table.Column<string>(maxLength: 32, nullable: true),
                    Role = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name", "ShopId" },
                values: new object[] { 1, "手机", new Guid("2cc9e247-b8ea-43fe-b085-17877cfa8473") });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "Id", "CategorieId", "Code", "Name", "ShopId" },
                values: new object[] { 1, 1, "8888888888888", "iphone18", new Guid("2cc9e247-b8ea-43fe-b085-17877cfa8473") });

            migrationBuilder.InsertData(
                table: "Shops",
                columns: new[] { "Id", "Address", "Contacts", "CreateDate", "Email", "Name", "Phone", "ValidDate" },
                values: new object[] { new Guid("2cc9e247-b8ea-43fe-b085-17877cfa8473"), "shenzhen", "alice", new DateTime(2021, 11, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "AliceSmith@shop.com", "Alice Shop", "18888888888", new DateTime(2099, 11, 11, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Name", "Phone", "Role", "ShopId" },
                values: new object[] { 1, "AliceSmith@shop.com", "alice", "18888888888", 0, new Guid("2cc9e247-b8ea-43fe-b085-17877cfa8473") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "Shops");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
