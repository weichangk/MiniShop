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
                .HasIndex(m => new { m.ShopId })
                .IsUnique();
            modelBuilder.Entity<Store>()
                .HasIndex(m => new { m.ShopId, m.StoreId })
                .IsUnique();
            modelBuilder.Entity<Store>()
                .HasIndex(m => new { m.ShopId, m.Name })
                .IsUnique();
            modelBuilder.Entity<Vip>()
                .HasIndex(m => new { m.ShopId, m.Code })
                .IsUnique();
            modelBuilder.Entity<Item>()
                .HasIndex(m => new { m.ShopId, m.Code })
                .IsUnique();
            modelBuilder.Entity<Categorie>()
                .HasIndex(m => new { m.ShopId, m.Code })
                .IsUnique();
            modelBuilder.Entity<Unit>()
                .HasIndex(m => new { m.ShopId, m.Code })
                .IsUnique();
            modelBuilder.Entity<Supplier>()
                .HasIndex(m => new { m.ShopId, m.Code })
                .IsUnique();
            modelBuilder.Entity<PurchaseOder>()
                .HasIndex(m => new { m.ShopId, m.OderNo })
                .IsUnique();
            modelBuilder.Entity<PurchaseReceiveOder>()
                .HasIndex(m => new { m.ShopId, m.OderNo })
                .IsUnique();
            modelBuilder.Entity<PurchaseReturnOder>()
                .HasIndex(m => new { m.ShopId, m.OderNo })
                .IsUnique();

            //modelBuilder.Seed();
        }
    }
}
