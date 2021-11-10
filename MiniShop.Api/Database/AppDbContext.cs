using Microsoft.EntityFrameworkCore;
using MiniShop.Model;
using Newtonsoft.Json;
using System.IO;
using System.Reflection;

namespace MiniShop.Api.Database
{
    public class AppDbContext : DbContext
    {
        public DbSet<Shop> Shops { get; set; }
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
            var seedDataJson = File.ReadAllText(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"/Database/SeedData.json");
            SeedDataModel seedDataModel = JsonConvert.DeserializeObject<SeedDataModel>(seedDataJson);
            modelBuilder.Entity<Shop>().HasData(seedDataModel.Shops);
            modelBuilder.Entity<Item>().HasData(seedDataModel.Items);
            modelBuilder.Entity<Categorie>().HasData(seedDataModel.Categories);
        }
    }
}
