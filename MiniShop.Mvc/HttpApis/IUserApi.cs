using MiniShop.Dto;
using System;
using System.Collections.Generic;
using WebApiClient;
using WebApiClient.Attributes;
using yrjw.ORM.Chimp;
using yrjw.ORM.Chimp.Result;
using Microsoft.AspNetCore.JsonPatch;

namespace MiniShop.Mvc.HttpApis
{
    [JsonReturn]
    public interface IUserApi : IHttpApi
    {
        /// <summary>
        /// 获取登录用户信息或店长角色信息首次注册
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="phone"></param>
        /// <param name="email"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpGet("/api/User/GetLoginInfoOrShopManagerFirstRegister/{userName}/{role}/{phone}/{email}")]
        ITask<ResultModel<UserDto>> GetLoginInfoOrShopManagerFirstRegister(string userName, string role, string phone, string email);

        /// <summary>
        /// 根据商店ID获取所有用户
        /// </summary>
        /// <param name="shopId"></param>
        /// <returns></returns>
        [HttpGet("/api/User?shopId={shopId}")]
        ITask<ResultModel<PagedList<UserDto>>> QueryAsync(Guid shopId);

        /// <summary>
        /// 根据商店ID和分页条件获取所有用户
        /// </summary>
        /// <param name="shopId"></param>
        /// <returns></returns>
        //[HttpGet("/api/User?pageIndex={pageIndex}&pageSize={pageSize}&shopId={shopId}")]
        [HttpGet("/api/User/{pageIndex}/{pageSize}/{shopId}")]
        ITask<ResultModel<PagedList<UserDto>>> GetPagedListAsync(int pageIndex, int pageSize, Guid shopId);

        /// <summary>
        /// 根据用户ID获取用户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("/api/User/{id}")]
        ITask<ResultModel<UserDto>> QueryAsync(int id);

        /// <summary>
        /// 根据用户名获取用户
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet("/api/User/QueryByName/{name}")]
        ITask<ResultModel<UserDto>> QueryAsyncByName(string name);

        /// <summary>
        /// 根据用户名获取用户
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        [HttpGet("/api/User/QueryByPhone/{phone}")]
        ITask<ResultModel<UserDto>> QueryAsyncByPhone(string phone);

        /// <summary>
        /// 根据用户名获取用户
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpGet("/api/User/QueryByEmail/{email}")]
        ITask<ResultModel<UserDto>> QueryAsyncByEmail(string email);

        /// <summary>
        /// 根据用户id删除用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("/api/User/{id}")]
        ITask<ResultModel<UserDto>> DeleteAsync(int id);

        /// <summary>
        /// 创建用户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("/api/User")]
        ITask<ResultModel<UserCreateDto>> AddAsync([JsonContent]UserCreateDto model);

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("/api/User")]
        ITask<ResultModel<UserUpdateDto>> UpdateAsync([JsonContent] UserUpdateDto model);

        /// <summary>
        /// 使用JsonPatch修改用户
        /// </summary>
        /// <param name="id"></param>
        /// <param name="doc"></param>
        /// <returns></returns>
        [HttpPatch("/api/User/{id}")]
        ITask<ResultModel<UserDto>> PatchUpdateAsync(int id, [JsonContent] JsonPatchDocument<UserUpdateDto> doc);
    }
}
