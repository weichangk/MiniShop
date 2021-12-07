using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MiniShop.Dto;
using MiniShop.IServices;
using MiniShop.Model;
using MiniShop.Model.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace MiniShop.Api.Controllers
{
    [Description("用户信息")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerAbstract
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IShopService _shopService;
        public UserController(ILogger<ControllerAbstract> logger, IMapper mapper, IUserService userService, IShopService shopService) : base(logger)
        {
            _mapper = mapper;
            _userService = userService;
            _shopService = shopService;
        }

        [Description("首次登录创建默认商店和用户信息")]
        [HttpGet("CreateDefaultShopAndUser/{userName}/{phone}/{email}/{role}")]
        public async Task<IActionResult> CreateDefaultShopAndUser(string userName, string phone, string email, string role)
        {
            User user = _userService.UserExist(userName).Result;
            if (user == null)
            {
                if (role != null && role.Equals(EnumRole.ShopManager.ToString()))
                {
                    _logger.LogDebug("首次登录创建默认商店和用户信息");
                    user = _userService.CreateDefaultShopAndUser(userName, phone, email);
                    await _userService.SaveAsync();
                }
                else
                {
                    return NotFound("用户不存在");
                }
            }
            var userInfo = _mapper.Map<UserInfoDto>(user);
            return Ok(userInfo);
        }

        [Description("获取所有用户")]
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            _logger.LogDebug("获取所有用户");
            var users = await _userService.Select(u => u.Id >= 1).ToListAsync();
            if (users == null || users.Count() <= 0)
            {
                return NotFound("没有找到用户数据");
            }
            var usersInfo = _mapper.Map<IEnumerable<UserInfoDto>>(users);
            return Ok(usersInfo);
        }

        [Description("根据商店ID获取所有用户")]
        [HttpGet("{shopId}")]
        public async Task<IActionResult> GetUsersByShopId(Guid shopId)
        {
            _logger.LogDebug($"根据商店ID:{shopId}获取所有用户");
            var users = await _userService.Select(u => u.ShopId.Equals(shopId)).ToListAsync();
            if (users == null || users.Count() <= 0)
            {
                return NotFound($"商店:{shopId}没有找到用户数据");
            }
            var usersInfo = _mapper.Map<IEnumerable<UserInfoDto>>(users);
            return Ok(usersInfo);
        }

        [Description("根据用户ID获取用户")]
        [HttpGet("{userId}", Name = "GetUserByUserId")]
        public async Task<IActionResult> GetUserByUserId(int userId)
        {
            _logger.LogDebug($"根据用户ID:{userId}获取用户");
            var user = await _userService.Select(u => u.Id.Equals(userId)).FirstOrDefaultAsync();
            if (user == null)
            {
                return NotFound($"没有找到用户:{userId}");
            }
            return Ok(user);
        }

        [Description("在指定商店ID下创建用户")]
        [HttpPost("{shopId}")]
        public async Task<IActionResult> CreateShop(Guid shopId, [FromBody] UserCreateDto userCreateDto)
        {
            _logger.LogDebug($"在指定商店ID:{shopId}下创建用户");
            var shop = await _shopService.Select(s => s.Id == shopId).FirstOrDefaultAsync();
            if (shop == null)
            {
                return NotFound($"没有找到商店:{shopId}");
            }
            var user = _mapper.Map<User>(userCreateDto);
            user.ShopId = shopId;
            user.Role = EnumRole.Admin;
            var newUser = _userService.Insert(user);
            await _userService.SaveAsync();
            return CreatedAtRoute("GetUserByUserId", new { userId = newUser.Id }, newUser);
        }
    }
}
