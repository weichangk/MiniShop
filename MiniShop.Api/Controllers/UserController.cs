using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MiniShop.Dto;
using MiniShop.IServices;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using yrjw.ORM.Chimp.Result;

namespace MiniShop.Api.Controllers
{
    /// <summary>
    /// 用户信息控制器
    /// </summary>
    [Description("用户信息")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerAbstract
    {
        private readonly Lazy<IUserService> _userService;
        private readonly Lazy<ICreateUserService> _createUserService;

        /// <summary>
        /// 用户信息控制器
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="userService"></param>
        /// <param name="createUserService"></param>
        public UserController(ILogger<ControllerAbstract> logger, Lazy<IUserService> userService, Lazy<ICreateUserService> createUserService) : base(logger)
        {
            _userService = userService;
            _createUserService = createUserService;
        }

        /// <summary>
        /// 获取登录用户信息或店长角色信息首次注册
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="role"></param>
        /// <param name="phone"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        [Description("获取登录用户信息或店长角色信息首次注册")]
        [Parameters(name = "userName", param = "用户名")]
        [Parameters(name = "role", param = "角色")]
        [Parameters(name = "phone", param = "手机号")]
        [Parameters(name = "email", param = "邮箱")]
        [HttpGet("GetLoginInfoOrShopManagerFirstRegister/{userName}/{role}/{phone}/{email}")]
        public async Task<IResultModel> GetLoginInfoOrShopManagerFirstRegister(string userName, string role, string phone, string email)
        {
            _logger.LogDebug($"获取登录用户信息或店长角色信息首次注册");
            return await _userService.Value.GetLoginInfoOrShopManagerFirstRegister(userName, role, phone, email);
        }

        /// <summary>
        /// 根据商店ID获取所有用户
        /// </summary>
        /// <param name="shopId"></param>
        /// <returns></returns>
        [Description("根据商店ID获取所有用户")]
        [OperationId("获取用户列表")]
        [ResponseCache(Duration = 0)]
        [Parameters(name = "shopId", param = "商店ID")]
        [HttpGet("{shopId}")]
        public async Task<IResultModel> Query(Guid shopId)
        {
            _logger.LogDebug($"根据商店ID {shopId} 获取所有用户");
            return await _userService.Value.GetUsersByShopId(shopId);
        }

        /// <summary>
        /// 根据商店ID和分页条件获取所有用户
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="shopId"></param>
        /// <returns></returns>
        [Description("根据商店ID和分页条件获取所有用户")]
        [OperationId("获取用户分页列表")]
        [ResponseCache(Duration = 0)]
        [Parameters(name = "pageIndex", param = "索引页")]
        [Parameters(name = "pageSize", param = "单页条数")]
        [Parameters(name = "shopId", param = "商店ID")]
        [HttpGet("{pageIndex}/{pageSize}/{shopId}")]
        public async Task<IResultModel> Query([Required] int pageIndex, int pageSize, Guid shopId)
        {
            _logger.LogDebug($"根据商店ID:{shopId} 分页条件:pageIndex{pageIndex} pageSize{pageSize} 获取用户");
            return await _userService.Value.GetPageUsersByShopId(pageIndex, pageSize, shopId);
        }

        /// <summary>
        /// 根据用户ID获取用户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [Description("根据用户ID获取用户")]
        [OperationId("获取用户")]
        [ResponseCache(Duration = 0)]
        [Parameters(name = "userId", param = "用户ID")]
        [HttpGet("{userId}")]
        public async Task<IResultModel> Query(int userId)
        {
            _logger.LogDebug($"根据用户ID:{userId}获取用户");
            return await _userService.Value.GetByIdAsync(userId);
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Description("通过指定用户ID删除当前用用户")]
        [OperationId("删除用户")]
        [Parameters(name = "id", param = "用户ID")]
        [HttpDelete("{id}")]
        public async Task<IResultModel> Delete([Required] int id)
        {
            _logger.LogDebug("删除用户");
            return await _userService.Value.RemoveAsync(id);
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Description("添加用户，成功后返回当前用户信息")]
        [OperationId("添加用户")]
        [HttpPost]
        public async Task<IResultModel> Add([FromBody]UserCreateDto model)
        {
            _logger.LogDebug("添加用户");
            return await _createUserService.Value.InsertAsync(model);
        }

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Description("修改用户，成功后返回当前用户信息")]
        [OperationId("修改用户")]
        [HttpPut]
        public async Task<IResultModel> Update([FromBody] UserCreateDto model)
        {
            _logger.LogDebug("修改用户");
            return await _createUserService.Value.UpdateAsync(model);
        }
    }
}
