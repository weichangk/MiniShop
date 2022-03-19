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
using System.Net;
using System.Threading.Tasks;

namespace MiniShop.Api.Controllers
{
    [Description("单位信息")]
    public class UnitController : ControllerAbstract
    {
        private readonly Lazy<IUnitService> _unitService;
        private readonly Lazy<ICreateUnitService> _createUnitService;
        private readonly Lazy<IUpdateUnitService> _updateUnitService;
        public UnitController(ILogger<UnitController> logger, Lazy<IMapper> mapper,
            Lazy<IUnitService> unitService,
            Lazy<ICreateUnitService> createUnitService,
            Lazy<IUpdateUnitService> updateUnitService) : base(logger, mapper)
        {
            _unitService = unitService;
            _createUnitService = createUnitService;
            _updateUnitService = updateUnitService;
        }

        [Description("根据单位ID查询单位")]
        [OperationId("根据单位ID查询单位")]
        [ResponseCache(Duration = 0)]
        [Parameters(name = "id", param = "单位ID")]
        [HttpGet]
        public async Task<IResultModel> GetById([Required] int id)
        {
            _logger.LogDebug($"根据单位ID：{id} 查询单位");
            return await _unitService.Value.GetByIdAsync(id);
        }

        [Description("根据单位编码查询单位")]
        [OperationId("根据单位编码查询单位")]
        [ResponseCache(Duration = 0)]
        [Parameters(name = "shopId", param = "商店ID")]
        [Parameters(name = "code", param = "单位编码")]
        [HttpGet("GetByCodeOnShop")]
        public async Task<IResultModel> GetByCodeOnShop([Required] Guid shopId, int code)
        {
            _logger.LogDebug($"根据商店ID：{shopId} 单位编码：{code} 查询单位");
            return await _unitService.Value.GetByCodeOnShopAsync(shopId, code);
        }

        [Description("根据商店ID查询最大单位编码")]
        [OperationId("根据商店ID查询最大单位编码")]
        [ResponseCache(Duration = 0)]
        [Parameters(name = "shopId", param = "商店ID")]
        [HttpGet("GetMaxCodeByShopId")]
        public async Task<IResultModel> GetMaxCodeByShopId([Required] Guid shopId)
        {
            _logger.LogDebug($"根据商店ID：{shopId} 查询最大单位编码");
            return await _unitService.Value.GetMaxCodeByShopId(shopId);
        }

        [Description("根据分页条件获取单位")]
        [OperationId("获取单位分页列表")]
        [ResponseCache(Duration = 0)]
        [Parameters(name = "pageIndex", param = "索引页")]
        [Parameters(name = "pageSize", param = "单页条数")]
        [Parameters(name = "shopId", param = "商店ID")]
        [HttpGet("GetPageOnShop")]
        public async Task<IResultModel> GetPageOnShop([Required] int pageIndex, int pageSize, Guid shopId)
        {
            _logger.LogDebug($"根据商店ID：{shopId} 分页条件：索引页{pageIndex} 单页条数{pageSize} 获取单位");
            return await _unitService.Value.GetPageByShopIdAsync(pageIndex, pageSize, shopId);
        }

        [Description("根据商店ID、分页条件、查询条件获取单位")]
        [OperationId("按条件获取单位分页列表")]
        [ResponseCache(Duration = 0)]
        [Parameters(name = "pageIndex", param = "索引页")]
        [Parameters(name = "pageSize", param = "单页条数")]
        [Parameters(name = "shopId", param = "商店ID")]
        [Parameters(name = "code", param = "单位编码")]
        [Parameters(name = "name", param = "单位名称")]
        [HttpGet("GetPageOnShopWhereQueryCodeOrName")]
        public async Task<IResultModel> GetPageOnShopWhereQueryCodeOrName([Required] int pageIndex, int pageSize, Guid shopId, string code, string name)
        {
            _logger.LogDebug($"根据商店ID：{shopId} 分页条件：索引页 {pageIndex} 单页条数 {pageSize} 查询条件：单位编码 {code} 单位名称 {name} 获取单位");
            return await _unitService.Value.GetPageByShopIdWhereQueryAsync(pageIndex, pageSize, shopId, code, name);
        }

        [Description("通过指定单位ID删除单位")]
        [OperationId("删除单位")]
        [Parameters(name = "id", param = "单位ID")]
        [HttpDelete]
        [Authorize(Roles = "ShopManager, ShopAssistant")]
        public async Task<IResultModel> Delete([Required] int id)
        {
            _logger.LogDebug("删除单位");
            var delData = (ResultModel<UnitDto>)(await _unitService.Value.GetByIdAsync(id));
            if (delData == null || delData.Data == null)
            {
                _logger.LogError($"单位删除错误：{id} 不存在");
                return ResultModel.Failed($"单位删除错误：{id} 不存在", (int)HttpStatusCode.NotFound);
            }
            else
            {
                if (delData.Data.Code == 0)
                {
                    return ResultModel.Failed("不能删除系统默认单位", (int)HttpStatusCode.Forbidden);
                }
            }
            return await _unitService.Value.RemoveAsync(id);
        }

        [Description("通过指定单位ID集合批量删除单位")]
        [OperationId("批量删除单位")]
        [Parameters(name = "ids", param = "单位ID集合")]
        [HttpDelete("BatchDelete")]
        [Authorize(Roles = "ShopManager, ShopAssistant")]
        public async Task<IResultModel> BatchDelete([FromBody] List<int> ids)
        {
            _logger.LogDebug("批量删除单位");
            foreach (var id in ids)
            {
                var delData = (ResultModel<UnitDto>)(await _unitService.Value.GetByIdAsync(id));
                if (delData == null || delData.Data == null)
                {
                    _logger.LogError($"单位删除错误：{id} 不存在");
                    return ResultModel.Failed($"单位删除错误：{id} 不存在", (int)HttpStatusCode.NotFound);
                }
                else
                {
                    if (delData.Data.Code == 0)
                    {
                        return ResultModel.Failed("不能删除系统默认单位", (int)HttpStatusCode.Forbidden);
                    }
                }
            }
            return await _unitService.Value.RemoveAsync(ids);
        }

        [Description("添加单位，成功后返回当前单位信息")]
        [OperationId("添加单位")]
        [HttpPost]
        [Authorize(Roles = "ShopManager, ShopAssistant")]
        public async Task<IResultModel> Add([FromBody] UnitCreateDto model)
        {
            _logger.LogDebug("添加单位");
            return await _createUnitService.Value.InsertAsync(model);
        }

        [Description("Put修改单位，成功返回单位信息")]
        [OperationId("修改单位")]
        [HttpPut]
        [Authorize(Roles = "ShopManager, ShopAssistant")]
        public async Task<IResultModel> Update([FromBody] UnitUpdateDto model)
        {
            _logger.LogDebug("修改单位");
            return await _updateUnitService.Value.UpdateAsync(model);
        }

        [Description("Patch使用修改单位，成功返回单位信息")]
        [OperationId("修改单位")]
        [HttpPatch]
        [Authorize(Roles = "ShopManager, ShopAssistant")]
        public async Task<IResultModel> PatchUpdate([FromRoute] int id, [FromBody] JsonPatchDocument<UnitUpdateDto> patchDocument)
        {
            _logger.LogDebug("使用JsonPatch修改单位");
            return await _updateUnitService.Value.PatchAsync(id, patchDocument);
        }






    }
}
