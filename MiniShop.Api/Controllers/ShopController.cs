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
        private readonly Lazy<IShopCreateService> _shopCreateService;
        private readonly Lazy<IShopUpdateService> _shopUpdateService;

        public ShopController(ILogger<ShopController> logger, Lazy<IMapper> mapper,
            Lazy<IShopService> shopService,
            Lazy<IShopCreateService> shopCreateService,
            Lazy<IShopUpdateService> shopUpdateService) : base(logger, mapper)
        {
            _shopService = shopService;
            _shopCreateService = shopCreateService;
            _shopUpdateService = shopUpdateService;
        }

        [Description("根据商店ID查询商店")]
        [OperationId("查询商店")]
        [ResponseCache(Duration = 0)]
        [Parameters(name = "id", param = "商店ID")]
        [HttpGet("{id}")]
        public async Task<IResultModel> Query([Required] int id)
        {
            _logger.LogDebug($"根据商店ID：{id } 查询商店");
            return await _shopService.Value.GetByIdAsync(id);
        }

        [Description("根据ShopId查询商店")]
        [OperationId("查询商店")]
        [ResponseCache(Duration = 0)]
        [Parameters(name = "shopId", param = "ShopId")]
        [HttpGet("GetByShopId/{shopId}")]
        public async Task<IResultModel> Query([Required] Guid shopId)
        {
            _logger.LogDebug($"根据ShopId：{shopId } 查询商店");
            return await _shopService.Value.QueryByShopIdAsync(shopId);
        }

        [Description("添加商店，成功返回商店信息")]
        [OperationId("添加商店")]
        [HttpPost]
        public async Task<IResultModel> Add([FromBody] ShopCreateDto model)
        {
            _logger.LogDebug("新增商店");
            return await _shopCreateService.Value.InsertAsync(model);
        }

        [Description("修改商店，成功返回商店信息")]
        [OperationId("修改商店")]
        [HttpPut]
        public async Task<IResultModel> Update([FromBody] ShopUpdateDto model)
        {
            _logger.LogDebug("修改商店");
            return await _shopUpdateService.Value.UpdateAsync(model);
        }
    }
}
