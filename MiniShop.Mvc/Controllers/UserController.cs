using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MiniShop.Mvc.HttpApis;
using MiniShop.Mvc.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MiniShop.Mvc.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserApi _userApi;
        private readonly UserManager<IdsDbContext> _userManager;
        private readonly SignInManager<IdsDbContext> _signInManager;

        public UserController(ILogger<UserController> logger, IUserApi userApi) : base(logger)
        {
            _userApi = userApi;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetUserListByShopIdAsync()
        {
            var id = User.Claims.FirstOrDefault(s => s.Type == "LoginShopId")?.Value;
            Guid loginShopId = Guid.Parse(id);
            var result = await _userApi.GetUsersByShopId(loginShopId);
            if (result != null)
            {
                return Json(new Table() { data = result, count = result.Count() });
            }
            return Json(new Table() { data = null, count = 0 });
        }

        [HttpGet]
        public async Task<IActionResult> GetPageUsersByShopId(int page, int limit)
        {
            var id = User.Claims.FirstOrDefault(s => s.Type == "LoginShopId")?.Value;
            Guid loginShopId = Guid.Parse(id);
            var result = await _userApi.GetPageUsersByShopId(page, limit, loginShopId);
            return Json(new Table() { data = result, count = result == null ? 0 : result.Count() });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUserByUserId(int userId)
        {
            var result =  await _userApi.DeleteUserByUserId(userId);
            return Ok();
            //return Json(new Result() { success = result.Success, msg = result.Msg });
        }
    }
}
