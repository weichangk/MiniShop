using Microsoft.EntityFrameworkCore;
using Orm.Core;

namespace MiniShopAdmin.Model.Code
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

            modelBuilder.Entity<RenewPackage>()
                .HasIndex(m => m.Name)
                .IsUnique();

            modelBuilder.Seed();
        }
    }
}
