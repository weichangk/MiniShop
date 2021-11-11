using MiniShop.Api.Database;
using MiniShop.Model;

namespace MiniShop.Api.Services
{
    public class ItemService : BaseService<Item>, IItemService
    {
        public ItemService(AppDbContext context)
        {
            _context = context;
        }
    }
}
