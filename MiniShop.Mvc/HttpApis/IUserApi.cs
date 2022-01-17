using Microsoft.AspNetCore.JsonPatch;
using MiniShop.Dto;
using MiniShop.Mvc.Code;
using Orm.Core;
using Orm.Core.Result;
using System;
using System.Collections.Generic;
using WebApiClient;
using WebApiClient.Attributes;

namespace MiniShop.Mvc.HttpApis
{
    [SetAccessTokenFilter]
    [JsonReturn]
    public interface IUserApi : IHttpApi
    {
        [HttpGet("/api/User/{name}")]
        ITask<ResultModel<UserDto>> GetByNameAsync(string name);

        [HttpGet("/api/User/GetPageByShopId/{pageIndex}/{pageSize}/{shopId}")]
        ITask<ResultModel<PagedList<UserDto>>> GetPageByShopIdAsync(int pageIndex, int pageSize, Guid shopId);

        [HttpGet("/api/User/GetPageByShopIdStoreId/{pageIndex}/{pageSize}/{shopId}/{storeId}")]
        ITask<ResultModel<PagedList<UserDto>>> GetPageByShopIdStoreIdAsync(int pageIndex, int pageSize, Guid shopId, Guid storeId);

        [HttpGet("/api/User/GetPageByShopIdAndWhereQuery/{pageIndex}/{pageSize}/{shopId}/{name}/{phone}/{rank}")]
        ITask<ResultModel<PagedList<UserDto>>> GetPageByShopIdAndWhereQueryAsync(int pageIndex, int pageSize, Guid shopId, string name, string phone, string rank);

        [HttpGet("/api/User/GetPageByShopIdStoreIdAndWhereQuery/{pageIndex}/{pageSize}/{shopId}/{storeId}/{name}/{phone}/{rank}")]
        ITask<ResultModel<PagedList<UserDto>>> GetPageByShopIdStoreIdAndWhereQueryAsync(int pageIndex, int pageSize, Guid shopId, Guid storeId, string name, string phone, string rank);

        [HttpDelete("/api/User/{name}")]
        ITask<ResultModel<string>> DeleteAsync(string name);

        [HttpDelete("/api/User/BatchDelete")]
        ITask<ResultModel<string>> BatchDeleteAsync([JsonContent] List<string> names);

        [HttpPost("/api/User")]
        ITask<ResultModel<UserDto>> AddAsync([JsonContent] UserCreateDto model);

        [HttpPut("/api/User")]
        ITask<ResultModel<UserDto>> UpdateAsync([JsonContent] UserUpdateDto model);

        [HttpPatch("/api/User/{name}")]
        ITask<ResultModel<UserDto>> PatchUpdateAsync(string name, [JsonContent] JsonPatchDocument<UserUpdateDto> doc);
    }
}
