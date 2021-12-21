using MiniShop.Dto;
using System;
using System.Collections.Generic;
using WebApiClient;
using WebApiClient.Attributes;
using yrjw.ORM.Chimp;
using yrjw.ORM.Chimp.Result;

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
        ITask<IEnumerable<UserDto>> QueryAsync(Guid shopId);

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
        [HttpGet("/api/User?userId={userId}")]
        ITask<IEnumerable<UserDto>> QueryAsync(int userId);

        [HttpDelete("/api/User/{id}")]
        ITask<ResultModel<UserDto>> DeleteAsync(int id);

        [HttpPost("/api/User")]
        ITask<ResultModel<UserCreateDto>> AddAsync([JsonContent]UserCreateDto model);

        [HttpPut("/api/User")]
        ITask<ResultModel<UserCreateDto>> UpdateAsync([JsonContent] UserCreateDto model);
    }
}
