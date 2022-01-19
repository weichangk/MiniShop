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
    [SetAccessTokenFilter]
    [JsonReturn]
    public interface IUserApi : IHttpApi
    {
        [HttpGet("/api/User/{name}")]
        ITask<ResultModel<UserDto>> GetByNameAsync(string name);

        [HttpGet("/api/User/GetPage/{pageIndex}/{pageSize}/{shopId}/{rank}")]
        ITask<ResultModel<PagedList<UserDto>>> GetPageAsync(int pageIndex, int pageSize, Guid shopId, EnumRole rank);

        [HttpGet("/api/User/GetPage/{pageIndex}/{pageSize}/{shopId}/{storeId}/{rank}")]
        ITask<ResultModel<PagedList<UserDto>>> GetPageAsync(int pageIndex, int pageSize, Guid shopId, Guid storeId, EnumRole rank);

        [HttpGet("/api/User/GetPageWhereQuery/{pageIndex}/{pageSize}/{shopId}/{rank}/{queryRank}/{queryName}/{queryPhone}")]
        ITask<ResultModel<PagedList<UserDto>>> GetPageWhereQueryAsync(int pageIndex, int pageSize, Guid shopId, EnumRole rank, EnumRole? queryRank, string queryName, string queryPhone);

        [HttpGet("/api/User/GetPageWhereQuery/{pageIndex}/{pageSize}/{shopId}/{rank}/{queryStore}/{queryRank}/{queryName}/{queryPhone}")]
        ITask<ResultModel<PagedList<UserDto>>> GetPageWhereQueryAsync(int pageIndex, int pageSize, Guid shopId, EnumRole rank, Guid? queryStore, EnumRole? queryRank, string queryName, string queryPhone);

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
