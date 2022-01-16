using MiniShop.Dto;
using MiniShop.Model;
using System;
using System.Threading.Tasks;
using Orm.Core.Result;

namespace MiniShop.IServices
{
    public interface IStoreService : IBaseService<Store, StoreDto, int>
    {
        Task<IResultModel> GetByStoreIdAsync(Guid storeId);

        Task<IResultModel> GetPageByShopIdAsync(int pageIndex, int pageSize, Guid shopId);

        Task<IResultModel> GetPageByShopIdAndWhereQueryAsync(int pageIndex, int pageSize, Guid shopId, string name);
    }

    public interface ICreateStoreService : IBaseService<Store, StoreCreateDto, int>
    {

    }

    public interface IUpdateStoreService : IBaseService<Store, StoreUpdateDto, int>
    {

    }
}
