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
    public class SupplierService : BaseService<Supplier, SupplierDto, int>, ISupplierService, IDependency
    {
        public SupplierService(Lazy<IMapper> mapper, IUnitOfWork unitOfWork, ILogger<SupplierService> logger,
            Lazy<IRepository<Supplier>> _repository) : base(mapper, unitOfWork, logger, _repository)
        {

        }

        public async Task<IResultModel> GetMaxCodeByShopIdAsync(Guid shopId)
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
            var list = await data.ProjectTo<SupplierDto>(_mapper.Value.ConfigurationProvider).ToPagedListAsync(pageIndex, pageSize);
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
            var list = await data.ProjectTo<SupplierDto>(_mapper.Value.ConfigurationProvider).ToPagedListAsync(pageIndex, pageSize);
            return ResultModel.Success(list);
        }
    }

    public class CreateSupplierService : BaseService<Supplier, SupplierCreateDto, int>, ICreateSupplierService, IDependency
    {
        public CreateSupplierService(Lazy<IMapper> mapper, IUnitOfWork unitOfWork, ILogger<CreateSupplierService> logger,
            Lazy<IRepository<Supplier>> repository) : base(mapper, unitOfWork, logger, repository)
        {

        }
    }


    public class UpdateSupplierService : BaseService<Supplier, SupplierUpdateDto, int>, IUpdateSupplierService, IDependency
    {
        public UpdateSupplierService(Lazy<IMapper> mapper, IUnitOfWork unitOfWork, ILogger<UpdateSupplierService> logger, Lazy<IRepository<Supplier>> repository)
        : base(mapper, unitOfWork, logger, repository)
        {

        }
    }
}
