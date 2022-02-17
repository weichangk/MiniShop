using Microsoft.EntityFrameworkCore;

namespace MiniShop.Model.Code
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Categorie>().HasData(InitializationData.Initialization.Categorie);
            //modelBuilder.Entity<Unit>().HasData(InitializationData.Initialization.Unit);
        }
    }
}
