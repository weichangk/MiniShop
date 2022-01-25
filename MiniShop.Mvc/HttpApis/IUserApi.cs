using Microsoft.AspNetCore.JsonPatch;
using MiniShop.Dto;
using MiniShop.Model.Enums;
using MiniShop.Mvc.Code;
using Orm.Core;
using Orm.Core.Result;
using System;
using System.Collections.Generic;
using WebApiClient;
using WebApiClient.Attributes;

namespace MiniShop.Mvc.HttpApis
{
    [MiniShopApi]
    [SetAccessTokenFilter]
    [JsonReturn]
    public interface IUserApi : IHttpApi
    {
        [HttpGet("/api/User")]
        ITask<ResultModel<UserDto>> GetByNameAsync(string name);

        [HttpGet("/api/User/GetPageByRankOnShop")]
        ITask<ResultModel<PagedList<UserDto>>> GetPageByRankOnShopAsync(int pageIndex, int pageSize, Guid shopId, EnumRole rank);

        [HttpGet("/api/User/GetPageByRankOnStore")]
        ITask<ResultModel<PagedList<UserDto>>> GetPageByRankOnStoreAsync(int pageIndex, int pageSize, Guid shopId, Guid storeId, EnumRole rank);

        [HttpGet("/api/User/GetPageByRankOnShopWhereQueryRankOrNameOrPhone")]
        ITask<ResultModel<PagedList<UserDto>>> GetPageByRankOnShopWhereQueryRankOrNameOrPhoneAsync(int pageIndex, int pageSize, Guid shopId, EnumRole rank, EnumRole? queryRank, string queryName, string queryPhone);

        [HttpGet("/api/User/GetPageByRankOnShopWhereQueryStoreOrRankOrNameOrPhone")]
        ITask<ResultModel<PagedList<UserDto>>> GetPageByRankOnShopWhereQueryStoreOrRankOrNameOrPhoneAsync(int pageIndex, int pageSize, Guid shopId, EnumRole rank, Guid? queryStore, EnumRole? queryRank, string queryName, string queryPhone);

        [HttpDelete("/api/User")]
        ITask<ResultModel<string>> DeleteByNameAsync(EnumRole rank, string name);

        [HttpDelete("/api/User/BatchDelete")]
        ITask<ResultModel<string>> BatchDeleteByNamesAsync(EnumRole rank, [JsonContent] List<string> names);

        [HttpPost("/api/User")]
        ITask<ResultModel<UserDto>> AddAsync(EnumRole rank, [JsonContent] UserCreateDto model);

        [HttpPut("/api/User")]
        ITask<ResultModel<UserDto>> PutUpdateAsync(EnumRole rank, [JsonContent] UserUpdateDto model);

        [HttpPatch("/api/User")]
        ITask<ResultModel<UserDto>> PatchUpdateByNameAsync(EnumRole rank, string name, [JsonContent] JsonPatchDocument<UserUpdateDto> doc);
    }
}
