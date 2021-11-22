using MiniShop.Api.Database;
using MiniShop.Model;

namespace MiniShop.Api.Services
{
    public class ShopService : BaseService<Shop>, IShopService
    {
        public ShopService(AppDbContext context)
        {
            _context = context;
        }

        public void CreateShopDefaultInfo(Shop shop)
        {
            _context.Set<Shop>().Add(shop);
        }
    }
}
