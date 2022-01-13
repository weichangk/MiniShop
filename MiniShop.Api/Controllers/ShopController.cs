using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MiniShop.Dto;
using MiniShop.IServices;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Orm.Core.Result;

namespace MiniShop.Api.Controllers
{
    [Description("商店信息")]
    public class ShopController : ControllerAbstract
    {
        private readonly Lazy<IShopService> _shopService;

        public ShopController(ILogger<ShopController> logger, Lazy<IMapper> mapper, Lazy<IShopService> shopService) : base(logger, mapper)
        {
            _shopService = shopService;
        }

        [Description("根据商店ID获取商店")]
        [OperationId("获取商店")]
        [ResponseCache(Duration = 0)]
        [Parameters(name = "shopId", param = "商店ID")]
        [HttpGet("{shopId}")]
        public async Task<IResultModel> Query([Required] Guid shopId)
        {
            _logger.LogDebug($"根据商店ID {shopId }获取商店");
            return await _shopService.Value.GetByIdAsync(shopId);
        }

        [Description("修改商店，成功后返回当前商店信息")]
        [OperationId("修改商店")]
        [HttpPut]
        public async Task<IResultModel> Update([FromBody] ShopDto model)
        {
            _logger.LogDebug("修改商店");
            return await _shopService.Value.UpdateAsync(model);
        }
    }
}
