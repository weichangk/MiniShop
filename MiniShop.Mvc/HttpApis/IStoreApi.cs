using Microsoft.AspNetCore.JsonPatch;
using MiniShop.Dto;
using MiniShop.Mvc.Code;
using System;
using System.Collections.Generic;
using WebApiClient;
using WebApiClient.Attributes;
using Orm.Core;
using Orm.Core.Result;

namespace MiniShop.Mvc.HttpApis
{
    [SetAccessTokenFilter]
    [JsonReturn]
    public interface IStoreApi : IHttpApi
    {
        /// <summary>
        /// 根据商店ID获取所有门店
        /// </summary>
        /// <param name="shopId"></param>
        /// <returns></returns>
        [HttpGet("/api/store?shopId={shopId}")]
        ITask<ResultModel<PagedList<StoreDto>>> QueryByShopIdAsync(Guid shopId);

        /// <summary>
        /// 根据商店ID、分页条件获取所有门店
        /// </summary>
        /// <param name="shopId"></param>
        /// <returns></returns>
        [HttpGet("/api/store/{pageIndex}/{pageSize}/{shopId}")]
        ITask<ResultModel<PagedList<StoreDto>>> GetPageListAsync(int pageIndex, int pageSize, Guid shopId);

        /// <summary>
        /// 根据商店ID、分页条件、查询条件查询门店
        /// </summary>
        /// <param name="shopId"></param>
        /// <returns></returns>
        [HttpGet("/api/store/GetPageAndWhereQuery/{pageIndex}/{pageSize}/{shopId}/{name}/{contacts}")]
        ITask<ResultModel<PagedList<UserDto>>> GetPageListAndWhereQueryAsync(int pageIndex, int pageSize, Guid shopId, string name, string contacts);

        /// <summary>
        /// 根据门店ID获取门店 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("/api/store/{id}")]
        ITask<ResultModel<StoreDto>> QueryByIdAsync(Guid id);

        /// <summary>
        /// 根据商店ID和门店名获取门店
        /// </summary>
        /// <param name="shopId"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet("/api/store/QueryByShopIdAndName/{shopId}/{name}")]
        ITask<ResultModel<StoreDto>> QueryAsyncByName(Guid shopId, string name);

        /// <summary>
        /// 根据门店id删除门店
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("/api/store/{id}")]
        ITask<ResultModel<StoreDto>> DeleteAsync(Guid id);

        /// <summary>
        /// 根据门店id批量删除门店
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete("/api/store/BatchDelete")]
        ITask<ResultModel<StoreDto>> BatchDeleteAsync([JsonContent] List<Guid> ids);

        /// <summary>
        /// 创建门店
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("/api/store")]
        ITask<ResultModel<StoreCreateDto>> AddAsync([JsonContent] StoreCreateDto model);

        /// <summary>
        /// 修改门店
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("/api/store")]
        ITask<ResultModel<StoreUpdateDto>> UpdateAsync([JsonContent] StoreUpdateDto model);

        /// <summary>
        /// 使用JsonPatch修改门店
        /// </summary>
        /// <param name="id"></param>
        /// <param name="doc"></param>
        /// <returns></returns>
        [HttpPatch("/api/store/{id}")]
        ITask<ResultModel<StoreDto>> PatchUpdateAsync(Guid id, [JsonContent] JsonPatchDocument<StoreUpdateDto> doc);
    }
}
