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
using MiniShop.Model.Code;

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

        [HttpGet]
        public IActionResult GetRankScope()
        {
            List<dynamic> rankSelect = new List<dynamic>();
            var shopAssistantOp = new { opValue = EnumRole.ShopAssistant.ToString(), opName = EnumRole.ShopAssistant.ToDescription() };
            var storeManagerOp = new { opValue = EnumRole.StoreManager.ToString(), opName = EnumRole.StoreManager.ToDescription() };
            var storeAssistantOp = new { opValue = EnumRole.StoreAssistant.ToString(), opName = EnumRole.StoreAssistant.ToDescription() };
            var cashierOp = new { opValue = EnumRole.Cashier.ToString(), opName = EnumRole.Cashier.ToDescription() };
            switch (_userInfo.Rank)
            {          
                case EnumRole.ShopManager:
                    rankSelect.Add(shopAssistantOp);
                    rankSelect.Add(storeManagerOp);
                    rankSelect.Add(storeAssistantOp);
                    rankSelect.Add(cashierOp);
                    break;
                case EnumRole.ShopAssistant:
                    rankSelect.Add(storeManagerOp);
                    rankSelect.Add(storeAssistantOp);
                    rankSelect.Add(cashierOp);
                    break;
                case EnumRole.StoreManager:
                    rankSelect.Add(storeAssistantOp);
                    rankSelect.Add(cashierOp);
                    break;
                case EnumRole.StoreAssistant:
                    rankSelect.Add(cashierOp);
                    break;
                //case EnumRole.Cashier:
                //    break;
                default:
                    break;
            }
            return Json(new Result() { Success = true, Data = rankSelect });
        }

        [HttpGet]
        public IActionResult Add()
        {
            UserCreateDto model = new UserCreateDto { Rank = EnumRole.Cashier, };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync(UserCreateDto model)
        {
            model.ShopId = _userInfo.ShopId;
            var result = await _userApi.AddAsync(model);
            return Json(new Result() { Success = result.Success, Msg = result.Msg, Status = result.Status });
        }

        [HttpGet]
        public async Task<IActionResult> UpdateAsync(string name)
        {
            var result =  await _userApi.GetByNameAsync(name);
            if (result.Success)
            {
                return View(result.Data);
            }
            return Json(new Result() { Success = result.Success, Msg = result.Msg, Status = result.Status });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAsync(UserDto model)
        {
            var dto = _mapper.Map<UserUpdateDto>(model);
            var result = await _userApi.UpdateAsync(dto);
            return Json(new Result() { Success = result.Success, Msg = result.Msg, Status = result.Status });
        }

        [HttpPatch]
        public async Task<IActionResult> ChangeFreezeStateAsync(string name, bool enable)
        {
            var userDto = await _userApi.GetByNameAsync(name);
            if (userDto.Data != null)
            {
                if (userDto.Data.Rank == EnumRole.ShopManager)
                {
                    return Json(new Result() { Success = false, Msg = "不能禁用老板" });
                }
                if (userDto.Data.UserName == _userInfo.UserName)
                {
                    return Json(new Result() { Success = false, Msg = $"不能禁用当前登录用户：{userDto.Data.UserName}" });
                }
                var doc = new JsonPatchDocument<UserUpdateDto>();
                doc.Replace(item => item.IsFreeze, enable);
                var result = await _userApi.PatchUpdateAsync(name, doc);
                return Json(new Result() { Success = result.Success, Msg = result.Msg, Status = result.Status });
            }
            else
            {
                return Json(new Result() { Success = false, Msg = "查找不到要修改的用户", Status = (int)HttpStatusCode.NotFound });
            }
        }

        [ResponseCache(Duration = 0)]
        [HttpGet]
        public async Task<IActionResult> GetPageByShopIdAsync(int page, int limit)
        {
            var result = await _userApi.GetPageByShopIdAsync(page, limit, _userInfo.ShopId);
            return Json(new Table() { Data = result.Data.Item, Count = result == null ? 0 : result.Data.Total });
        }

        [ResponseCache(Duration = 0)]
        [HttpGet]
        public async Task<IActionResult> GetPageByShopIdAndWhereQueryAsync(int page, int limit, string name, string phone, string rank)
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

            var result = await _userApi.GetPageByShopIdAndWhereQueryAsync(page, limit, _userInfo.ShopId, name, phone, rank);
            return Json(new Table() { Data = result.Data.Item, Count = result == null ? 0 : result.Data.Total });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(string name)
        {
            var userDto = await _userApi.GetByNameAsync(name);
            if (userDto.Data != null)
            {
                if (userDto.Data.Rank == EnumRole.ShopManager)
                {
                    return Json(new Result() { Success = false, Msg = $"不能删除老板：{userDto.Data.UserName}" });
                }
                if (userDto.Data.UserName == _userInfo.UserName)
                {
                    return Json(new Result() { Success = false, Msg = $"不能删除当前登录用户：{userDto.Data.UserName}" });
                }

                var result = await _userApi.DeleteAsync(name);
                return Json(new Result() { Success = result.Success, Msg = result.Msg, Status = result.Status });
            }
            else
            {
                return Json(new Result() { Success = false, Msg = "查找不到要删除的用户", Status = (int)HttpStatusCode.NotFound });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> BatchDeleteAsync(string names)
        {
            List<string> namesList = names.Split(",").ToList();
            foreach (var item in namesList)
            {
                var userDto = await _userApi.GetByNameAsync(item);
                if (userDto.Data != null)
                {
                    if (userDto.Data.Rank == EnumRole.ShopManager)
                    {
                        return Json(new Result() { Success = false, Msg = $"不能删除老板：{userDto.Data.UserName}" });
                    }
                    if (userDto.Data.UserName == _userInfo.UserName)
                    {
                        return Json(new Result() { Success = false, Msg = $"不能删除当前登录用户：{userDto.Data.UserName}" });
                    }
                }
                else
                {
                    return Json(new Result() { Success = false, Msg = "查找不到要删除的用户", Status = (int)HttpStatusCode.NotFound });
                }
            }

            var result = await _userApi.BatchDeleteAsync(namesList);
            return Json(new Result() { Success = result.Success, Msg = result.Msg, Status = result.Status });
        }

    }
}
