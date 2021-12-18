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
            var model = new UserDto();
            var id = User.Claims.FirstOrDefault(s => s.Type == "LoginShopId")?.Value;
            Guid loginShopId = Guid.Parse(id);
            model.ShopId = loginShopId;
            return View(model);
        }

        //表单提交，保存用户信息，id=0 添加，id>0 修改
        [HttpPost]
        public async Task<IActionResult> SaveAsync(UserDto model)
        {
            if (ModelState.IsValid)
            {
                IResultModel result;
                if (model.Id == 0)
                {
                    result = await _userApi.AddAsync(model);
                }
                else
                {
                    result = await _userApi.UpdateAsync(model);
                }
                if (result.Success)
                {
                    //var _msg = model.Id == 0 ? "添加成功！" : "修改成功！";
                    //return RedirectToAction("ShowMsg", "Home", new { msg = _msg });
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

            if (model.Id == 0)
            {
                return View("Create", model);
            }
            return View("Edit", model);
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
        public async Task<IActionResult> DeleteUserByUserId(int userId)
        {
            ////var result =  await _userApi.DeleteUserByUserId(userId);
            return Ok();
            //return Json(new Result() { success = result.Success, msg = result.Msg });
        }
    }
}
