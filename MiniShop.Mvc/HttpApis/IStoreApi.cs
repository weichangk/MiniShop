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
        [HttpGet("/api/store/{id}")]
        ITask<ResultModel<StoreDto>> GetByIdAsync(int id);

        [HttpGet("/api/store/GetByStoreId/{shopId}")]
        ITask<ResultModel<StoreDto>> GetByStoreIdAsync(Guid shopId);

        [HttpGet("/api/store/GetPageByShopId/{pageIndex}/{pageSize}/{shopId}")]
        ITask<ResultModel<PagedList<StoreDto>>> GetPageByShopIdAsync(int pageIndex, int pageSize, Guid shopId);

        [HttpGet("/api/store/GetPageByShopIdAndWhereQuery/{pageIndex}/{pageSize}/{shopId}/{name}/{contacts}")]
        ITask<ResultModel<PagedList<UserDto>>> GetPageByShopIdAndWhereQueryAsync(int pageIndex, int pageSize, Guid shopId, string name, string contacts);

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
