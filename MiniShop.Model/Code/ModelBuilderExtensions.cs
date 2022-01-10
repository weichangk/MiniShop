using Microsoft.EntityFrameworkCore;

namespace MiniShop.Model.Code
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<User>().HasData(InitializationData.Initialization.User);
        }
    }
}
