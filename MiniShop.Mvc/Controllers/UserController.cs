using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MiniShop.Dto;
using MiniShop.Mvc.HttpApis;
using MiniShop.Mvc.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using yrjw.ORM.Chimp.Result;

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


        //添加页面
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        //保存添加用户信息
        [HttpPost]
        public async Task<IActionResult> SaveAddAsync([FromBody] UserCreateDto model)      
        {
            if (ModelState.IsValid)
            {
                IResultModel result;
                var id = User.Claims.FirstOrDefault(s => s.Type == "LoginShopId")?.Value;
                Guid loginShopId = Guid.Parse(id);
                model.ShopId = loginShopId;
                result = await _userApi.AddAsync(model);
                if (result.Success)
                {
                    return Json(new Result() { success = result.Success, msg = result.Msg });
                }
                else
                {
                    if (result.Errors.Count > 0)
                    {
                        ModelState.AddModelError(result.Errors[0].Id, result.Errors[0].Msg);
                    }
                    else
                    {
                        ModelState.AddModelError("error", result.Msg);
                    }
                }
            }
            return View("Create", model);
        }

        //Layui数据表格异步获取展示列表数据
        [ResponseCache(Duration = 0)]
        [HttpGet]
        public async Task<IActionResult> GetUserListByShopIdAsync()
        {
            var id = User.Claims.FirstOrDefault(s => s.Type == "LoginShopId")?.Value;
            Guid loginShopId = Guid.Parse(id);
            var result = await _userApi.QueryAsync(loginShopId);
            if (result != null)
            {
                return Json(new Table() { data = result, count = result.Count() });
            }
            return Json(new Table() { data = null, count = 0 });
        }

        [HttpGet]
        public async Task<IActionResult> GetPagedListAsync(int page, int limit)
        {
            var id = User.Claims.FirstOrDefault(s => s.Type == "LoginShopId")?.Value;
            Guid loginShopId = Guid.Parse(id);
            var result = await _userApi.GetPagedListAsync(page, limit, loginShopId);
            return Json(new Table() { data = result.Data.Item, count = result == null ? 0 : result.Data.Total });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await _userApi.DeleteAsync(id);
            return Json(new Result() { success = result.Success, msg = result.Msg });
        }
    }

    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
