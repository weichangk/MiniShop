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
    [Description("类别信息")]
    public class CategorieController : ControllerAbstract
    {
        private readonly Lazy<ICategorieService> _categorieService;
        private readonly Lazy<ICreateCategorieService> _createCategorieService;
        private readonly Lazy<IUpdateCategorieService> _updateCategorieService;
        public CategorieController(ILogger<CategorieController> logger, Lazy<IMapper> mapper,
            Lazy<ICategorieService> categorieService,
            Lazy<ICreateCategorieService> createCategorieService,
            Lazy<IUpdateCategorieService> updateCategorieService) : base(logger, mapper)
        {
            _categorieService = categorieService;
            _createCategorieService = createCategorieService;
            _updateCategorieService = updateCategorieService;
        }

        [Description("根据类别ID查询类别")]
        [OperationId("根据类别ID查询类别")]
        [ResponseCache(Duration = 0)]
        [Parameters(name = "id", param = "类别ID")]
        [HttpGet]
        public async Task<IResultModel> GetById([Required] int id)
        {
            _logger.LogDebug($"根据类别ID：{id} 查询类别");
            return await _categorieService.Value.GetByIdAsync(id);
        }

        [Description("根据类别编码查询类别")]
        [OperationId("根据类别编码查询类别")]
        [ResponseCache(Duration = 0)]
        [Parameters(name = "shopId", param = "商店ID")]
        [Parameters(name = "code", param = "类别编码")]
        [HttpGet("GetByCodeOnShop")]
        public async Task<IResultModel> GetByCodeOnShop([Required] Guid shopId, int code)
        {
            _logger.LogDebug($"根据商店ID：{shopId} 类别编码：{code} 查询类别");
            return await _categorieService.Value.GetByCodeOnShopAsync(shopId, code);
        }

        [Description("根据类别等级查询最大类别编码")]
        [OperationId("根据类别等级查询最大类别编码")]
        [ResponseCache(Duration = 0)]
        [Parameters(name = "shopId", param = "商店ID")]
        [Parameters(name = "level", param = "类别编码")]
        [HttpGet("GetMaxCodeByLevelOnShop")]
        public async Task<IResultModel> GetMaxCodeByLevelOnShop([Required] Guid shopId, int level)
        {
            _logger.LogDebug($"根据商店ID：{shopId} 类别等级：{level} 查询最大类别编码");
            return await _categorieService.Value.GetMaxCodeByLevelOnShop(shopId, level);
        }

        [Description("根据分页条件获取类别")]
        [OperationId("获取类别分页列表")]
        [ResponseCache(Duration = 0)]
        [Parameters(name = "pageIndex", param = "索引页")]
        [Parameters(name = "pageSize", param = "单页条数")]
        [Parameters(name = "shopId", param = "商店ID")]
        [HttpGet("GetPageOnShop")]
        public async Task<IResultModel> GetPageOnShop([Required] int pageIndex, int pageSize, Guid shopId)
        {
            _logger.LogDebug($"根据商店ID：{shopId} 分页条件：索引页{pageIndex} 单页条数{pageSize} 获取类别");
            return await _categorieService.Value.GetPageByShopIdAsync(pageIndex, pageSize, shopId);
        }

        [Description("根据商店ID、分页条件、查询条件获取类别")]
        [OperationId("按条件获取类别分页列表")]
        [ResponseCache(Duration = 0)]
        [Parameters(name = "pageIndex", param = "索引页")]
        [Parameters(name = "pageSize", param = "单页条数")]
        [Parameters(name = "shopId", param = "商店ID")]
        [Parameters(name = "code", param = "类别编码")]
        [Parameters(name = "name", param = "类别名称")]
        [HttpGet("GetPageOnShopWhereQueryCodeOrName")]
        public async Task<IResultModel> GetPageOnShopWhereQueryCodeOrName([Required] int pageIndex, int pageSize, Guid shopId, string code, string name)
        {
            _logger.LogDebug($"根据商店ID：{shopId} 分页条件：索引页 {pageIndex} 单页条数 {pageSize} 查询条件：类别编码 {code} 类别名称 {name} 获取类别");
            return await _categorieService.Value.GetPageByShopIdWhereQueryAsync(pageIndex, pageSize, shopId, code, name);
        }

        [Description("通过指定类别ID删除类别")]
        [OperationId("删除类别")]
        [Parameters(name = "id", param = "类别ID")]
        [HttpDelete]
        [Authorize(Roles = "ShopManager, ShopAssistant")]
        public async Task<IResultModel> Delete([Required] int id)
        {
            _logger.LogDebug("删除类别");
            var delData = (ResultModel<CategorieDto>)(await _categorieService.Value.GetByIdAsync(id));
            if (delData == null || delData.Data == null)
            {
                _logger.LogError($"类别删除错误：{id} 不存在");
                return ResultModel.Failed($"类别删除错误：{id} 不存在", (int)HttpStatusCode.NotFound);
            }
            else
            {
                if (delData.Data.Code == 0)
                {
                    return ResultModel.Failed("不能删除系统默认类别", (int)HttpStatusCode.Forbidden);
                }
            }
            return await _categorieService.Value.RemoveAsync(id);
        }

        [Description("通过指定类别ID集合批量删除类别")]
        [OperationId("批量删除类别")]
        [Parameters(name = "ids", param = "类别ID集合")]
        [HttpDelete("BatchDelete")]
        [Authorize(Roles = "ShopManager, ShopAssistant")]
        public async Task<IResultModel> BatchDelete([FromBody] List<int> ids)
        {
            _logger.LogDebug("批量删除类别");
            foreach (var id in ids)
            {
                var delData = (ResultModel<CategorieDto>)(await _categorieService.Value.GetByIdAsync(id));
                if (delData == null || delData.Data == null)
                {
                    _logger.LogError($"类别删除错误：{id} 不存在");
                    return ResultModel.Failed($"类别删除错误：{id} 不存在", (int)HttpStatusCode.NotFound);
                }
                else
                {
                    if (delData.Data.Code == 0)
                    {
                        return ResultModel.Failed("不能删除系统默认类别", (int)HttpStatusCode.Forbidden);
                    }
                }
            }
            return await _categorieService.Value.RemoveAsync(ids);
        }

        [Description("添加类别，成功后返回当前类别信息")]
        [OperationId("添加类别")]
        [HttpPost]
        [Authorize(Roles = "ShopManager, ShopAssistant")]
        public async Task<IResultModel> Add([FromBody] CategorieCreateDto model)
        {
            _logger.LogDebug("添加类别");
            return await _createCategorieService.Value.InsertAsync(model);
        }

        [Description("Put修改类别，成功返回类别信息")]
        [OperationId("修改类别")]
        [HttpPut]
        [Authorize(Roles = "ShopManager, ShopAssistant")]
        public async Task<IResultModel> Update([FromBody] CategorieUpdateDto model)
        {
            _logger.LogDebug("修改类别");
            return await _updateCategorieService.Value.UpdateAsync(model);
        }

        [Description("Patch使用修改类别，成功返回类别信息")]
        [OperationId("修改类别")]
        [HttpPatch]
        [Authorize(Roles = "ShopManager, ShopAssistant")]
        public async Task<IResultModel> PatchUpdate([FromRoute] int id, [FromBody] JsonPatchDocument<CategorieUpdateDto> patchDocument)
        {
            _logger.LogDebug("使用JsonPatch修改类别");
            return await _updateCategorieService.Value.PatchAsync(id, patchDocument);
        }






    }
}
