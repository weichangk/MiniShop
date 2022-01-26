﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MiniShopAdmin.Model.Code;

namespace MiniShopAdmin.Api.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20220126023952_UpdateRenewPackage")]
    partial class UpdateRenewPackage
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("MiniShopAdmin.Model.RenewPackage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Image")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<DateTime>("ModifiedTime")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("Months")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<string>("OperatorName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(65,30)");

                    b.Property<string>("Remark")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("RenewPackage");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CreatedTime = new DateTime(2022, 1, 26, 10, 39, 50, 989, DateTimeKind.Local).AddTicks(2558),
                            Image = "https://gw.alipayobjects.com/zos/rmsportal/gLaIAoVWTtLbBWZNYEMg.png",
                            ModifiedTime = new DateTime(2022, 1, 26, 10, 39, 50, 992, DateTimeKind.Local).AddTicks(3074),
                            Months = 6,
                            Name = "半年",
                            Price = 299m,
                            Remark = "希望是一个好东西，也许是最好的，好东西是不会消亡的"
                        },
                        new
                        {
                            Id = 2,
                            CreatedTime = new DateTime(2022, 1, 26, 10, 39, 50, 994, DateTimeKind.Local).AddTicks(4229),
                            Image = "https://gw.alipayobjects.com/zos/rmsportal/iXjVmWVHbCJAyqvDxdtx.png",
                            ModifiedTime = new DateTime(2022, 1, 26, 10, 39, 50, 994, DateTimeKind.Local).AddTicks(4249),
                            Months = 12,
                            Name = "一年",
                            Price = 499m,
                            Remark = "希望是一个好东西，也许是最好的，好东西是不会消亡的"
                        },
                        new
                        {
                            Id = 3,
                            CreatedTime = new DateTime(2022, 1, 26, 10, 39, 50, 994, DateTimeKind.Local).AddTicks(7038),
                            Image = "https://gw.alipayobjects.com/zos/rmsportal/iZBVOIhGJiAnhplqjvZW.png",
                            ModifiedTime = new DateTime(2022, 1, 26, 10, 39, 50, 994, DateTimeKind.Local).AddTicks(7046),
                            Months = 24,
                            Name = "两年",
                            Price = 799m,
                            Remark = "希望是一个好东西，也许是最好的，好东西是不会消亡的"
                        },
                        new
                        {
                            Id = 4,
                            CreatedTime = new DateTime(2022, 1, 26, 10, 39, 50, 994, DateTimeKind.Local).AddTicks(9333),
                            Image = "https://gw.alipayobjects.com/zos/rmsportal/uMfMFlvUuceEyPpotzlq.png",
                            ModifiedTime = new DateTime(2022, 1, 26, 10, 39, 50, 994, DateTimeKind.Local).AddTicks(9339),
                            Months = 36,
                            Name = "三年",
                            Price = 1099m,
                            Remark = "希望是一个好东西，也许是最好的，好东西是不会消亡的"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
