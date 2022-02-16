using MiniShop.Dto;
using MiniShop.Model;
using Orm.Core.Result;
using System;
using System.Threading.Tasks;

namespace MiniShop.IServices
{
    public interface ICategorieService : IBaseService<Categorie, CategorieDto, int>
    {
        Task<IResultModel> GetByCodeOnShopAsync(Guid shopId, int code);

        Task<IResultModel> GetMaxCodeByLevelOnShop(Guid shopId, int level);

        Task<IResultModel> GetPageByShopIdAsync(int pageIndex, int pageSize, Guid shopId);

        Task<IResultModel> GetPageByShopIdWhereQueryAsync(int pageIndex, int pageSize, Guid shopId, string code, string name);
    }

    public interface ICreateCategorieService : IBaseService<Categorie, CategorieCreateDto, int>
    {

    }

    public interface IUpdateCategorieService : IBaseService<Categorie, CategorieUpdateDto, int>
    {

    }
}
