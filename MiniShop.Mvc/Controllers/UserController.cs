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
using System.Net;

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
            UserCreateDto model = new UserCreateDto { Role = Model.EnumRole.Admin, };
            return View(model);
        }

        //保存添加用户信息
        [HttpPost]
        public async Task<IActionResult> SaveAddAsync(UserCreateDto model)      
        {
            if (ModelState.IsValid)
            {
                IResultModel result;
                var id = User.Claims.FirstOrDefault(s => s.Type == "LoginShopId")?.Value;
                Guid loginShopId = Guid.Parse(id);
                model.ShopId = loginShopId;
                result = await _userApi.AddAsync(model);
                return Json(new Result() { success = result.Success, msg = result.Msg, status = result.Status });
            }
            return Json(new Result() { success = false, msg = ModelStateErrorMessage(ModelState), status = (int)HttpStatusCode.BadRequest });
        }

        //修改页面
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var result =  await _userApi.QueryAsync(id);
            if (result.Success)
            {
                return View(result.Data);
            }
            return Json(new Result() { success = result.Success, msg = result.Msg, status = result.Status });
        }

        //保存修改用户信息
        [HttpPost]
        public async Task<IActionResult> SaveEditAsync(UserDto model)
        {
            return Json(new Result() { success = false, msg = ModelStateErrorMessage(ModelState), status = 500 });
            if (ModelState.IsValid)
            {
                var doc = new JsonPatchDocument<UserUpdateDto>();
                doc.Replace(item => item.Name, model.Name);
                doc.Replace(item => item.Phone, model.Phone);
                doc.Replace(item => item.Email, model.Email);
                doc.Replace(item => item.Role, model.Role);
                var result = await _userApi.PatchUpdateAsync(model.Id, doc);
                return Json(new Result() { success = result.Success, msg = result.Msg, status = result.Status });
            }
            return Json(new Result() { success = false, msg = ModelStateErrorMessage(ModelState), status = (int)HttpStatusCode.BadRequest });
        }

        [ResponseCache(Duration = 0)]
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
            var userDto = await _userApi.QueryAsync(id);
            if (userDto.Data != null)
            {
                if (userDto.Data.Role == Model.EnumRole.ShopManager)
                {
                    return Json(new Result() { success = false, msg = "不能删除店长" });
                }
                var result = await _userApi.DeleteAsync(id);
                return Json(new Result() { success = result.Success, msg = result.Msg, status = result.Status });
            }
            else
            {
                return Json(new Result() { success = false, msg = "查找不到要删除的用户", status = (int)HttpStatusCode.NotFound });
            }
        }

        [HttpPatch]
        public async Task<IActionResult> ChangeEnableAsync(int id, bool enable)
        {
            var userDto = await _userApi.QueryAsync(id);
            if (userDto.Data != null)
            {
                if (userDto.Data.Role == Model.EnumRole.ShopManager)
                {
                    return Json(new Result() { success = false, msg = "不能禁用店长" });
                }
                var doc = new JsonPatchDocument<UserUpdateDto>();
                doc.Replace(item => item.Enable, enable);
                var result = await _userApi.PatchUpdateAsync(id, doc);
                return Json(new Result() { success = result.Success, msg = result.Msg, status = result.Status });
            }
            else
            {
                return Json(new Result() { success = false, msg = "查找不到要修改的用户", status = (int)HttpStatusCode.NotFound });
            }
        }
    }
}
