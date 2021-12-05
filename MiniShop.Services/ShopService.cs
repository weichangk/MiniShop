using MiniShop.IServices;
using MiniShop.Model;
using MiniShop.Orm;

namespace MiniShop.Services
{
    public class ShopService : BaseService<Shop>, IShopService
    {
        public ShopService(AppDbContext context)
        {
            _context = context;
        }
    }
}
