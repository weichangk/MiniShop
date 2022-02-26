using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MiniShop.Api.Migrations
{
    public partial class AddPurchaseRelateAndModIndex22022610 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Vip_Code",
                table: "Vip");

            migrationBuilder.DropIndex(
                name: "IX_Unit_Code",
                table: "Unit");

            migrationBuilder.DropIndex(
                name: "IX_Supplier_Code",
                table: "Supplier");

            migrationBuilder.DropIndex(
                name: "IX_Store_StoreId",
                table: "Store");

            migrationBuilder.DropIndex(
                name: "IX_Shop_ShopId",
                table: "Shop");

            migrationBuilder.DropIndex(
                name: "IX_Categorie_Code",
                table: "Categorie");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Store",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "StoreId",
                table: "Shop",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "PurchaseOder",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ShopId = table.Column<Guid>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedTime = table.Column<DateTime>(nullable: false),
                    OperatorName = table.Column<string>(nullable: true),
                    OderNo = table.Column<string>(nullable: true),
                    StoreId = table.Column<int>(nullable: false),
                    SupplierId = table.Column<int>(nullable: false),
                    OderAmount = table.Column<decimal>(nullable: false),
                    AuditState = table.Column<int>(nullable: false),
                    AuditOperatorName = table.Column<string>(nullable: true),
                    AuditTime = table.Column<DateTime>(nullable: true),
                    OrderState = table.Column<int>(nullable: false),
                    Remark = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseOder", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseOder_Store_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Store",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchaseOder_Supplier_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Supplier",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseReceiveOder",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ShopId = table.Column<Guid>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedTime = table.Column<DateTime>(nullable: false),
                    OperatorName = table.Column<string>(nullable: true),
                    OderNo = table.Column<string>(nullable: true),
                    StoreId = table.Column<int>(nullable: false),
                    SupplierId = table.Column<int>(nullable: false),
                    OderAmount = table.Column<decimal>(nullable: false),
                    AuditState = table.Column<int>(nullable: false),
                    AuditOperatorName = table.Column<string>(nullable: true),
                    AuditTime = table.Column<DateTime>(nullable: true),
                    OrderState = table.Column<int>(nullable: false),
                    Remark = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseReceiveOder", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseReceiveOder_Store_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Store",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchaseReceiveOder_Supplier_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Supplier",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseReturnOder",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ShopId = table.Column<Guid>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedTime = table.Column<DateTime>(nullable: false),
                    OperatorName = table.Column<string>(nullable: true),
                    OderNo = table.Column<string>(nullable: true),
                    StoreId = table.Column<int>(nullable: false),
                    SupplierId = table.Column<int>(nullable: false),
                    OderAmount = table.Column<decimal>(nullable: false),
                    AuditState = table.Column<int>(nullable: false),
                    AuditOperatorName = table.Column<string>(nullable: true),
                    AuditTime = table.Column<DateTime>(nullable: true),
                    Remark = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseReturnOder", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseReturnOder_Store_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Store",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchaseReturnOder_Supplier_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Supplier",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseOderItem",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    StoreId = table.Column<Guid>(nullable: false),
                    ShopId = table.Column<Guid>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedTime = table.Column<DateTime>(nullable: false),
                    OperatorName = table.Column<string>(nullable: true),
                    PurchaseOderId = table.Column<int>(nullable: false),
                    ItemId = table.Column<int>(nullable: false),
                    Number = table.Column<decimal>(nullable: false),
                    GiveNumber = table.Column<decimal>(nullable: false),
                    Amount = table.Column<decimal>(nullable: false),
                    PurchasePrice = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseOderItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseOderItem_Item_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Item",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchaseOderItem_PurchaseOder_PurchaseOderId",
                        column: x => x.PurchaseOderId,
                        principalTable: "PurchaseOder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseReceiveOderItem",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    StoreId = table.Column<Guid>(nullable: false),
                    ShopId = table.Column<Guid>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedTime = table.Column<DateTime>(nullable: false),
                    OperatorName = table.Column<string>(nullable: true),
                    PurchaseReceiveOderId = table.Column<int>(nullable: false),
                    ItemId = table.Column<int>(nullable: false),
                    Number = table.Column<decimal>(nullable: false),
                    GiveNumber = table.Column<decimal>(nullable: false),
                    Amount = table.Column<decimal>(nullable: false),
                    PurchasePrice = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseReceiveOderItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseReceiveOderItem_Item_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Item",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchaseReceiveOderItem_PurchaseReceiveOder_PurchaseReceiveO~",
                        column: x => x.PurchaseReceiveOderId,
                        principalTable: "PurchaseReceiveOder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseReturnOderItem",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    StoreId = table.Column<Guid>(nullable: false),
                    ShopId = table.Column<Guid>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedTime = table.Column<DateTime>(nullable: false),
                    OperatorName = table.Column<string>(nullable: true),
                    PurchaseReturnOderId = table.Column<int>(nullable: false),
                    ItemId = table.Column<int>(nullable: false),
                    Number = table.Column<decimal>(nullable: false),
                    GiveNumber = table.Column<decimal>(nullable: false),
                    Amount = table.Column<decimal>(nullable: false),
                    PurchasePrice = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseReturnOderItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseReturnOderItem_Item_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Item",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchaseReturnOderItem_PurchaseReturnOder_PurchaseReturnOder~",
                        column: x => x.PurchaseReturnOderId,
                        principalTable: "PurchaseReturnOder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Vip_ShopId_Code",
                table: "Vip",
                columns: new[] { "ShopId", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Unit_ShopId_Code",
                table: "Unit",
                columns: new[] { "ShopId", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Supplier_ShopId_Code",
                table: "Supplier",
                columns: new[] { "ShopId", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Store_ShopId_Name",
                table: "Store",
                columns: new[] { "ShopId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Shop_ShopId_StoreId",
                table: "Shop",
                columns: new[] { "ShopId", "StoreId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categorie_ShopId_Code",
                table: "Categorie",
                columns: new[] { "ShopId", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOder_StoreId",
                table: "PurchaseOder",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOder_SupplierId",
                table: "PurchaseOder",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOder_ShopId_OderNo",
                table: "PurchaseOder",
                columns: new[] { "ShopId", "OderNo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOderItem_ItemId",
                table: "PurchaseOderItem",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOderItem_PurchaseOderId",
                table: "PurchaseOderItem",
                column: "PurchaseOderId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseReceiveOder_StoreId",
                table: "PurchaseReceiveOder",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseReceiveOder_SupplierId",
                table: "PurchaseReceiveOder",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseReceiveOder_ShopId_OderNo",
                table: "PurchaseReceiveOder",
                columns: new[] { "ShopId", "OderNo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseReceiveOderItem_ItemId",
                table: "PurchaseReceiveOderItem",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseReceiveOderItem_PurchaseReceiveOderId",
                table: "PurchaseReceiveOderItem",
                column: "PurchaseReceiveOderId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseReturnOder_StoreId",
                table: "PurchaseReturnOder",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseReturnOder_SupplierId",
                table: "PurchaseReturnOder",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseReturnOder_ShopId_OderNo",
                table: "PurchaseReturnOder",
                columns: new[] { "ShopId", "OderNo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseReturnOderItem_ItemId",
                table: "PurchaseReturnOderItem",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseReturnOderItem_PurchaseReturnOderId",
                table: "PurchaseReturnOderItem",
                column: "PurchaseReturnOderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PurchaseOderItem");

            migrationBuilder.DropTable(
                name: "PurchaseReceiveOderItem");

            migrationBuilder.DropTable(
                name: "PurchaseReturnOderItem");

            migrationBuilder.DropTable(
                name: "PurchaseOder");

            migrationBuilder.DropTable(
                name: "PurchaseReceiveOder");

            migrationBuilder.DropTable(
                name: "PurchaseReturnOder");

            migrationBuilder.DropIndex(
                name: "IX_Vip_ShopId_Code",
                table: "Vip");

            migrationBuilder.DropIndex(
                name: "IX_Unit_ShopId_Code",
                table: "Unit");

            migrationBuilder.DropIndex(
                name: "IX_Supplier_ShopId_Code",
                table: "Supplier");

            migrationBuilder.DropIndex(
                name: "IX_Store_ShopId_Name",
                table: "Store");

            migrationBuilder.DropIndex(
                name: "IX_Shop_ShopId_StoreId",
                table: "Shop");

            migrationBuilder.DropIndex(
                name: "IX_Categorie_ShopId_Code",
                table: "Categorie");

            migrationBuilder.DropColumn(
                name: "StoreId",
                table: "Shop");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Store",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vip_Code",
                table: "Vip",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Unit_Code",
                table: "Unit",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Supplier_Code",
                table: "Supplier",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Store_StoreId",
                table: "Store",
                column: "StoreId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Shop_ShopId",
                table: "Shop",
                column: "ShopId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categorie_Code",
                table: "Categorie",
                column: "Code",
                unique: true);
        }
    }
}
