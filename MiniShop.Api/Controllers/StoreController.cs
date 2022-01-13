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
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
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

        [Description("根据商店ID和分页条件获取门店")]
        [OperationId("获取门店分页列表")]
        [ResponseCache(Duration = 0)]
        [Parameters(name = "pageIndex", param = "索引页")]
        [Parameters(name = "pageSize", param = "单页条数")]
        [Parameters(name = "shopId", param = "商店ID")]
        [HttpGet("{pageIndex}/{pageSize}/{shopId}")]
        public async Task<IResultModel> Query([Required] int pageIndex, int pageSize, Guid shopId)
        {
            _logger.LogDebug($"根据商店ID:{shopId} 分页条件:pageIndex{pageIndex} pageSize{pageSize} 获取门店");
            return await _storeService.Value.GetPageUsersByShopId(pageIndex, pageSize, shopId);
        }

        [Description("根据商店ID、分页条件、查询条件获取门店")]
        [OperationId("按条件获取用户分页列表")]
        [ResponseCache(Duration = 0)]
        [Parameters(name = "pageIndex", param = "索引页")]
        [Parameters(name = "pageSize", param = "单页条数")]
        [Parameters(name = "shopId", param = "商店ID")]
        [Parameters(name = "name", param = "用户名")]
        [Parameters(name = "contacts", param = "联系人")]
        [HttpGet("GetPageAndWhereQuery/{pageIndex}/{pageSize}/{shopId}/{name}/{phone}/{role}")]
        public async Task<IResultModel> Query([Required] int pageIndex, int pageSize, Guid shopId, string name, string contacts)
        {
            _logger.LogDebug($"根据商店ID:{shopId} 分页条件:pageIndex {pageIndex} pageSize {pageSize} 查询条件:name {name} phone {contacts}获取门店");
            return await _storeService.Value.GetPageUsersByShopIdAndWhereQueryAsync(pageIndex, pageSize, shopId, name, contacts);
        }

        [Description("根据用户ID获取门店")]
        [OperationId("获取门店")]
        [ResponseCache(Duration = 0)]
        [Parameters(name = "id", param = "门店ID")]
        [HttpGet("{id}")]
        public async Task<IResultModel> Query([Required] Guid id)
        {
            _logger.LogDebug($"根据用户ID:{id}获取用户");
            return await _storeService.Value.GetByIdAsync(id);
        }


        [Description("根据商户ID和门店名获取门店")]
        [OperationId("获取门店")]
        [ResponseCache(Duration = 0)]
        [Parameters(name = "shopId", param = "商店ID")]
        [Parameters(name = "name", param = "门店名")]
        [HttpGet("GetByShopIdAndName/{shopId}/{name}")]
        public async Task<IResultModel> QueryByName([Required] Guid shopId, string name)
        {
            _logger.LogDebug($"根据商店ID:{shopId} 和门店名:{name} 获取门店");
            return await _storeService.Value.GetByShopIdAndNameAsync(shopId, name);
        }

        [Description("通过指定门店ID删除门店")]
        [OperationId("删除门店")]
        [Parameters(name = "id", param = "门店ID")]
        [HttpDelete("{id}")]
        public async Task<IResultModel> Delete([Required] Guid id)
        {
            _logger.LogDebug("删除门店");
            return await _storeService.Value.RemoveAsync(id);
        }

        [Description("通过指定门店ID集合批量删除门店")]
        [OperationId("批量删除门店")]
        [Parameters(name = "ids", param = "门店ID集合")]
        [HttpDelete("BatchDelete")]
        public async Task<IResultModel> BatchDelete([FromBody] List<Guid> ids)
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

        [Description("修改门店，成功后返回当前门店信息")]
        [OperationId("修改门店")]
        [HttpPut]
        public async Task<IResultModel> Update([FromBody] StoreUpdateDto model)
        {
            _logger.LogDebug("修改门店");
            return await _updateStoreService.Value.UpdateAsync(model);
        }

        [Description("使用JsonPatch修改门店，成功后返回当前门店信息")]
        [OperationId("使用JsonPatch修改门店")]
        [HttpPatch("{id}")]
        public async Task<IResultModel> PatchUpdate([FromRoute] Guid id, [FromBody] JsonPatchDocument<StoreUpdateDto> patchDocument)
        {
            _logger.LogDebug("使用JsonPatch修改门店");
            return await _updateStoreService.Value.PatchAsync(id, patchDocument);
        }
    }
}
