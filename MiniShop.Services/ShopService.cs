using Microsoft.Extensions.Logging;
using MiniShop.IServices;
using MiniShop.Model;
using MiniShop.Orm;

namespace MiniShop.Services
{
    public class ShopService : BaseService<Shop>, IShopService
    {
        public ShopService(AppDbContext context, ILogger<ShopService> logger) : base(logger)
        {
            _context = context;
        }
    }
}
