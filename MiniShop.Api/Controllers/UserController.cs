using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MiniShop.Dto;
using MiniShop.IServices;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using yrjw.ORM.Chimp.Result;
using Microsoft.AspNetCore.JsonPatch;

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
        private readonly Lazy<IUpdateUserService> _updateUserService;

        /// <summary>
        /// 用户信息控制器
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="userService"></param>
        /// <param name="createUserService"></param>
        /// <param name="updateUserService"></param>
        public UserController(ILogger<ControllerAbstract> logger, Lazy<IUserService> userService, Lazy<ICreateUserService> createUserService,
        Lazy<IUpdateUserService> updateUserService) : base(logger)
        {
            _userService = userService;
            _createUserService = createUserService;
            _updateUserService = updateUserService;
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
        /// <param name="id"></param>
        /// <returns></returns>
        [Description("根据用户ID获取用户")]
        [OperationId("获取用户")]
        [ResponseCache(Duration = 0)]
        [Parameters(name = "id", param = "用户ID")]
        [HttpGet("{id}")]
        public async Task<IResultModel> Query([Required] int id)
        {
            _logger.LogDebug($"根据用户ID:{id}获取用户");
            return await _userService.Value.GetByIdAsync(id);
        }

        /// <summary>
        /// 根据用户名获取用户
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [Description("根据用户名获取用户")]
        [OperationId("获取用户")]
        [ResponseCache(Duration = 0)]
        [Parameters(name = "name", param = "用户名")]
        [HttpGet("QueryByName/{name}")]
        public async Task<IResultModel> QueryByName([Required] string name)
        {
            _logger.LogDebug($"根据用户名:{name}获取用户");
            return await _userService.Value.GetByNameAsync(name);
        }

        /// <summary>
        /// 根据手机号获取用户
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        [Description("根据手机号获取用户")]
        [OperationId("获取用户")]
        [ResponseCache(Duration = 0)]
        [Parameters(name = "phone", param = "手机号")]
        [HttpGet("QueryByPhone/{phone}")]
        public async Task<IResultModel> QueryByPhone([Required] string phone)
        {
            _logger.LogDebug($"根据手机号:{phone}获取用户");
            return await _userService.Value.GetByPhoneAsync(phone);
        }

        /// <summary>
        /// 根据邮箱获取用户
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [Description("根据邮箱获取用户")]
        [OperationId("获取用户")]
        [ResponseCache(Duration = 0)]
        [Parameters(name = "email", param = "邮箱")]
        [HttpGet("QueryByEmail/{email}")]
        public async Task<IResultModel> QueryByEmail([Required] string email)
        {
            _logger.LogDebug($"根据邮箱:{email}获取用户");
            return await _userService.Value.GetByEmailAsync(email);
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
        public async Task<IResultModel> Update([FromBody] UserUpdateDto model)
        {
            _logger.LogDebug("修改用户");
            return await _updateUserService.Value.UpdateAsync(model);
        }

        /// <summary>
        /// 使用JsonPatch修改用户
        /// </summary>
        /// <param name="id"></param>
        /// <param name="patchDocument"></param>
        /// <returns></returns>
        [Description("使用JsonPatch修改用户，成功后返回当前用户信息")]
        [OperationId("使用JsonPatch修改用户")]
        [HttpPatch("{id}")]
        public async Task<IResultModel> PatchUpdate([FromRoute]int id, [FromBody] JsonPatchDocument<UserUpdateDto> patchDocument)
        {
            _logger.LogDebug("使用JsonPatch修改用户");
            return await _updateUserService.Value.PatchAsync(id, patchDocument);
        }
    }
}
