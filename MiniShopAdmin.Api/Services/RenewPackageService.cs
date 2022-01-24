using AutoMapper;
using Microsoft.Extensions.Logging;
using MiniShopAdmin.Api.Dtos;
using MiniShopAdmin.Api.Models;
using Orm.Core;
using System;

namespace MiniShopAdmin.Api.Services
{
    public class RenewPackageService : BaseService<RenewPackage, RenewPackageDto, int>, IRenewPackageService, IDependency
    {
        public RenewPackageService(Lazy<IMapper> mapper, IUnitOfWork unitOfWork, ILogger<RenewPackageService> logger,
            Lazy<IRepository<RenewPackage>> _repository) : base(mapper, unitOfWork, logger, _repository)
        {

        }
    }

    public class RenewPackageCreateService : BaseService<RenewPackage, RenewPackageCreateDto, int>, IRenewPackageCreateService, IDependency
    {
        public RenewPackageCreateService(Lazy<IMapper> mapper, IUnitOfWork unitOfWork, ILogger<RenewPackageCreateService> logger,
            Lazy<IRepository<RenewPackage>> _repository) : base(mapper, unitOfWork, logger, _repository)
        {

        }
    }

    public class RenewPackageUpdateService : BaseService<RenewPackage, RenewPackageUpdateDto, int>, IRenewPackageUpdateService, IDependency
    {
        public RenewPackageUpdateService(Lazy<IMapper> mapper, IUnitOfWork unitOfWork, ILogger<RenewPackageUpdateService> logger,
            Lazy<IRepository<RenewPackage>> _repository) : base(mapper, unitOfWork, logger, _repository)
        {

        }
    }
}
