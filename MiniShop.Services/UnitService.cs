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
    public class UnitService : BaseService<Unit, UnitDto, int>, IUnitService, IDependency
    {
        public UnitService(Lazy<IMapper> mapper, IUnitOfWork unitOfWork, ILogger<UnitService> logger,
            Lazy<IRepository<Unit>> _repository) : base(mapper, unitOfWork, logger, _repository)
        {

        }

        public async Task<IResultModel> GetMaxCodeByShopId(Guid shopId)
        {
            var data = _repository.Value.TableNoTracking.Where(s => s.ShopId == shopId);
            var list = await data.ToListAsync();
            var maxCode = list == null || list.Count == 0 ? 0 : list.Max(s => s.Code) + 1;
            return ResultModel.Success(maxCode);
        }

        public async Task<IResultModel> GetPageByShopIdAsync(int pageIndex, int pageSize, Guid shopId)
        {
            var data = _repository.Value.TableNoTracking;
            data = data.Where(s => s.ShopId == shopId);
            var list = await data.ProjectTo<UnitDto>(_mapper.Value.ConfigurationProvider).ToPagedListAsync(pageIndex, pageSize);
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
            var list = await data.ProjectTo<UnitDto>(_mapper.Value.ConfigurationProvider).ToPagedListAsync(pageIndex, pageSize);
            return ResultModel.Success(list);
        }
    }

    public class CreateUnitService : BaseService<Unit, UnitCreateDto, int>, ICreateUnitService, IDependency
    {
        public CreateUnitService(Lazy<IMapper> mapper, IUnitOfWork unitOfWork, ILogger<CreateUnitService> logger,
            Lazy<IRepository<Unit>> repository) : base(mapper, unitOfWork, logger, repository)
        {

        }
    }


    public class UpdateUnitService : BaseService<Unit, UnitUpdateDto, int>, IUpdateUnitService, IDependency
    {
        public UpdateUnitService(Lazy<IMapper> mapper, IUnitOfWork unitOfWork, ILogger<UpdateUnitService> logger, Lazy<IRepository<Unit>> repository)
        : base(mapper, unitOfWork, logger, repository)
        {

        }
    }
}
