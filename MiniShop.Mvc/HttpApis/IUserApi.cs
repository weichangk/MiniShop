using Microsoft.AspNetCore.JsonPatch;
using MiniShop.Dto;
using MiniShop.Mvc.Code;
using Orm.Core;
using Orm.Core.Result;
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
        ITask<ResultModel<UserDto>> QueryAsync(string name);

        [HttpGet("/api/User/PageListByShop/{pageIndex}/{pageSize}/{shopId}")]
        ITask<ResultModel<PagedList<UserDto>>> QueryPageListByShopAsync(int pageIndex, int pageSize, string shopId);

        [HttpGet("/api/User/PageListByShopAndWhere/{pageIndex}/{pageSize}/{shopId}/{name}/{phone}/{rank}")]
        ITask<ResultModel<PagedList<UserDto>>> QueryPageListByShopAndWhereAsync(int pageIndex, int pageSize, string shopId, string name, string phone, string rank);

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
