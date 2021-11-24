using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MiniShop.Api.Services;
using MiniShop.Dto;
using MiniShop.Model;
using MiniShop.Model.Enums;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MiniShop.Api.Controllers
{
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

        [HttpGet("UserLogin/{userName}/{phone}/{email}/{role}")]
        public async Task<IActionResult> UserLogin(string userName, string phone, string email, string role)
        {
            User user = _userService.UserExist(userName).Result;
            if (user == null)
            {
                if (role != null && role.Equals(EnumRole.ShopManager.ToString()))
                {
                    user = _userService.CreateShopManagerUser(userName, phone, email);
                    await _userService.SaveAsync();
                }
                else
                {
                    return NotFound("用户不存在");
                }
            }
            return Ok(user);
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.Select(u => u.Id >= 1).ToListAsync();
            if (users == null || users.Count() <= 0)
            {
                return NotFound("没有找到用户数据");
            }
            return Ok(users);
        }

        [HttpGet("GetUsersByShopId/{shopId}", Name = "GetUsersByShopId")]
        public async Task<IActionResult> GetUsersByShopId(int shopId)
        {
            var users = await _userService.Select(u => u.ShopId.Equals(shopId)).ToListAsync();
            if (users == null || users.Count() <= 0)
            {
                return NotFound($"商店{shopId} 没有找到用户数据");
            }
            return Ok(users);
        }

        [HttpGet("GetUserByUserId/{userId}", Name = "GetUserByUserId")]
        public async Task<IActionResult> GetUserByUserId(int userId)
        {
            var user = await _userService.Select(u => u.Id.Equals(userId)).FirstOrDefaultAsync();
            if (user == null)
            {
                return NotFound($"没有找到用户{userId}");
            }
            return Ok(user);
        }

        [HttpPost("{shopId}")]
        public async Task<IActionResult> CreateShop(Guid shopId, [FromBody] UserCreateDto userCreateDto)
        {
            var shop = await _shopService.Select(s => s.Id == shopId).FirstOrDefaultAsync();
            if (shop == null)
            {
                return NotFound($"没有找到商店{shopId}");
            }
            var user = _mapper.Map<User>(userCreateDto);
            user.ShopId = shopId;
            user.Role = Model.Enums.EnumRole.Admin;
            var newUser = _userService.Insert(user);
            await _userService.SaveAsync();
            return CreatedAtRoute("GetUserByUserId", new { userId = newUser.Id }, newUser);
        }
    }
}
