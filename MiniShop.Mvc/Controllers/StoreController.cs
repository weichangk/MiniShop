using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MiniShop.Dto;
using MiniShop.Mvc.HttpApis;
using MiniShop.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using yrjw.ORM.Chimp.Result;

namespace MiniShop.Mvc.Controllers
{
    public class StoreController : BaseController
    {
        private readonly IStoreApi _storeApi;

        public StoreController(ILogger<StoreController> logger, IStoreApi storeApi) : base(logger)
        {
            _storeApi = storeApi;
        }

        private async Task<bool> UniqueStoreNameByShopId(Guid shopId, string name)
        {
            var existStore = await _storeApi.QueryAsyncByName(shopId, name);
            if (existStore.Data != null)
            {
                return false;
            }
            return true;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            StoreCreateDto model = new StoreCreateDto();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SaveAddAsync(StoreCreateDto model)
        {
            if (ModelState.IsValid)
            {
                var id = User.Claims.FirstOrDefault(s => s.Type == "LoginShopId")?.Value;
                Guid loginShopId = Guid.Parse(id);
                model.ShopId = loginShopId;

                if (!await UniqueStoreNameByShopId(loginShopId, model.Name))
                {
                    return Json(new Result() { success = false, msg = $"门店名：{model.Name} 已被占用", status = (int)HttpStatusCode.BadRequest });
                }

                var result = await _storeApi.AddAsync(model);
                return Json(new Result() { success = result.Success, msg = result.Msg, status = result.Status });
            }
            return Json(new Result() { success = false, msg = ModelStateErrorMessage(ModelState), status = (int)HttpStatusCode.BadRequest });
        }

        public async Task<IActionResult> Edit(int id)
        {
            var result = await _storeApi.QueryAsync(id);
            if (result.Success)
            {
                return View(result.Data);
            }
            return Json(new Result() { success = result.Success, msg = result.Msg, status = result.Status });
        }

        [HttpPost]
        public async Task<IActionResult> SaveEditAsync(StoreDto model)
        {
            if (ModelState.IsValid)
            {
                var oldStoreDto = await _storeApi.QueryAsync(model.Id);

                var id = User.Claims.FirstOrDefault(s => s.Type == "LoginShopId")?.Value;
                Guid loginShopId = Guid.Parse(id);
                model.ShopId = loginShopId;

                if (oldStoreDto.Data.Name != model.Name && !await UniqueStoreNameByShopId(loginShopId, model.Name))
                {
                    return Json(new Result() { success = false, msg = $"门店名：{model.Name} 已被占用", status = (int)HttpStatusCode.BadRequest });
                }

                var doc = new JsonPatchDocument<StoreUpdateDto>();
                doc.Replace(item => item.Name, model.Name);
                doc.Replace(item => item.Phone, model.Phone);
                doc.Replace(item => item.Contacts, model.Contacts);
                doc.Replace(item => item.Address, model.Address);

                var result = await _storeApi.PatchUpdateAsync(model.Id, doc);
                return Json(new Result() { success = result.Success, msg = result.Msg, status = result.Status });
            }
            return Json(new Result() { success = false, msg = ModelStateErrorMessage(ModelState), status = (int)HttpStatusCode.BadRequest });
        }

        [ResponseCache(Duration = 0)]
        public async Task<IActionResult> GetPageListAsync(int page, int limit)
        {
            var id = User.Claims.FirstOrDefault(s => s.Type == "LoginShopId")?.Value;
            Guid loginShopId = Guid.Parse(id);
            var result = await _storeApi.GetPageListAsync(page, limit, loginShopId);
            return Json(new Table() { data = result.Data.Item, count = result == null ? 0 : result.Data.Total });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var storeDto = await _storeApi.QueryAsync(id);
            if (storeDto.Data != null)
            {
                if (storeDto.Data.Name == "总部门店")
                {
                    return Json(new Result() { success = false, msg = $"不能删除总部门店：{storeDto.Data.Name}" });
                }
                var loginStoreId = User.Claims.FirstOrDefault(s => s.Type == "LoginStoreId")?.Value;
                int storeId = int.Parse(loginStoreId);
                if (storeDto.Data.Id == storeId)
                {
                    return Json(new Result() { success = false, msg = $"不能删除当前登录门店：{storeDto.Data.Name}" });
                }

                var result = await _storeApi.DeleteAsync(id);
                return Json(new Result() { success = result.Success, msg = result.Msg, status = result.Status });
            }
            else
            {
                return Json(new Result() { success = false, msg = "查找不到要删除的门店", status = (int)HttpStatusCode.NotFound });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> BatchDeleteAsync(string ids)
        {
            List<string> idsStrList = ids.Split(",").ToList();
            List<int> idsIntList = new List<int>();
            ResultModel<StoreDto> resultModel = new ResultModel<StoreDto>();
            var loginId = User.Claims.FirstOrDefault(s => s.Type == "LoginId")?.Value;
            int userId = int.Parse(loginId);
            foreach (var id in idsStrList)
            {
                resultModel = await _storeApi.QueryAsync(int.Parse(id));
                if (resultModel.Data != null)
                {
                    idsIntList.Add(int.Parse(id));
                    if (resultModel.Data.Name == "总部门店")
                    {
                        return Json(new Result() { success = false, msg = $"不能删除总部门店：{resultModel.Data.Name}" });
                    }
                    var loginStoreId = User.Claims.FirstOrDefault(s => s.Type == "LoginStoreId")?.Value;
                    int storeId = int.Parse(loginStoreId);
                    if (resultModel.Data.Id == storeId)
                    {
                        return Json(new Result() { success = false, msg = $"不能删除当前登录门店：{resultModel.Data.Name}" });
                    }
                }
            }

            if (idsIntList.Count > 0)
            {
                var result = await _storeApi.BatchDeleteAsync(idsIntList);
                return Json(new Result() { success = result.Success, msg = result.Msg, status = result.Status });
            }
            else
            {
                return Json(new Result() { success = false, msg = "查找不到要删除的门店", status = (int)HttpStatusCode.NotFound });
            }

        }
    }
}
