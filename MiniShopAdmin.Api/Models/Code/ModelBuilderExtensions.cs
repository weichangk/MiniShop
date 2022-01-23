using Microsoft.EntityFrameworkCore;

namespace MiniShopAdmin.Api.Models.Code
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<User>().HasData(InitializationData.Initialization.User);
        }
    }
}
