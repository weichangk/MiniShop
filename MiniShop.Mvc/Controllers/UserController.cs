using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MiniShop.Dto;
using MiniShop.Mvc.HttpApis;
using MiniShop.Mvc.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
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

        #region private method
        private async Task<bool> UniqueName(string name)
        {
            var existUser = await _userApi.QueryAsyncByName(name);
            if (existUser.Data != null)
            {
                return false;
            }
            return true;
        }

        private async Task<bool> UniquePhone(string phone)
        {
            var existUser = await _userApi.QueryAsyncByPhone(phone);
            if (existUser.Data != null)
            {
                return false;
            }
            return true;
        }

        private async Task<bool> UniqueEmail(string email)
        {
            var existUser = await _userApi.QueryAsyncByEmail(email);
            if (existUser.Data != null)
            {
                return false;
            }
            return true;
        }
        #endregion


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
                if (! await UniqueName(model.Name))
                {
                    return Json(new Result() { success = false, msg = $"用户名：{model.Name} 已被占用", status = (int)HttpStatusCode.BadRequest });
                }
                if (!await UniquePhone(model.Phone))
                {
                    return Json(new Result() { success = false, msg = $"手机号：{model.Phone} 已被占用", status = (int)HttpStatusCode.BadRequest });
                }
                if (!await UniqueEmail(model.Email))
                {
                    return Json(new Result() { success = false, msg = $"邮箱：{model.Email} 已被占用", status = (int)HttpStatusCode.BadRequest });
                }

                var id = User.Claims.FirstOrDefault(s => s.Type == "LoginShopId")?.Value;
                Guid loginShopId = Guid.Parse(id);
                model.ShopId = loginShopId;
                var result = await _userApi.AddAsync(model);
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
            if (ModelState.IsValid)
            {
                var userDto = await _userApi.QueryAsync(model.Id);
                if (userDto.Data == null)
                {
                    return Json(new Result() { success = false, msg = "查找不到要修改的用户", status = (int)HttpStatusCode.NotFound });
                }
                if (userDto.Data.Name != model.Name && !await UniqueName(model.Name))
                {
                    return Json(new Result() { success = false, msg = $"用户名：{model.Name} 已被占用", status = (int)HttpStatusCode.BadRequest });
                }
                if (userDto.Data.Phone != model.Phone && !await UniquePhone(model.Phone))
                {
                    return Json(new Result() { success = false, msg = $"手机号：{model.Phone} 已被占用", status = (int)HttpStatusCode.BadRequest });
                }
                if (userDto.Data.Email != model.Email && !await UniqueEmail(model.Email))
                {
                    return Json(new Result() { success = false, msg = $"邮箱：{model.Email} 已被占用", status = (int)HttpStatusCode.BadRequest });
                }

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
