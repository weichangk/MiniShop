using AutoMapper;
using Microsoft.Extensions.Logging;
using MiniShop.Dto;
using MiniShop.IServices;
using MiniShop.Model;
using System;
using yrjw.ORM.Chimp;

namespace MiniShop.Services
{
    public class ShopService : BaseService<Shop, ShopDto, Guid>, IShopService, IDependency
    {
        public ShopService(Lazy<IMapper> mapper, IUnitOfWork unitOfWork, ILogger<ShopService> logger,
            Lazy<IRepository<Shop>> _repository) : base(mapper, unitOfWork, logger, _repository)
        {

        }
    }
}
