using MiniShop.Dto;
using MiniShop.Model;
using Orm.Core.Result;
using System;
using System.Threading.Tasks;

namespace MiniShop.IServices
{
    public interface IShopService : IBaseService<Shop, ShopDto, Guid>
    {
        Task<IResultModel> QueryByShopIdAsync(Guid shopId);
    }

    public interface IShopCreateService : IBaseService<Shop, ShopCreateDto, Guid>
    { 

    }
}
