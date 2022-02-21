using MiniShop.Dto;
using MiniShop.Model;
using Orm.Core.Result;
using System;
using System.Threading.Tasks;

namespace MiniShop.IServices
{
    public interface IItemService : IBaseService<Item, ItemDto, int>
    {
        Task<IResultModel> GetByCodeOnShopAsync(Guid shopId, string code);

        Task<IResultModel> GetPageByShopIdAsync(int pageIndex, int pageSize, Guid shopId);

        Task<IResultModel> GetPageByShopIdWhereQueryAsync(int pageIndex, int pageSize, Guid shopId, string code, string name);
    }

    public interface ICreateItemService : IBaseService<Item, ItemCreateDto, int>
    {

    }

    public interface IUpdateItemService : IBaseService<Item, ItemUpdateDto, int>
    {

    }
}
