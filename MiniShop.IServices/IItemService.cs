using MiniShop.Dto;
using MiniShop.Model;

namespace MiniShop.IServices
{
    public interface IItemService : IBaseService<Item, ItemDto, int>
    {
    }
}
