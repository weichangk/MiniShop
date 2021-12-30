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
using System.Collections.Generic;
using yrjw.ORM.Chimp.Result;

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

                //更新当前登录用户要更新Claim。。。。。暂时不支持修改当前登录用户
                var loginId = User.Claims.FirstOrDefault(s => s.Type == "LoginId")?.Value;
                int userId = int.Parse(loginId);
                if (oldUserDto.Data.Id == userId)
                {
                    return Json(new Result() { success = false, msg = $"不能修改当前登录用户：{oldUserDto.Data.Name}" });
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
                                    //return Json(new Result() { success = result.Success, msg = result.Msg, status = result.Status });
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
                            //更新当前登录用户要更新Claim。。。。。
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
        public async Task<IActionResult> GetPageListAsync(int page, int limit)
        {
            var id = User.Claims.FirstOrDefault(s => s.Type == "LoginShopId")?.Value;
            Guid loginShopId = Guid.Parse(id);
            var result = await _userApi.GetPageListAsync(page, limit, loginShopId);
            return Json(new Table() { data = result.Data.Item, count = result == null ? 0 : result.Data.Total });
        }

        [ResponseCache(Duration = 0)]
        [HttpGet]
        public async Task<IActionResult> GetPageListAndWhereQueryAsync(int page, int limit, string name, string phone, string role)
        {
            var id = User.Claims.FirstOrDefault(s => s.Type == "LoginShopId")?.Value;
            Guid loginShopId = Guid.Parse(id);
            if (string.IsNullOrEmpty(name))
            {
                name = " ";
            }
            else
            {
                name = System.Web.HttpUtility.UrlEncode(name);
            }
            if (string.IsNullOrEmpty(phone))
            {
                phone = " ";
            }
            else
            {
                phone = System.Web.HttpUtility.UrlEncode(phone);
            }
            if (string.IsNullOrEmpty(role))
            {
                role = " ";
            }
            else
            {
                switch (role)
                {
                    case "店长":
                        role = "ShopManager";
                        break;
                    case "管理员":
                        role = "Admin";
                        break;
                    case "操作员":
                        role = "Operator";
                        break;
                    case "收银员":
                        role = "Cashier";
                        break;
                    default:
                        role = System.Web.HttpUtility.UrlEncode(role);
                        break;
                }
            }


            var result = await _userApi.GetPageListAndWhereQueryAsync(page, limit, loginShopId, name, phone, role);
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
                    return Json(new Result() { success = false, msg = $"不能删除店长：{userDto.Data.Name}" });
                }
                var loginId = User.Claims.FirstOrDefault(s => s.Type == "LoginId")?.Value;
                int userId = int.Parse(loginId);
                if (userDto.Data.Id == userId)
                {
                    return Json(new Result() { success = false, msg = $"不能删除当前登录用户：{userDto.Data.Name}" });
                }

                //1. 删除后台数据库的用户
                var result = await _userApi.DeleteAsync(id);
                if (result.Success)
                {
                    //2. 删除登陆中心数据库的用户
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
                        //做回滚不靠谱，后面在想怎么跨库事务处理吧。。。
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

        [HttpDelete]
        public async Task<IActionResult> BatchDeleteAsync(string ids)
        {
            List<string> idsStrList = ids.Split(",").ToList();
            List<int> idsIntList = new List<int>();
            List<UserDto> userDtos = new List<UserDto>();
            ResultModel<UserDto> resultModel = new ResultModel<UserDto>();
            var loginId = User.Claims.FirstOrDefault(s => s.Type == "LoginId")?.Value;
            int userId = int.Parse(loginId);
            foreach (var id in idsStrList)
            {
                resultModel = await _userApi.QueryAsync(int.Parse(id));
                if (resultModel.Data != null)
                {
                    idsIntList.Add(int.Parse(id));
                    userDtos.Add(resultModel.Data);
                    if (resultModel.Data.Role == Model.EnumRole.ShopManager)
                    {
                        return Json(new Result() { success = false, msg = $"不能删除店长：{resultModel.Data.Name}" });
                    }
                    if (resultModel.Data.Id == userId)
                    {
                        return Json(new Result() { success = false, msg = $"不能删除当前登录用户：{resultModel.Data.Name}" });
                    }
                }
            }

            if (idsIntList.Count > 0)
            {
                var result = await _userApi.BatchDeleteAsync(idsIntList);
                if (result.Success)
                {
                    foreach (var item in userDtos)
                    {
                        var identityUser = await _userManager.FindByNameAsync(item.Name);
                        if (identityUser != null)
                        {
                            var identityResult = await _userManager.DeleteAsync(identityUser);
                            if (identityResult.Succeeded)
                            {
                                //成功
                                //return Json(new Result() { success = true, msg = "Success", status = (int)HttpStatusCode.OK });
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
                            //做回滚不靠谱，后面在想怎么跨库事务处理吧。。。
                        }
                    }
                    //成功
                    return Json(new Result() { success = true, msg = "Success", status = (int)HttpStatusCode.OK });

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
                var loginId = User.Claims.FirstOrDefault(s => s.Type == "LoginId")?.Value;
                int userId = int.Parse(loginId);
                if (userDto.Data.Id == userId)
                {
                    return Json(new Result() { success = false, msg = $"不能禁用当前登录用户：{userDto.Data.Name}" });
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
