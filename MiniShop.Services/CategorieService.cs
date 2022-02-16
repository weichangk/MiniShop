using AutoMapper;
using Microsoft.Extensions.Logging;
using MiniShop.Dto;
using MiniShop.IServices;
using MiniShop.Model;
using System;
using Orm.Core;
using System.Threading.Tasks;
using Orm.Core.Result;
using AutoMapper.QueryableExtensions;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace MiniShop.Services
{
    public class CategorieService : BaseService<Categorie, CategorieDto, int>, ICategorieService, IDependency
    {
        public CategorieService(Lazy<IMapper> mapper, IUnitOfWork unitOfWork, ILogger<CategorieService> logger,
            Lazy<IRepository<Categorie>> _repository) : base(mapper, unitOfWork, logger, _repository)
        {

        }

        public async Task<IResultModel> GetByCodeOnShopAsync(Guid shopId, int code)
        {
            var data = _repository.Value.TableNoTracking.Where(s => s.ShopId == shopId && s.Code == code);
            var categorieDto = await data.ProjectTo<CategorieDto>(_mapper.Value.ConfigurationProvider).FirstOrDefaultAsync();
            return ResultModel.Success(categorieDto);
        }

        public async Task<IResultModel> GetMaxCodeByLevelOnShop(Guid shopId, int level)
        {
            var data = _repository.Value.TableNoTracking.Where(s => s.ShopId == shopId && s.Level == level);
            var list = await data.ToListAsync();
            var maxCode = list == null || list.Count == 0 ? 0 : list.Max(s => s.Code) + 1;
            return ResultModel.Success(maxCode);
        }

        public async Task<IResultModel> GetPageByShopIdAsync(int pageIndex, int pageSize, Guid shopId)
        {
            var data = _repository.Value.TableNoTracking;
            data = data.Where(s => s.ShopId == shopId);
            var list = await data.ProjectTo<CategorieDto>(_mapper.Value.ConfigurationProvider).ToPagedListAsync(pageIndex, pageSize);
            return ResultModel.Success(list);
        }

        public async Task<IResultModel> GetPageByShopIdWhereQueryAsync(int pageIndex, int pageSize , Guid shopId, string code, string name)
        {
            var data = _repository.Value.TableNoTracking;
            data = data.Where(s => s.ShopId == shopId);

            code = System.Web.HttpUtility.UrlDecode(code);
            if (!string.IsNullOrEmpty(code))
            {
                data = data.Where(s => s.Code.ToString().Contains(code));
            }
            name = System.Web.HttpUtility.UrlDecode(name);
            if (!string.IsNullOrEmpty(name))
            {
                data = data.Where(s => s.Name != null && s.Name.Contains(name));
            }
            var list = await data.ProjectTo<CategorieDto>(_mapper.Value.ConfigurationProvider).ToPagedListAsync(pageIndex, pageSize);
            return ResultModel.Success(list);
        }
    }

    public class CreateCategorieService : BaseService<Categorie, CategorieCreateDto, int>, ICreateCategorieService, IDependency
    {
        public CreateCategorieService(Lazy<IMapper> mapper, IUnitOfWork unitOfWork, ILogger<CreateCategorieService> logger,
            Lazy<IRepository<Categorie>> repository) : base(mapper, unitOfWork, logger, repository)
        {

        }
    }


    public class UpdateCategorieService : BaseService<Categorie, CategorieUpdateDto, int>, IUpdateCategorieService, IDependency
    {
        public UpdateCategorieService(Lazy<IMapper> mapper, IUnitOfWork unitOfWork, ILogger<UpdateCategorieService> logger, Lazy<IRepository<Categorie>> repository)
        : base(mapper, unitOfWork, logger, repository)
        {

        }
    }
}
