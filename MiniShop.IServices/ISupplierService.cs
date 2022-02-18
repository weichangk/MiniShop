using MiniShop.Dto;
using MiniShop.Model;
using Orm.Core.Result;
using System;
using System.Threading.Tasks;

namespace MiniShop.IServices
{
    public interface ISupplierService : IBaseService<Supplier, SupplierDto, int>
    {
        Task<IResultModel> GetMaxCodeByShopIdAsync(Guid shopId);

        Task<IResultModel> GetPageByShopIdAsync(int pageIndex, int pageSize, Guid shopId);

        Task<IResultModel> GetPageByShopIdWhereQueryAsync(int pageIndex, int pageSize, Guid shopId, string code, string name);
    }

    public interface ICreateSupplierService : IBaseService<Supplier, SupplierCreateDto, int>
    {

    }

    public interface IUpdateSupplierService : IBaseService<Supplier, SupplierUpdateDto, int>
    {

    }
}
