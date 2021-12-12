﻿using Microsoft.EntityFrameworkCore;
using MiniShop.Model;
using MiniShop.Model.Enums;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Reflection;

namespace MiniShop.Orm
{
    public class AppDbContext : DbContext
    {
        public DbSet<Shop> Shops { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Categorie> Categories { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //SeedData(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            Guid shopId = Guid.NewGuid();
            Shop shop = new Shop
            {
                Id = shopId,
                Name = "weick shop",
                Contacts = "weick",
                Phone = "18276743761",
                Email = "18276743761@163.com",
                Address = "shenzhen",
                CreateDate = DateTime.Parse("2021-11-11"),
                ValidDate = DateTime.Parse("2099-11-11"),
            };
            modelBuilder.Entity<Shop>().HasData(shop);

            User user = new User
            {
                Id = 1,
                ShopId = shopId,
                Name = "weick",
                Phone = "18276743761",
                Email = "18276743761@163.com",
                Role = EnumRole.ShopManager,
            };
            modelBuilder.Entity<User>().HasData(user);

            var seedDataJson = File.ReadAllText(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"/SeedData.json");
            SeedDataModel seedDataModel = JsonConvert.DeserializeObject<SeedDataModel>(seedDataJson);
            foreach (var item in seedDataModel.Categories)
            {
                item.ShopId = shopId;
            }
            modelBuilder.Entity<Categorie>().HasData(seedDataModel.Categories);
            foreach (var item in seedDataModel.Items)
            {
                item.ShopId = shopId;
            }
            modelBuilder.Entity<Item>().HasData(seedDataModel.Items);
        }
    }
}