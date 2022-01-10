using MiniShop.Dto;
using MiniShop.Model;
using System;
using System.Threading.Tasks;
using Orm.Core.Result;

namespace MiniShop.IServices
{
    public interface IStoreService : IBaseService<Store, StoreDto, Guid>
    {
        /// <summary>
        /// 根据商店ID和分页条件获取门店
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="shopId"></param>
        /// <returns></returns>
        Task<IResultModel> GetPageUsersByShopId(int pageIndex, int pageSize, Guid shopId);

        /// <summary>
        /// 根据商店ID、分页条件、查询条件获取门店
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="shopId"></param>
        /// <param name="name"></param>
        /// <param name="contacts"></param>
        /// <returns></returns>
        Task<IResultModel> GetPageUsersByShopIdAndWhereQueryAsync(int pageIndex, int pageSize, Guid shopId, string name, string contacts);

        /// <summary>
        /// 根据商店ID和门店名获取门店
        /// </summary>
        /// <param name="shopId"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<IResultModel> GetByShopIdAndNameAsync(Guid shopId, string name);
    }

    public interface ICreateStoreService : IBaseService<Store, StoreCreateDto, Guid>
    {

    }

    public interface IUpdateStoreService : IBaseService<Store, StoreUpdateDto, Guid>
    {

    }
}
