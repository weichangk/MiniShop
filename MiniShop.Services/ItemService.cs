using AutoMapper;
using Microsoft.Extensions.Logging;
using MiniShop.Dto;
using MiniShop.IServices;
using MiniShop.Model;
using System;
using Orm.Core;

namespace MiniShop.Services
{
    public class ItemService : BaseService<Item, ItemDto, int>, IItemService, IDependency
    {
        public ItemService(Lazy<IMapper> mapper, IUnitOfWork unitOfWork, ILogger<ItemService> logger,
            Lazy<IRepository<Item>> _repository) : base(mapper, unitOfWork, logger, _repository)
        {

        }
    }
}
