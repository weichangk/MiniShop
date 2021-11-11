using Microsoft.EntityFrameworkCore;
using MiniShop.Model;
using MiniShop.Model.Enums;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Reflection;

namespace MiniShop.Api.Database
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
            SeedData(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            Shop shop = new Shop
            {
                Id = 1,
                Name="Alice Shop",
                Contacts="alice",
                Phone="18888888888",
                Email="AliceSmith@shop.com",
                Address="shenzhen",
                CreateDate= DateTime.Parse("2021-11-11"),
                ValidDate = DateTime.Parse("2021-11-11"),
            };
            modelBuilder.Entity<Shop>().HasData(shop);

            User user = new User
            {
                Id = 1,
                ShopId = 1,
                Name = "alice",
                Phone = "18888888888",
                Email = "AliceSmith@shop.com",
                Role = EnumRole.ShopManager,
            };
            modelBuilder.Entity<User>().HasData(user);

            var seedDataJson = File.ReadAllText(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"/Database/SeedData.json");
            SeedDataModel seedDataModel = JsonConvert.DeserializeObject<SeedDataModel>(seedDataJson);
            modelBuilder.Entity<Categorie>().HasData(seedDataModel.Categories);
            modelBuilder.Entity<Item>().HasData(seedDataModel.Items);
        }
    }
}
