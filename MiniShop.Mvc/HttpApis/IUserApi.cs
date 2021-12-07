using MiniShop.Dto;
using System;
using System.Collections.Generic;
using WebApiClient;
using WebApiClient.Attributes;

namespace MiniShop.Mvc.HttpApis
{
    [JsonReturn]
    public interface IUserApi : IHttpApi
    {
        /// <summary>
        /// 首次登录创建默认商店和用户信息
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="phone"></param>
        /// <param name="email"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpGet("/api/User/CreateDefaultShopAndUser/{userName}/{phone}/{email}/{role}")]
        ITask<UserInfoDto> CreateDefaultShopAndUser(string userName, string phone, string email, string role);

        /// <summary>
        /// 根据商店ID获取所有用户
        /// </summary>
        /// <param name="shopId"></param>
        /// <returns></returns>
        [HttpGet("/api/User?shopId={shopId}")]
        ITask<IEnumerable<UserInfoDto>> GetUsersByShopId(Guid shopId);

        /// <summary>
        /// 根据用户ID获取用户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("/api/User?userId={userId}")]
        ITask<IEnumerable<UserInfoDto>> GetUserByUserId(int userId);

        /// <summary>
        /// 在指定商店ID下创建用户
        /// </summary>
        /// <param name="shopId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("/api/User/{shopId}")]
        ITask<UserInfoDto> CreateShop(Guid shopId, [JsonContent] UserCreateDto model);
    }
}
