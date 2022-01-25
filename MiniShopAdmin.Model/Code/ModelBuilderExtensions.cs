using Microsoft.EntityFrameworkCore;

namespace MiniShopAdmin.Model.Code
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RenewPackage>().HasData(InitializationData.Initialization.RenewPackage);
        }
    }
}
