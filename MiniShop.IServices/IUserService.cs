using MiniShop.Dto;
using MiniShop.Model;
using System;
using System.Threading.Tasks;
using yrjw.ORM.Chimp.Result;

namespace MiniShop.IServices
{
    public interface IUserService : IBaseService<User, UserDto, int>
    {
        /// <summary>
        /// 获取登录用户信息或店长角色信息首次注册
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="role"></param>
        /// <param name="phone"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<IResultModel> GetLoginInfoOrShopManagerFirstRegister(string userName, string role, string phone, string email);

        /// <summary>
        /// 根据商店ID获取所有用户
        /// </summary>
        /// <param name="shopId"></param>
        /// <returns></returns>
        Task<IResultModel> GetUsersByShopId(Guid shopId);

        /// <summary>
        /// 根据商店ID和分页条件获取所有用户
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="shopId"></param>
        /// <returns></returns>
        Task<IResultModel> GetPageUsersByShopId(int pageIndex, int pageSize, Guid shopId);
    }
}
