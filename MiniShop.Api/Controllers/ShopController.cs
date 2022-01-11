using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MiniShop.Dto;
using MiniShop.IServices;
using MiniShop.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Orm.Core.Result;

namespace MiniShop.Api.Controllers
{
    /// <summary>
    /// 商店信息控制器
    /// </summary>
    [Description("商店信息")]
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ShopController : ControllerAbstract
    {
        private readonly Lazy<IShopService> _shopService;

        /// <summary>
        /// 商店信息控制器
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="shopService"></param>
        public ShopController(ILogger<ShopController> logger, Lazy<IShopService> shopService) : base(logger)
        {
            _shopService = shopService;
        }

        /// <summary>
        /// 根据商店ID获取商店
        /// </summary>
        /// <param name="shopId"></param>
        /// <returns></returns>
        [Description("根据商店ID获取商店")]
        [OperationId("获取商店")]
        [ResponseCache(Duration = 0)]
        [Parameters(name ="shopId", param = "商店ID")]
        [HttpGet("{shopId}")]
        public async Task<IResultModel> Query([Required] Guid shopId)
        {
            _logger.LogDebug($"根据商店ID {shopId }获取商店");
            return await _shopService.Value.GetByIdAsync(shopId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
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
