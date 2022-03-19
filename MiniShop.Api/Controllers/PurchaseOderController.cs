using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MiniShop.Dto;
using MiniShop.IServices;
using Orm.Core.Result;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace MiniShop.Api.Controllers
{
    [Description("采购订单信息")]
    public class PurchaseOderController : ControllerAbstract
    {
        private readonly Lazy<IPurchaseOderService> _purchaseOderService;
        private readonly Lazy<ICreatePurchaseOderService> _createPurchaseOderService;
        private readonly Lazy<IUpdatePurchaseOderService> _updatePurchaseOderService;
        public PurchaseOderController(ILogger<PurchaseOderController> logger, Lazy<IMapper> mapper,
            Lazy<IPurchaseOderService> purchaseOderService,
            Lazy<ICreatePurchaseOderService> createPurchaseOderService,
            Lazy<IUpdatePurchaseOderService> updatePurchaseOderService) : base(logger, mapper)
        {
            _purchaseOderService = purchaseOderService;
            _createPurchaseOderService = createPurchaseOderService;
            _updatePurchaseOderService = updatePurchaseOderService;
        }

        [Description("根据采购订单ID查询采购订单")]
        [OperationId("根据采购订单ID查询采购订单")]
        [ResponseCache(Duration = 0)]
        [Parameters(name = "id", param = "采购订单ID")]
        [HttpGet]
        public async Task<IResultModel> GetById([Required] int id)
        {
            _logger.LogDebug($"根据采购订单ID：{id} 查询采购订单");
            return await _purchaseOderService.Value.GetByIdAsync(id);
        }

        [Description("根据采购订单编码查询采购订单")]
        [OperationId("根据采购订单编码查询采购订单")]
        [ResponseCache(Duration = 0)]
        [Parameters(name = "shopId", param = "商店ID")]
        [Parameters(name = "oderNo", param = "订单编码")]
        [HttpGet("GetByOderNoOnShop")]
        public async Task<IResultModel> GetByOderNoOnShop([Required] Guid shopId, string oderNo)
        {
            _logger.LogDebug($"根据商店ID：{shopId} 采购订单编码：{oderNo} 查询采购订单");
            return await _purchaseOderService.Value.GetByOderNoOnShopAsync(shopId, oderNo);
        }

        [Description("根据分页条件获取采购订单")]
        [OperationId("获取采购订单分页列表")]
        [ResponseCache(Duration = 0)]
        [Parameters(name = "pageIndex", param = "索引页")]
        [Parameters(name = "pageSize", param = "单页条数")]
        [Parameters(name = "shopId", param = "商店ID")]
        [HttpGet("GetPageOnShop")]
        public async Task<IResultModel> GetPageOnShop([Required] int pageIndex, int pageSize, Guid shopId)
        {
            _logger.LogDebug($"根据商店ID：{shopId} 分页条件：索引页{pageIndex} 单页条数{pageSize} 获取采购订单");
            return await _purchaseOderService.Value.GetPageByShopIdAsync(pageIndex, pageSize, shopId);
        }

        [Description("根据商店ID、分页条件、查询条件获取采购订单")]
        [OperationId("按条件获取采购订单分页列表")]
        [ResponseCache(Duration = 0)]
        [Parameters(name = "pageIndex", param = "索引页")]
        [Parameters(name = "pageSize", param = "单页条数")]
        [Parameters(name = "shopId", param = "商店ID")]
        [Parameters(name = "code", param = "采购订单编码")]
        [Parameters(name = "name", param = "采购订单名称")]
        [HttpGet("GetPageOnShopWhereQueryCodeOrName")]
        public async Task<IResultModel> GetPageOnShopWhereQueryCodeOrName([Required] int pageIndex, int pageSize, Guid shopId, int storeId, string oderNo)
        {
            _logger.LogDebug($"根据商店ID：{shopId} 分页条件：索引页 {pageIndex} 单页条数 {pageSize} 查询条件：门店ID {storeId} 采购订单编码 {oderNo} 获取采购订单");
            return await _purchaseOderService.Value.GetPageByShopIdWhereQueryAsync(pageIndex, pageSize, shopId, storeId, oderNo);
        }

        [Description("通过指定采购订单ID删除采购订单")]
        [OperationId("删除采购订单")]
        [Parameters(name = "id", param = "采购订单ID")]
        [HttpDelete]
        [Authorize(Roles = "ShopManager, ShopAssistant")]
        public async Task<IResultModel> Delete([Required] int id)
        {
            _logger.LogDebug("删除采购订单");
            return await _purchaseOderService.Value.RemoveAsync(id);
        }

        [Description("通过指定采购订单ID集合批量删除采购订单")]
        [OperationId("批量删除采购订单")]
        [Parameters(name = "ids", param = "采购订单ID集合")]
        [HttpDelete("BatchDelete")]
        [Authorize(Roles = "ShopManager, ShopAssistant")]
        public async Task<IResultModel> BatchDelete([FromBody] List<int> ids)
        {
            _logger.LogDebug("批量删除采购订单");
            return await _purchaseOderService.Value.RemoveAsync(ids);
        }

        [Description("添加采购订单，成功后返回当前采购订单信息")]
        [OperationId("添加采购订单")]
        [HttpPost]
        [Authorize(Roles = "ShopManager, ShopAssistant")]
        public async Task<IResultModel> Add([FromBody] PurchaseOderCreateDto model)
        {
            _logger.LogDebug("添加采购订单");
            return await _createPurchaseOderService.Value.InsertAsync(model);
        }

        [Description("Put修改采购订单，成功返回采购订单信息")]
        [OperationId("修改采购订单")]
        [HttpPut]
        [Authorize(Roles = "ShopManager, ShopAssistant")]
        public async Task<IResultModel> Update([FromBody] PurchaseOderUpdateDto model)
        {
            _logger.LogDebug("修改采购订单");
            return await _updatePurchaseOderService.Value.UpdateAsync(model);
        }

        [Description("Patch使用修改采购订单，成功返回采购订单信息")]
        [OperationId("修改采购订单")]
        [HttpPatch]
        [Authorize(Roles = "ShopManager, ShopAssistant")]
        public async Task<IResultModel> PatchUpdate([FromRoute] int id, [FromBody] JsonPatchDocument<PurchaseOderUpdateDto> patchDocument)
        {
            _logger.LogDebug("使用JsonPatch修改采购订单");
            return await _updatePurchaseOderService.Value.PatchAsync(id, patchDocument);
        }



    }
}
