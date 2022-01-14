using MiniShop.Dto;
using MiniShop.Model;
using System;
using System.Threading.Tasks;
using Orm.Core.Result;

namespace MiniShop.IServices
{
    public interface IStoreService : IBaseService<Store, StoreDto, Guid>
    {
        Task<IResultModel> QueryByStoreIdAsync(Guid storeId);

        Task<IResultModel> GetPageUsersByShopId(int pageIndex, int pageSize, Guid shopId);

        Task<IResultModel> GetPageUsersByShopIdAndWhereQueryAsync(int pageIndex, int pageSize, Guid shopId, string name, string contacts);

        Task<IResultModel> GetByShopIdAndNameAsync(Guid shopId, string name);
    }

    public interface ICreateStoreService : IBaseService<Store, StoreCreateDto, Guid>
    {

    }

    public interface IUpdateStoreService : IBaseService<Store, StoreUpdateDto, Guid>
    {

    }
}
