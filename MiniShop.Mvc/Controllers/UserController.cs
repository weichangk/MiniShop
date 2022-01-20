using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MiniShop.Dto;
using MiniShop.Mvc.HttpApis;
using MiniShop.Mvc.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using System.Net;
using System.Collections.Generic;
using MiniShop.Model.Enums;
using AutoMapper;
using MiniShop.Mvc.Code;
using MiniShop.Model.Code;
using Orm.Core.Result;
using Orm.Core;
using System;

namespace MiniShop.Mvc.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserApi _userApi;
        private readonly IStoreApi _storeApi;

        public UserController(ILogger<UserController> logger, IMapper mapper, IUserInfo userInfo,
            IUserApi userApi, IStoreApi storeApi) : base(logger, mapper, userInfo)
        {
            _userApi = userApi;
            _storeApi = storeApi;
        }

        public IActionResult Index()
        {
            ViewBag.CurrentRank = "";
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetStoresByCurrentShopAsync()
        {
            var result = await _storeApi.GetByShopIdAsync(_userInfo.ShopId);
            if (result.Data != null)
            {
                List<dynamic> rankSelect = new List<dynamic>();
                foreach (var item in result.Data)
                {
                    var op = new { opValue = item.StoreId, opName = item.Name };
                    rankSelect.Add(op);
                }
                return Json(new Result() { Success = true, Data = rankSelect });
            }
            return Json(new Result() { Success = false });
        }

        [HttpGet]
        public IActionResult GetRankScopeByCurrentRankForAddUser()
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
        public IActionResult GetRankScopeByCurrentRankForUpdateUser()
        {
            List<dynamic> rankSelect = new List<dynamic>();
            var shopManagerOp = new { opValue = EnumRole.ShopManager.ToString(), opName = EnumRole.ShopManager.ToDescription() };
            var shopAssistantOp = new { opValue = EnumRole.ShopAssistant.ToString(), opName = EnumRole.ShopAssistant.ToDescription() };
            var storeManagerOp = new { opValue = EnumRole.StoreManager.ToString(), opName = EnumRole.StoreManager.ToDescription() };
            var storeAssistantOp = new { opValue = EnumRole.StoreAssistant.ToString(), opName = EnumRole.StoreAssistant.ToDescription() };
            var cashierOp = new { opValue = EnumRole.Cashier.ToString(), opName = EnumRole.Cashier.ToDescription() };
            switch (_userInfo.Rank)
            {
                case EnumRole.ShopManager:
                    rankSelect.Add(shopManagerOp);
                    rankSelect.Add(shopAssistantOp);
                    rankSelect.Add(storeManagerOp);
                    rankSelect.Add(storeAssistantOp);
                    rankSelect.Add(cashierOp);
                    break;
                case EnumRole.ShopAssistant:
                    rankSelect.Add(shopAssistantOp);
                    rankSelect.Add(storeManagerOp);
                    rankSelect.Add(storeAssistantOp);
                    rankSelect.Add(cashierOp);
                    break;
                case EnumRole.StoreManager:
                    rankSelect.Add(storeManagerOp);
                    rankSelect.Add(storeAssistantOp);
                    rankSelect.Add(cashierOp);
                    break;
                case EnumRole.StoreAssistant:
                    rankSelect.Add(storeAssistantOp);
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
        public IActionResult GetRankScopeByCurrentRankForQueryUser()
        {
            List<dynamic> rankSelect = new List<dynamic>();
            var shopManagerOp = new { opValue = EnumRole.ShopManager.ToString(), opName = EnumRole.ShopManager.ToDescription() };
            var shopAssistantOp = new { opValue = EnumRole.ShopAssistant.ToString(), opName = EnumRole.ShopAssistant.ToDescription() };
            var storeManagerOp = new { opValue = EnumRole.StoreManager.ToString(), opName = EnumRole.StoreManager.ToDescription() };
            var storeAssistantOp = new { opValue = EnumRole.StoreAssistant.ToString(), opName = EnumRole.StoreAssistant.ToDescription() };
            var cashierOp = new { opValue = EnumRole.Cashier.ToString(), opName = EnumRole.Cashier.ToDescription() };
            switch (_userInfo.Rank)
            {
                case EnumRole.ShopManager:
                    rankSelect.Add(shopManagerOp);
                    rankSelect.Add(shopAssistantOp);
                    rankSelect.Add(storeManagerOp);
                    rankSelect.Add(storeAssistantOp);
                    rankSelect.Add(cashierOp);
                    break;
                case EnumRole.ShopAssistant:
                    rankSelect.Add(shopAssistantOp);
                    rankSelect.Add(storeManagerOp);
                    rankSelect.Add(storeAssistantOp);
                    rankSelect.Add(cashierOp);
                    break;
                case EnumRole.StoreManager:
                    rankSelect.Add(storeManagerOp);
                    rankSelect.Add(storeAssistantOp);
                    rankSelect.Add(cashierOp);
                    break;
                case EnumRole.StoreAssistant:
                    rankSelect.Add(storeAssistantOp);
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
            UserCreateDto model = new UserCreateDto 
            { 
                ShopId = _userInfo.ShopId,
                StoreId = _userInfo.StoreId,
                //Rank = EnumRole.Cashier, 
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync(UserCreateDto model)
        {
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
            var result = await _userApi.PutUpdateAsync(dto);
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
                var result = await _userApi.PatchUpdateByNameAsync(name, doc);
                return Json(new Result() { Success = result.Success, Msg = result.Msg, Status = result.Status });
            }
            else
            {
                return Json(new Result() { Success = false, Msg = "查找不到要修改的用户", Status = (int)HttpStatusCode.NotFound });
            }
        }

        [ResponseCache(Duration = 0)]
        [HttpGet]
        public async Task<IActionResult> GetPageAsync(int page, int limit)
        {
            ResultModel<PagedList<UserDto>> result = new ResultModel<PagedList<UserDto>>();
            switch (_userInfo.Rank)
            {
                case EnumRole.ShopManager:
                case EnumRole.ShopAssistant:
                    result = await _userApi.GetPageByRankOnShopAsync(page, limit, _userInfo.ShopId, _userInfo.Rank);
                    break;
                case EnumRole.StoreManager:
                case EnumRole.StoreAssistant:
                case EnumRole.Cashier:
                    result = await _userApi.GetPageByRankOnStoreAsync(page, limit, _userInfo.ShopId, _userInfo.StoreId, _userInfo.Rank);
                    break;
            }
            return Json(new Table() { Data = result.Data.Item, Count = result == null ? 0 : result.Data.Total });
        }

        [ResponseCache(Duration = 0)]
        [HttpGet]
        public async Task<IActionResult> GetPageWhereQueryAsync(int page, int limit, Guid? store, EnumRole? rank, string name, string phone)
        {
            name = System.Web.HttpUtility.UrlEncode(name);
            phone = System.Web.HttpUtility.UrlEncode(phone);

            ResultModel<PagedList<UserDto>> result = new ResultModel<PagedList<UserDto>>();
            switch (_userInfo.Rank)
            {
                case EnumRole.ShopManager:
                case EnumRole.ShopAssistant:
                    result = await _userApi.GetPageByRankOnShopWhereQueryStoreOrRankOrNameOrPhoneAsync(page, limit, _userInfo.ShopId, _userInfo.Rank, store, rank, name, phone);
                    break;
                case EnumRole.StoreManager:
                case EnumRole.StoreAssistant:
                case EnumRole.Cashier:
                    result = await _userApi.GetPageByRankOnShopWhereQueryStoreOrRankOrNameOrPhoneAsync(page, limit, _userInfo.ShopId, _userInfo.Rank, _userInfo.StoreId, rank, name, phone);
                    break;
                default:
                    break;
            }
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

                var result = await _userApi.DeleteByNameAsync(name);
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

            var result = await _userApi.BatchDeleteByNamesAsync(namesList);
            return Json(new Result() { Success = result.Success, Msg = result.Msg, Status = result.Status });
        }

    }


    //备注：
    //一个商店只有一个老板（在注册时创建），可以有多个老板助理（所属门店应为总部，即门店id=商店id）；一个商店可以有多个门店，一个门店只有一个店长（要做唯一处理），可以有多个店长助理，可以有多个收银员
    //老板和老板助理职位能查全部商店所有门店用户，其他职位用户查所在当前门店下该职位以下（包含该职位）的所有用户。  
    //职位的管理范围是该职位下的所有职位，如添加修改删除用户只能操作该职位以下的所有职位用户（同职位用户不能修改删除）
    //在用户列表中按条件搜索用户时，是在该用户职位下可见的用户进行搜索，所以按职位搜索时，搜索的职位条件不可以大于该用户职位。

}
