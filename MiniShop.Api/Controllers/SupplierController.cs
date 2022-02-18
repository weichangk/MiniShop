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
    [Description("供应商信息")]
    public class SupplierController : ControllerAbstract
    {
        private readonly Lazy<ISupplierService> _supplierService;
        private readonly Lazy<ICreateSupplierService> _createSupplierService;
        private readonly Lazy<IUpdateSupplierService> _updateSupplierService;
        public SupplierController(ILogger<SupplierController> logger, Lazy<IMapper> mapper,
            Lazy<ISupplierService> supplierService,
            Lazy<ICreateSupplierService> createSupplierService,
            Lazy<IUpdateSupplierService> updateSupplierService) : base(logger, mapper)
        {
            _supplierService = supplierService;
            _createSupplierService = createSupplierService;
            _updateSupplierService = updateSupplierService;
        }

        [Description("根据供应商ID查询供应商")]
        [OperationId("根据供应商ID查询供应商")]
        [ResponseCache(Duration = 0)]
        [Parameters(name = "id", param = "供应商ID")]
        [HttpGet]
        public async Task<IResultModel> GetByIdAsync([Required] int id)
        {
            _logger.LogDebug($"根据供应商ID：{id} 查询供应商");
            return await _supplierService.Value.GetByIdAsync(id);
        }

        [Description("根据商店ID查询最大供应商编码")]
        [OperationId("根据商店ID查询最大供应商编码")]
        [ResponseCache(Duration = 0)]
        [Parameters(name = "shopId", param = "商店ID")]
        [HttpGet("GetMaxCodeByShopId")]
        public async Task<IResultModel> GetMaxCodeByShopIdAsync([Required] Guid shopId)
        {
            _logger.LogDebug($"根据商店ID：{shopId} 查询最大供应商编码");
            return await _supplierService.Value.GetMaxCodeByShopIdAsync(shopId);
        }

        [Description("根据分页条件获取供应商")]
        [OperationId("获取供应商分页列表")]
        [ResponseCache(Duration = 0)]
        [Parameters(name = "pageIndex", param = "索引页")]
        [Parameters(name = "pageSize", param = "单页条数")]
        [Parameters(name = "shopId", param = "商店ID")]
        [HttpGet("GetPageByShopId")]
        public async Task<IResultModel> GetPageByShopIdAsync([Required] int pageIndex, int pageSize, Guid shopId)
        {
            _logger.LogDebug($"根据商店ID：{shopId} 分页条件：索引页{pageIndex} 单页条数{pageSize} 获取供应商");
            return await _supplierService.Value.GetPageByShopIdAsync(pageIndex, pageSize, shopId);
        }

        [Description("根据商店ID、分页条件、查询条件获取供应商")]
        [OperationId("按条件获取供应商分页列表")]
        [ResponseCache(Duration = 0)]
        [Parameters(name = "pageIndex", param = "索引页")]
        [Parameters(name = "pageSize", param = "单页条数")]
        [Parameters(name = "shopId", param = "商店ID")]
        [Parameters(name = "code", param = "供应商编码")]
        [Parameters(name = "name", param = "供应商名称")]
        [HttpGet("GetPageByShopIdWhereQuery")]
        public async Task<IResultModel> GetPageByShopIdWhereQueryAsync([Required] int pageIndex, int pageSize, Guid shopId, string code, string name)
        {
            _logger.LogDebug($"根据商店ID：{shopId} 分页条件：索引页 {pageIndex} 单页条数 {pageSize} 查询条件：供应商编码 {code} 供应商名称 {name} 获取供应商");
            return await _supplierService.Value.GetPageByShopIdWhereQueryAsync(pageIndex, pageSize, shopId, code, name);
        }

        [Description("通过指定供应商ID删除供应商")]
        [OperationId("删除供应商")]
        [Parameters(name = "id", param = "供应商ID")]
        [HttpDelete]
        [Authorize(Roles = "ShopManager, ShopAssistant")]
        public async Task<IResultModel> DeleteAsync([Required] int id)
        {
            _logger.LogDebug("删除供应商");
            return await _supplierService.Value.RemoveAsync(id);
        }

        [Description("通过指定供应商ID集合批量删除供应商")]
        [OperationId("批量删除供应商")]
        [Parameters(name = "ids", param = "供应商ID集合")]
        [HttpDelete("BatchDelete")]
        [Authorize(Roles = "ShopManager, ShopAssistant")]
        public async Task<IResultModel> BatchDeleteAsync([FromBody] List<int> ids)
        {
            _logger.LogDebug("批量删除供应商");
            return await _supplierService.Value.RemoveAsync(ids);
        }

        [Description("添加供应商，成功后返回当前供应商信息")]
        [OperationId("添加供应商")]
        [HttpPost]
        [Authorize(Roles = "ShopManager, ShopAssistant")]
        public async Task<IResultModel> AddAsync([FromBody] SupplierCreateDto model)
        {
            _logger.LogDebug("添加供应商");
            return await _createSupplierService.Value.InsertAsync(model);
        }

        [Description("Put修改供应商，成功返回供应商信息")]
        [OperationId("修改供应商")]
        [HttpPut]
        [Authorize(Roles = "ShopManager, ShopAssistant")]
        public async Task<IResultModel> UpdateAsync([FromBody] SupplierUpdateDto model)
        {
            _logger.LogDebug("修改供应商");
            return await _updateSupplierService.Value.UpdateAsync(model);
        }

        [Description("Patch使用修改供应商，成功返回供应商信息")]
        [OperationId("修改供应商")]
        [HttpPatch]
        [Authorize(Roles = "ShopManager, ShopAssistant")]
        public async Task<IResultModel> PatchUpdateAsync([FromRoute] int id, [FromBody] JsonPatchDocument<SupplierUpdateDto> patchDocument)
        {
            _logger.LogDebug("使用JsonPatch修改供应商");
            return await _updateSupplierService.Value.PatchAsync(id, patchDocument);
        }

    }
}
