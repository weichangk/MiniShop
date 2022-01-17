using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MiniShop.Dto;
using MiniShop.IServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Orm.Core.Result;
using AutoMapper;

namespace MiniShop.Api.Controllers
{
    [Description("门店信息")]
    public class StoreController : ControllerAbstract
    {
        private readonly Lazy<IStoreService> _storeService;
        private readonly Lazy<ICreateStoreService> _createStoreService;
        private readonly Lazy<IUpdateStoreService> _updateStoreService;

        public StoreController(ILogger<StoreController> logger, Lazy<IMapper> mapper, Lazy<IStoreService> storeService,
            Lazy<ICreateStoreService> createStoreService,
            Lazy<IUpdateStoreService> updateStoreService) : base(logger, mapper)
        {
            _storeService = storeService;
            _updateStoreService = updateStoreService;
            _createStoreService = createStoreService;

        }

        [Description("根据门店ID查询门店")]
        [OperationId("查询门店")]
        [ResponseCache(Duration = 0)]
        [Parameters(name = "id", param = "门店ID")]
        [HttpGet("{id}")]
        public async Task<IResultModel> Query([Required] int id)
        {
            _logger.LogDebug($"根据门店ID：{id} 查询门店");
            return await _storeService.Value.GetByIdAsync(id);
        }

        [Description("根据StoreId查询门店")]
        [OperationId("查询门店")]
        [ResponseCache(Duration = 0)]
        [Parameters(name = "storeId", param = "StoreId")]
        [HttpGet("GetByStoreId/{storeId}")]
        public async Task<IResultModel> QueryByStoreId([Required] Guid storeId)
        {
            _logger.LogDebug($"根据StoreId：{storeId} 查询门店");
            return await _storeService.Value.GetByStoreIdAsync(storeId);
        }

        [Description("根据ShopId查询门店")]
        [OperationId("查询门店")]
        [ResponseCache(Duration = 0)]
        [Parameters(name = "shopId", param = "ShopId")]
        [HttpGet("GetByShopId/{shopId}")]
        public async Task<IResultModel> QueryByShopId([Required] Guid shopId)
        {
            _logger.LogDebug($"根据ShopId：{shopId} 查询门店");
            return await _storeService.Value.GetByShopIdAsync(shopId);
        }

        [Description("根据商店ID和分页条件获取门店")]
        [OperationId("获取门店分页列表")]
        [ResponseCache(Duration = 0)]
        [Parameters(name = "pageIndex", param = "索引页")]
        [Parameters(name = "pageSize", param = "单页条数")]
        [Parameters(name = "shopId", param = "商店ID")]
        [HttpGet("GetPageByShopId/{pageIndex}/{pageSize}/{shopId}")]
        public async Task<IResultModel> Query([Required] int pageIndex, int pageSize, Guid shopId)
        {
            _logger.LogDebug($"根据商店ID：{shopId} 分页条件：索引页{pageIndex} 单页条数{pageSize} 获取门店");
            return await _storeService.Value.GetPageByShopIdAsync(pageIndex, pageSize, shopId);
        }

        [Description("根据商店ID、分页条件、查询条件获取门店")]
        [OperationId("按条件获取用户分页列表")]
        [ResponseCache(Duration = 0)]
        [Parameters(name = "pageIndex", param = "索引页")]
        [Parameters(name = "pageSize", param = "单页条数")]
        [Parameters(name = "shopId", param = "商店ID")]
        [Parameters(name = "name", param = "用户名")]
        [HttpGet("GetPageByShopIdAndWhereQuery/{pageIndex}/{pageSize}/{shopId}/{name}")]
        public async Task<IResultModel> Query([Required] int pageIndex, int pageSize, Guid shopId, string name)
        {
            _logger.LogDebug($"根据商店ID：{shopId} 分页条件：索引页 {pageIndex} 单页条数 {pageSize} 查询条件：门店名称 {name} 获取门店");
            return await _storeService.Value.GetPageByShopIdAndWhereQueryAsync(pageIndex, pageSize, shopId, name);
        }

        [Description("通过指定门店ID删除门店")]
        [OperationId("删除门店")]
        [Parameters(name = "id", param = "门店ID")]
        [HttpDelete("{id}")]
        public async Task<IResultModel> Delete([Required] int id)
        {
            _logger.LogDebug("删除门店");
            return await _storeService.Value.RemoveAsync(id);
        }

        [Description("通过指定门店ID集合批量删除门店")]
        [OperationId("批量删除门店")]
        [Parameters(name = "ids", param = "门店ID集合")]
        [HttpDelete("BatchDelete")]
        public async Task<IResultModel> BatchDelete([FromBody] List<int> ids)
        {
            _logger.LogDebug("批量删除门店");
            return await _storeService.Value.RemoveAsync(ids);
        }

        [Description("添加门店，成功后返回当前门店信息")]
        [OperationId("添加门店")]
        [HttpPost]
        public async Task<IResultModel> Add([FromBody] StoreCreateDto model)
        {
            _logger.LogDebug("添加门店");
            return await _createStoreService.Value.InsertAsync(model);
        }

        [Description("Put修改门店，成功返回门店信息")]
        [OperationId("修改门店")]
        [HttpPut]
        public async Task<IResultModel> Update([FromBody] StoreUpdateDto model)
        {
            _logger.LogDebug("修改门店");
            return await _updateStoreService.Value.UpdateAsync(model);
        }

        [Description("Patch使用修改门店，成功返回门店信息")]
        [OperationId("修改门店")]
        [HttpPatch("{id}")]
        public async Task<IResultModel> PatchUpdate([FromRoute] int id, [FromBody] JsonPatchDocument<StoreUpdateDto> patchDocument)
        {
            _logger.LogDebug("使用JsonPatch修改门店");
            return await _updateStoreService.Value.PatchAsync(id, patchDocument);
        }
    }
}
