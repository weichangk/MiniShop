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
        /// <summary>
        /// 根据用户名获取用户
        /// </summary>
        [HttpGet("/api/User/{name}")]
        ITask<ResultModel<UserDto>> QueryAsync(string name);

        /// <summary>
        /// 根据商店ID和分页条件获取商店的用户分页列表
        /// </summary>
        [HttpGet("/api/User/PageListByShop/{pageIndex}/{pageSize}/{shopId}")]
        ITask<ResultModel<PagedList<UserDto>>> QueryPageListByShopAsync(int pageIndex, int pageSize, string shopId);

        /// <summary>
        /// 根据商店ID和分页条件、查询条件获取商店的用户分页列表
        /// </summary>
        [HttpGet("/api/User/PageListByShopAndWhere/{pageIndex}/{pageSize}/{shopId}/{name}/{phone}/{rank}")]
        ITask<ResultModel<PagedList<UserDto>>> QueryPageListByShopAndWhereAsync(int pageIndex, int pageSize, string shopId, string name, string phone, string rank);

        /// <summary>
        /// 根据用户名删除用户
        /// </summary>
        [HttpDelete("/api/User/{name}")]
        ITask<ResultModel<string>> DeleteAsync(string name);

        /// <summary>
        /// 根据用户名批量删除用户
        /// </summary>
        [HttpDelete("/api/User/BatchDelete")]
        ITask<ResultModel<string>> BatchDeleteAsync([JsonContent] List<string> names);

        /// <summary>
        /// 创建用户
        /// </summary>
        [HttpPost("/api/User")]
        ITask<ResultModel<UserDto>> AddAsync([JsonContent] UserCreateDto model);

        /// <summary>
        /// Put修改用户
        /// </summary>
        [HttpPut("/api/User")]
        ITask<ResultModel<UserDto>> UpdateAsync([JsonContent] UserUpdateDto model);

        /// <summary>
        /// Patch修改用户
        /// </summary>
        [HttpPatch("/api/User/{name}")]
        ITask<ResultModel<UserDto>> PatchUpdateAsync(string name, [JsonContent] JsonPatchDocument<UserUpdateDto> doc);
    }
}
