using MiniShop.Dto;
using MiniShop.Model;
using Orm.Core.Result;
using System;
using System.Threading.Tasks;

namespace MiniShop.IServices
{
    public interface IUnitService : IBaseService<Unit, UnitDto, int>
    {
        Task<IResultModel> GetByCodeOnShopAsync(Guid shopId, int code);

        Task<IResultModel> GetMaxCodeByShopId(Guid shopId);

        Task<IResultModel> GetPageByShopIdAsync(int pageIndex, int pageSize, Guid shopId);

        Task<IResultModel> GetPageByShopIdWhereQueryAsync(int pageIndex, int pageSize, Guid shopId, string code, string name);
    }

    public interface ICreateUnitService : IBaseService<Unit, UnitCreateDto, int>
    {

    }

    public interface IUpdateUnitService : IBaseService<Unit, UnitUpdateDto, int>
    {

    }
}
