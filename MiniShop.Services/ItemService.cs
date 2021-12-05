using MiniShop.IServices;
using MiniShop.Model;
using MiniShop.Orm;

namespace MiniShop.Services
{
    public class ItemService : BaseService<Item>, IItemService
    {
        public ItemService(AppDbContext context)
        {
            _context = context;
        }
    }
}
