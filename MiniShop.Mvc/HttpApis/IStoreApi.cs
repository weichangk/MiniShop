using Microsoft.AspNetCore.JsonPatch;
using MiniShop.Dto;
using MiniShop.Mvc.Code;
using System;
using System.Collections.Generic;
using Orm.Core;
using Orm.Core.Result;
using WebApiClientCore.Attributes;
using WebApiClientCore;

namespace MiniShop.Mvc.HttpApis
{
    [MiniShopApi]
    [SetAccessToken]
    [JsonReturn]
    public interface IStoreApi : IHttpApi
    {
        [HttpGet("/api/store/{id}")]
        ITask<ResultModel<StoreDto>> GetByIdAsync(int id);

        [HttpGet("/api/store/GetByStoreId/{storeId}")]
        ITask<ResultModel<StoreDto>> GetByStoreIdAsync(Guid storeId);

        [HttpGet("/api/store/GetPageByShopId/{pageIndex}/{pageSize}/{shopId}")]
        ITask<ResultModel<PagedList<StoreDto>>> GetPageByShopIdAsync(int pageIndex, int pageSize, Guid shopId);

        [HttpGet("/api/store/GetPageByShopIdAndWhereQuery/{pageIndex}/{pageSize}/{shopId}/{name}")]
        ITask<ResultModel<PagedList<StoreDto>>> GetPageByShopIdAndWhereQueryAsync(int pageIndex, int pageSize, Guid shopId, string name);

        [HttpGet("/api/store/GetByShopId/{shopId}")]
        ITask<ResultModel<List<StoreDto>>> GetByShopIdAsync(Guid shopId);

        [HttpDelete("/api/store/{id}")]
        ITask<ResultModel<StoreDto>> DeleteAsync(int id);

        [HttpDelete("/api/store/BatchDelete")]
        ITask<ResultModel<StoreDto>> BatchDeleteAsync([JsonContent] List<int> ids);

        [HttpPost("/api/store")]
        ITask<ResultModel<StoreCreateDto>> AddAsync([JsonContent] StoreCreateDto model);

        [HttpPut("/api/store")]
        ITask<ResultModel<StoreUpdateDto>> UpdateAsync([JsonContent] StoreUpdateDto model);

        [HttpPatch("/api/store/{id}")]
        ITask<ResultModel<StoreDto>> PatchUpdateAsync(int id, [JsonContent] JsonPatchDocument<StoreUpdateDto> doc);
    }
}
