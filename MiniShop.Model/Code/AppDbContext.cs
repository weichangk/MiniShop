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
            modelBuilder.Entity<Vip>()
                .HasIndex(a => a.Code)
                .IsUnique();
            modelBuilder.Entity<Categorie>()
                .HasIndex(a => a.Name)
                .IsUnique();
            modelBuilder.Entity<Unit>()
                .HasIndex(a => a.Name)
                .IsUnique();



            //modelBuilder.Seed();
        }
    }
}
