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
using Microsoft.AspNetCore.JsonPatch;
using System.Net;
using MiniShop.Model.Enums;

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

        [Description("根据商店ID获取商店")]
        [OperationId("根据商店ID获取商店")]
        [ResponseCache(Duration = 0)]
        [Parameters(name = "id", param = "商店ID")]
        [HttpGet]
        public async Task<IResultModel> GetById([Required] int id)
        {
            _logger.LogDebug($"根据商店ID：{id } 获取商店");
            return await _shopService.Value.GetByIdAsync(id);
        }

        [Description("根据ShopId获取商店")]
        [OperationId("根据ShopId获取商店")]
        [ResponseCache(Duration = 0)]
        [Parameters(name = "shopId", param = "ShopId")]
        [HttpGet("GetByShopId")]
        public async Task<IResultModel> GetByShopId([Required] Guid shopId)
        {
            _logger.LogDebug($"根据ShopId：{shopId } 获取商店");
            return await _shopService.Value.GetByShopIdAsync(shopId);
        }

        [Description("新增商店，成功返回商店信息")]
        [OperationId("新增商店")]
        [Authorize(Roles = nameof(EnumRole.ShopManager))]
        [HttpPost]
        public async Task<IResultModel> Add([FromBody] ShopCreateDto model)
        {
            _logger.LogDebug("新增商店");
            if (ModelState.IsValid)
            {
                return await _shopCreateService.Value.InsertAsync(model);
            }
            return ResultModel.Failed(ModelStateErrorMessage(ModelState), (int)HttpStatusCode.BadRequest);
        }

        [Description("Put修改商店，成功返回商店信息")]
        [OperationId("Put修改商店")]
        [Authorize(Roles = nameof(EnumRole.ShopManager))]
        [HttpPut]
        public async Task<IResultModel> Update([FromBody] ShopUpdateDto model)
        {
            _logger.LogDebug("Put修改商店");
            if (ModelState.IsValid)
            {
                return await _shopUpdateService.Value.UpdateAsync(model);
            }
            return ResultModel.Failed(ModelStateErrorMessage(ModelState), (int)HttpStatusCode.BadRequest);
        }

        [Description("Patch修改商店，成功返回商店信息")]
        [OperationId("Patch修改商店")]
        [Parameters(name = "id", param = "商店ID")]
        [Authorize(Roles = nameof(EnumRole.ShopManager))]
        [HttpPatch]
        public async Task<IResultModel> PatchUpdateById([Required] int id, [FromBody] JsonPatchDocument<ShopUpdateDto> patchDocument)
        {
            _logger.LogDebug("Patch修改商店");
            if (ModelState.IsValid)
            {
                return await _shopUpdateService.Value.PatchAsync(id, patchDocument);
            }
            return ResultModel.Failed(ModelStateErrorMessage(ModelState), (int)HttpStatusCode.BadRequest);
        }

    }
}
