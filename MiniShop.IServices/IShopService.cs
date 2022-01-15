using MiniShop.Dto;
using MiniShop.Model;
using Orm.Core.Result;
using System;
using System.Threading.Tasks;

namespace MiniShop.IServices
{
    public interface IShopService : IBaseService<Shop, ShopDto, int>
    {
        Task<IResultModel> QueryByShopIdAsync(Guid shopId);
    }

    public interface IShopCreateService : IBaseService<Shop, ShopCreateDto, int>
    { 

    }

    public interface IShopUpdateService : IBaseService<Shop, ShopUpdateDto, int>
    {

    }
}
