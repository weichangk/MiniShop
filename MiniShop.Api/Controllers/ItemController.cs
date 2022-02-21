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
    [Description("商品信息")]
    public class ItemController : ControllerAbstract
    {
        private readonly Lazy<IItemService> _itemService;
        private readonly Lazy<ICreateItemService> _createItemService;
        private readonly Lazy<IUpdateItemService> _updateItemService;
        public ItemController(ILogger<ItemController> logger, Lazy<IMapper> mapper,
            Lazy<IItemService> itemService,
            Lazy<ICreateItemService> createItemService,
            Lazy<IUpdateItemService> updateItemService) : base(logger, mapper)
        {
            _itemService = itemService;
            _createItemService = createItemService;
            _updateItemService = updateItemService;
        }

        [Description("根据商品ID查询商品")]
        [OperationId("根据商品ID查询商品")]
        [ResponseCache(Duration = 0)]
        [Parameters(name = "id", param = "商品ID")]
        [HttpGet]
        public async Task<IResultModel> GetById([Required] int id)
        {
            _logger.LogDebug($"根据商品ID：{id} 查询商品");
            return await _itemService.Value.GetByIdAsync(id);
        }

        [Description("根据商品编码查询商品")]
        [OperationId("根据商品编码查询商品")]
        [ResponseCache(Duration = 0)]
        [Parameters(name = "shopId", param = "商店ID")]
        [Parameters(name = "code", param = "商品编码")]
        [HttpGet("GetByCodeOnShop")]
        public async Task<IResultModel> GetByCodeOnShop([Required] Guid shopId, string code)
        {
            _logger.LogDebug($"根据商店ID：{shopId} 商品编码：{code} 查询商品");
            return await _itemService.Value.GetByCodeOnShopAsync(shopId, code);
        }

        [Description("根据分页条件获取商品")]
        [OperationId("获取商品分页列表")]
        [ResponseCache(Duration = 0)]
        [Parameters(name = "pageIndex", param = "索引页")]
        [Parameters(name = "pageSize", param = "单页条数")]
        [Parameters(name = "shopId", param = "商店ID")]
        [HttpGet("GetPageOnShop")]
        public async Task<IResultModel> GetPageOnShop([Required] int pageIndex, int pageSize, Guid shopId)
        {
            _logger.LogDebug($"根据商店ID：{shopId} 分页条件：索引页{pageIndex} 单页条数{pageSize} 获取商品");
            return await _itemService.Value.GetPageByShopIdAsync(pageIndex, pageSize, shopId);
        }

        [Description("根据商店ID、分页条件、查询条件获取商品")]
        [OperationId("按条件获取商品分页列表")]
        [ResponseCache(Duration = 0)]
        [Parameters(name = "pageIndex", param = "索引页")]
        [Parameters(name = "pageSize", param = "单页条数")]
        [Parameters(name = "shopId", param = "商店ID")]
        [Parameters(name = "code", param = "商品编码")]
        [Parameters(name = "name", param = "商品名称")]
        [HttpGet("GetPageOnShopWhereQuery")]
        public async Task<IResultModel> GetPageOnShopWhereQuery([Required] int pageIndex, int pageSize, Guid shopId, string code, string name)
        {
            _logger.LogDebug($"根据商店ID：{shopId} 分页条件：索引页 {pageIndex} 单页条数 {pageSize} 查询条件：商品编码 {code} 商品名称 {name} 获取商品");
            return await _itemService.Value.GetPageByShopIdWhereQueryAsync(pageIndex, pageSize, shopId, code, name);
        }

        [Description("通过指定商品ID删除商品")]
        [OperationId("删除商品")]
        [Parameters(name = "id", param = "商品ID")]
        [HttpDelete]
        [Authorize(Roles = "ShopManager, ShopAssistant")]
        public async Task<IResultModel> Delete([Required] int id)
        {
            _logger.LogDebug("删除商品");
            return await _itemService.Value.RemoveAsync(id);
        }

        [Description("通过指定商品ID集合批量删除商品")]
        [OperationId("批量删除商品")]
        [Parameters(name = "ids", param = "商品ID集合")]
        [HttpDelete("BatchDelete")]
        [Authorize(Roles = "ShopManager, ShopAssistant")]
        public async Task<IResultModel> BatchDelete([FromBody] List<int> ids)
        {
            _logger.LogDebug("批量删除商品");
            return await _itemService.Value.RemoveAsync(ids);
        }

        [Description("添加商品，成功后返回当前商品信息")]
        [OperationId("添加商品")]
        [HttpPost]
        [Authorize(Roles = "ShopManager, ShopAssistant")]
        public async Task<IResultModel> Add([FromBody] ItemCreateDto model)
        {
            _logger.LogDebug("添加商品");
            return await _createItemService.Value.InsertAsync(model);
        }

        [Description("Put修改商品，成功返回商品信息")]
        [OperationId("修改商品")]
        [HttpPut]
        [Authorize(Roles = "ShopManager, ShopAssistant")]
        public async Task<IResultModel> Update([FromBody] ItemUpdateDto model)
        {
            _logger.LogDebug("修改商品");
            return await _updateItemService.Value.UpdateAsync(model);
        }

        [Description("Patch使用修改商品，成功返回商品信息")]
        [OperationId("修改商品")]
        [HttpPatch]
        [Authorize(Roles = "ShopManager, ShopAssistant")]
        public async Task<IResultModel> PatchUpdate([FromRoute] int id, [FromBody] JsonPatchDocument<ItemUpdateDto> patchDocument)
        {
            _logger.LogDebug("使用JsonPatch修改商品");
            return await _updateItemService.Value.PatchAsync(id, patchDocument);
        }

    }
}
