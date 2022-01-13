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
using System.Collections.Generic;
using Orm.Core.Result;
using MiniShop.Model.Enums;
using AutoMapper;
using MiniShop.Mvc.Code;

namespace MiniShop.Mvc.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserApi _userApi;

        public UserController(ILogger<UserController> logger, IMapper mapper, IUserInfo userInfo, IUserApi userApi) : base(logger, mapper, userInfo)
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
            model.ShopId = _userInfo.ShopId;
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
            var dto = _mapper.Map<UserUpdateDto>(model);
            var result = await _userApi.UpdateAsync(dto);
            return Json(new Result() { success = result.Success, msg = result.Msg, status = result.Status });
        }

        [HttpPatch]
        public async Task<IActionResult> ChangeEnableAsync(string name, bool enable)
        {
            var userDto = await _userApi.QueryAsync(name);
            if (userDto.Data != null)
            {
                if (userDto.Data.Rank == EnumRole.ShopManager)
                {
                    return Json(new Result() { success = false, msg = "不能禁用店长" });
                }
                if (userDto.Data.UserName == _userInfo.UserName)
                {
                    return Json(new Result() { success = false, msg = $"不能禁用当前登录用户：{userDto.Data.UserName}" });
                }
                var doc = new JsonPatchDocument<UserUpdateDto>();
                doc.Replace(item => item.IsFreeze, enable);
                var result = await _userApi.PatchUpdateAsync(name, doc);
                return Json(new Result() { success = result.Success, msg = result.Msg, status = result.Status });
            }
            else
            {
                return Json(new Result() { success = false, msg = "查找不到要修改的用户", status = (int)HttpStatusCode.NotFound });
            }
        }

        [ResponseCache(Duration = 0)]
        [HttpGet]
        public async Task<IActionResult> GetPageListAsync(int page, int limit)
        {
            var result = await _userApi.QueryPageListByShopAsync(page, limit, _userInfo.ShopId.ToString());
            return Json(new Table() { data = result.Data.Item, count = result == null ? 0 : result.Data.Total });
        }

        [ResponseCache(Duration = 0)]
        [HttpGet]
        public async Task<IActionResult> GetPageListAndWhereQueryAsync(int page, int limit, string name, string phone, string rank)
        {
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
            if (string.IsNullOrEmpty(rank))
            {
                rank = " ";
            }
            else
            {
                switch (rank)
                {
                    case "店长":
                        rank = "ShopManager";
                        break;
                    case "管理员":
                        rank = "Admin";
                        break;
                    case "收银员":
                        rank = "Cashier";
                        break;
                    default:
                        rank = System.Web.HttpUtility.UrlEncode(rank);
                        break;
                }
            }

            var result = await _userApi.QueryPageListByShopAndWhereAsync(page, limit, _userInfo.ShopId.ToString(), name, phone, rank);
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
                if (userDto.Data.UserName == _userInfo.UserName)
                {
                    return Json(new Result() { success = false, msg = $"不能删除当前登录用户：{userDto.Data.UserName}" });
                }

                var result = await _userApi.DeleteAsync(name);
                return Json(new Result() { success = result.Success, msg = result.Msg, status = result.Status });
            }
            else
            {
                return Json(new Result() { success = false, msg = "查找不到要删除的用户", status = (int)HttpStatusCode.NotFound });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> BatchDeleteAsync(string names)
        {
            List<string> namesList = names.Split(",").ToList();
            var result = await _userApi.BatchDeleteAsync(namesList);
            return Json(new Result() { success = result.Success, msg = result.Msg, status = result.Status });
        }

    }
}
