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
using IdentityModel;
using System.Security.Claims;

namespace MiniShop.Mvc.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserApi _userApi;
        private readonly UserManager<IdentityUser> _userManager;

        public UserController(ILogger<UserController> logger, IUserApi userApi, UserManager<IdentityUser> userManager) : base(logger)
        {
            _userApi = userApi;
            _userManager = userManager;
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

                //1. 存后台数据库
                var result = await _userApi.AddAsync(model);
                if (result.Success)
                {
                    //2. 存登录中心数据库
                    var user = new IdentityUser
                    {
                        UserName = model.Name,
                        PhoneNumber = model.Phone,
                        Email = model.Email,
                    };
                    var identityResult = await _userManager.CreateAsync(user, model.PassWord);
                    if (identityResult.Succeeded)
                    {
                        identityResult = await _userManager.AddClaimsAsync(user, new Claim[] { new Claim(JwtClaimTypes.Role, model.Role.ToString()) });
                        if (identityResult.Succeeded)
                        {
                            //成功
                            return Json(new Result() { success = result.Success, msg = result.Msg, status = result.Status });
                        }
                        else
                        {
                            //失败
                            string errors = "";
                            foreach (var error in identityResult.Errors)
                            {
                                errors = $"{errors}{error.Description} ";
                            }
                            return Json(new Result() { success = false, msg = $"添加用户失败：{errors}", status = (int)HttpStatusCode.BadRequest });
                            //回滚，删除前面存登录中心数据库的用户
                            //回滚，删除前面存后台数据库的用户
                        }

                    }
                    else
                    {
                        //失败
                        string errors = "";
                        foreach (var error in identityResult.Errors)
                        {
                            errors = $"{errors}{error.Description} ";
                        }
                        return Json(new Result() { success = false, msg = $"添加用户失败：{errors}", status = (int)HttpStatusCode.BadRequest });
                        //回滚，删除前面存后台数据库的用户
                    }
                }

                //失败
                return Json(new Result() { success = result.Success, msg = result.Msg, status = result.Status });
            }
            //失败
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
                var oldUserDto = await _userApi.QueryAsync(model.Id);
                if (oldUserDto.Data == null)
                {
                    return Json(new Result() { success = false, msg = "查找不到要修改的用户", status = (int)HttpStatusCode.NotFound });
                }
                if (oldUserDto.Data.Name != model.Name && !await UniqueName(model.Name))
                {
                    return Json(new Result() { success = false, msg = $"用户名：{model.Name} 已被占用", status = (int)HttpStatusCode.BadRequest });
                }
                if (oldUserDto.Data.Phone != model.Phone && !await UniquePhone(model.Phone))
                {
                    return Json(new Result() { success = false, msg = $"手机号：{model.Phone} 已被占用", status = (int)HttpStatusCode.BadRequest });
                }
                if (oldUserDto.Data.Email != model.Email && !await UniqueEmail(model.Email))
                {
                    return Json(new Result() { success = false, msg = $"邮箱：{model.Email} 已被占用", status = (int)HttpStatusCode.BadRequest });
                }

                var doc = new JsonPatchDocument<UserUpdateDto>();
                doc.Replace(item => item.Name, model.Name);
                doc.Replace(item => item.Phone, model.Phone);
                doc.Replace(item => item.Email, model.Email);
                doc.Replace(item => item.Role, model.Role);

                //1. 更新后台数据库的用户
                var result = await _userApi.PatchUpdateAsync(model.Id, doc);
                if (result.Success)
                {
                    //2. 更新登陆中心数据库的用户
                    var identityUser = await _userManager.FindByNameAsync(oldUserDto.Data.Name);
                    if (identityUser != null)
                    {
                        identityUser.UserName = model.Name;
                        identityUser.PhoneNumber = model.Phone;
                        identityUser.Email = model.Email;
                        var identityResult = await _userManager.UpdateAsync(identityUser);
                        if (identityResult.Succeeded)
                        {
                            if (oldUserDto.Data.Role != model.Role)
                            {
                                identityResult = await _userManager.ReplaceClaimAsync(identityUser,  new Claim(JwtClaimTypes.Role, oldUserDto.Data.Role.ToString()), new Claim(JwtClaimTypes.Role, model.Role.ToString()) );
                                if (identityResult.Succeeded)
                                {
                                    //成功
                                    return Json(new Result() { success = result.Success, msg = result.Msg, status = result.Status });
                                }
                                else
                                {
                                    //失败
                                    string errors = "";
                                    foreach (var error in identityResult.Errors)
                                    {
                                        errors = $"{errors}{error.Description} ";
                                    }
                                    return Json(new Result() { success = false, msg = "更新用户失败：{errors}", status = (int)HttpStatusCode.BadRequest });
                                    //回滚，还原前面更新登录中心数据库的用户
                                    //回滚，还原前面更新后台数据库的用户
                                }
                            }
                            //成功
                            return Json(new Result() { success = result.Success, msg = result.Msg, status = result.Status });
                        }
                        else
                        {
                            //失败
                            string errors = "";
                            foreach (var error in identityResult.Errors)
                            {
                                errors = $"{errors}{error.Description} ";
                            }
                            return Json(new Result() { success = false, msg = "更新用户失败：{errors}", status = (int)HttpStatusCode.BadRequest });
                            //回滚，还原前面更新后台数据库的用户
                        }
                    }
                    else
                    {
                        //失败
                        return Json(new Result() { success = false, msg = "更新用户失败", status = (int)HttpStatusCode.BadRequest });
                        //回滚，还原前面更新后台数据库的用户
                    }
                }
                //失败
                return Json(new Result() { success = result.Success, msg = result.Msg, status = result.Status });
            }
            //失败
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
                if (result.Success)
                {
                    var identityUser = await _userManager.FindByNameAsync(userDto.Data.Name);
                    if (identityUser != null)
                    {
                        var identityResult = await _userManager.DeleteAsync(identityUser);
                        if (identityResult.Succeeded)
                        {
                            //成功
                            return Json(new Result() { success = result.Success, msg = result.Msg, status = result.Status });
                        }
                        else
                        {
                            //失败
                            string errors = "";
                            foreach (var error in identityResult.Errors)
                            {
                                errors = $"{errors}{error.Description} ";
                            }
                            return Json(new Result() { success = false, msg = "删除用户失败：{errors}", status = (int)HttpStatusCode.BadRequest });
                            //回滚，还原前面删除后台数据库的用户
                        }
                    }
                    else
                    {
                        //失败
                        return Json(new Result() { success = false, msg = "查找不到要删除的用户", status = (int)HttpStatusCode.NotFound });
                        //回滚，还原前面删除后台数据库的用户
                    }
                }
                //失败
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
