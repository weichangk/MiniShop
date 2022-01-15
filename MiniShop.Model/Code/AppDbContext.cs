using Microsoft.EntityFrameworkCore;
using Orm.Core;

namespace MiniShop.Model.Code
{
    public class AppDbContext : BaseDbContext
    {
        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Shop>()
                .HasIndex(m => m.ShopId)
                .IsUnique();
            modelBuilder.Entity<Store>()
                .HasIndex(m => m.StoreId)
                .IsUnique();
            modelBuilder.Entity<Vip>()
                .HasIndex(m => m.Code)
                .IsUnique();
            modelBuilder.Entity<Categorie>()
                .HasIndex(m => m.Name)
                .IsUnique();
            modelBuilder.Entity<Unit>()
                .HasIndex(m => m.Name)
                .IsUnique();

            //modelBuilder.Seed();
        }
    }
}
