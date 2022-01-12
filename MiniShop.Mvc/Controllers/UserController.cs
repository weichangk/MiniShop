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
using Orm.Core.Result;
using MiniShop.Model.Enums;

namespace MiniShop.Mvc.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserApi _userApi;

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
            UserCreateDto model = new UserCreateDto { Rank = EnumRole.Admin, };
            return View(model);
        }

        //保存添加用户信息
        [HttpPost]
        public async Task<IActionResult> SaveAddAsync(UserCreateDto model)
        {
            var id = User.Claims.FirstOrDefault(s => s.Type == "LoginShopId")?.Value;
            Guid loginShopId = Guid.Parse(id);
            model.ShopId = loginShopId;
            var result = await _userApi.AddAsync(model);
            return Json(new Result() { success = result.Success, msg = result.Msg, status = result.Status });
        }

        //修改页面
        [HttpGet]
        public async Task<IActionResult> Edit(string name)
        {
            var result =  await _userApi.QueryAsync(name);
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
            var doc = new JsonPatchDocument<UserUpdateDto>();
            doc.Replace(item => item.UserName, model.UserName);
            doc.Replace(item => item.PhoneNumber, model.PhoneNumber);
            doc.Replace(item => item.Email, model.Email);
            doc.Replace(item => item.Rank, model.Rank);

            var result = await _userApi.PatchUpdateAsync(model.UserName, doc);
            return Json(new Result() { success = result.Success, msg = result.Msg, status = result.Status });
        }

        [ResponseCache(Duration = 0)]
        [HttpGet]
        public async Task<IActionResult> GetPageListAsync(int page, int limit)
        {
            var shopId = User.Claims.FirstOrDefault(s => s.Type == "ShopId")?.Value;
            var result = await _userApi.QueryPageListByShopAsync(page, limit, shopId);
            return Json(new Table() { data = result.Data.Item, count = result == null ? 0 : result.Data.Total });
        }

        [ResponseCache(Duration = 0)]
        [HttpGet]
        public async Task<IActionResult> GetPageListAndWhereQueryAsync(int page, int limit, string name, string phone, string role)
        {
            var shopId = User.Claims.FirstOrDefault(s => s.Type == "ShopId")?.Value;
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
                    case "收银员":
                        role = "Cashier";
                        break;
                    default:
                        role = System.Web.HttpUtility.UrlEncode(role);
                        break;
                }
            }

            var result = await _userApi.QueryPageListByShopAndWhereAsync(page, limit, shopId, name, phone, role);
            return Json(new Table() { data = result.Data.Item, count = result == null ? 0 : result.Data.Total });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(string name)
        {
            var userDto = await _userApi.QueryAsync(name);
            if (userDto.Data != null)
            {
                if (userDto.Data.Rank == EnumRole.ShopManager)
                {
                    return Json(new Result() { success = false, msg = $"不能删除店长：{userDto.Data.UserName}" });
                }
                var loginId = User.Claims.FirstOrDefault(s => s.Type == "UserName")?.Value;
                int userId = int.Parse(loginId);
                if (userDto.Data.UserName == loginId)
                {
                    return Json(new Result() { success = false, msg = $"不能删除当前登录用户：{userDto.Data.UserName}" });
                }

                //1. 删除后台数据库的用户
                var result = await _userApi.DeleteAsync(name);
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
