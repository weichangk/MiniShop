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
using Microsoft.AspNetCore.JsonPatch;

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
                    //错误页面待开发
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

        //修改页面
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var result =  await _userApi.QueryAsync(id);
            if (result.Success)
            {
                ViewBag.OptionRole = "";
                return View(result.Data);
            }

            //错误页待开发
            return View("Edit");
        }

        //保存修改用户信息
        [HttpPost]
        public async Task<IActionResult> SaveEditAsync(UserDto model)
        {
            if(ModelState.IsValid)
            {
                var sas = ViewBag.OptionRole;
                var doc = new JsonPatchDocument<UserUpdateDto>();
                doc.Replace(item => item.Name, model.Name);
                doc.Replace(item => item.Phone, model.Phone);
                doc.Replace(item => item.Email, model.Email);
                doc.Replace(item => item.RoleName, model.Role.ToString());
                var result = await _userApi.PatchUpdateAsync(model.Id, doc);
                if (result.Success)
                {
                    return Json(new Result() { success = result.Success, msg = result.Msg });
                }
                else
                {
                    //错误页面待开发
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
                return Json(new Table() { data = result.Data, count = result.Data.Total});
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

        [HttpPatch]
        public async Task<IActionResult> ChangeEnableAsync(int id, bool enable)
        {
            var doc = new JsonPatchDocument<UserUpdateDto>();
            doc.Replace(item => item.Enable, enable);
            var result = await _userApi.PatchUpdateAsync(id, doc);
            return Json(new Result() { success = result.Success, msg = result.Msg });
        }
    }
}
