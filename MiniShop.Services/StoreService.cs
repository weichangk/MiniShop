using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MiniShop.Dto;
using MiniShop.IServices;
using MiniShop.Model;
using System;
using System.Linq;
using System.Threading.Tasks;
using Orm.Core;
using Orm.Core.Result;

namespace MiniShop.Services
{
    public class StoreService : BaseService<Store, StoreDto, int>, IStoreService, IDependency
    {
        public StoreService(Lazy<IMapper> mapper, IUnitOfWork unitOfWork, ILogger<StoreService> logger,
            Lazy<IRepository<Store>> _repository) : base(mapper, unitOfWork, logger, _repository)
        {

        }

        public async Task<IResultModel> GetByStoreIdAsync(Guid storeId)
        {
            var data = _repository.Value.TableNoTracking.Where(s => s.StoreId == storeId);
            var storeDto = await data.ProjectTo<StoreDto>(_mapper.Value.ConfigurationProvider).FirstOrDefaultAsync();
            return ResultModel.Success(storeDto);
        }

        public async Task<IResultModel> GetPageByShopIdAsync(int pageIndex, int pageSize, Guid shopId)
        {
            var data = _repository.Value.TableNoTracking;
            data = data.Where(s => s.ShopId == shopId);
            var list = await data.ProjectTo<StoreDto>(_mapper.Value.ConfigurationProvider).ToPagedListAsync(pageIndex, pageSize);
            return ResultModel.Success(list);
        }

        public async Task<IResultModel> GetPageByShopIdAndWhereQueryAsync(int pageIndex, int pageSize, Guid shopId, string name, string contacts)
        {
            var data = _repository.Value.TableNoTracking;
            data = data.Where(s => s.ShopId == shopId);
            if (!string.IsNullOrEmpty(name))
            {
                data = data.Where(s => s.Name.Contains(name));
            }
            if (!string.IsNullOrEmpty(contacts))
            {
                data = data.Where(s => s.Contacts.Contains(contacts));
            }

            var list = await data.ProjectTo<StoreDto>(_mapper.Value.ConfigurationProvider).ToPagedListAsync(pageIndex, pageSize);
            return ResultModel.Success(list);
        }
    }

    public class CreateStoreService : BaseService<Store, StoreCreateDto, int>, ICreateStoreService, IDependency
    {
        public CreateStoreService(Lazy<IMapper> mapper, IUnitOfWork unitOfWork, ILogger<CreateStoreService> logger,
            Lazy<IRepository<Store>> repository) : base(mapper, unitOfWork, logger, repository)
        {

        }
    }


    public class UpdateStoreService : BaseService<Store, StoreUpdateDto, int>, IUpdateStoreService, IDependency
    {
        public UpdateStoreService(Lazy<IMapper> mapper, IUnitOfWork unitOfWork, ILogger<UpdateStoreService> logger, Lazy<IRepository<Store>> repository)
        : base(mapper, unitOfWork, logger, repository)
        {

        }
    }
}
