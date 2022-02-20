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
using Orm.Core.Result;
using AutoMapper;
using MiniShop.Mvc.Code;

namespace MiniShop.Mvc.Controllers
{
    public class StoreController : BaseController
    {
        private readonly IStoreApi _storeApi;

        public StoreController(ILogger<StoreController> logger, IMapper mapper, IUserInfo userInfo, IStoreApi storeApi) : base(logger, mapper, userInfo)
        {
            _storeApi = storeApi;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Add()
        {
            StoreCreateDto model = new StoreCreateDto 
            { 
                ShopId = _userInfo.ShopId,
                StoreId = Guid.NewGuid(),
                CreatedTime = DateTime.Now,
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync(StoreCreateDto model)
        {
            var result = await ExecuteApiResultModelAsync(() => { return _storeApi.AddAsync(model); });
            return Json(new Result() { Success = result.Success, Msg = result.Msg, Status = result.Status });
        }

        public async Task<IActionResult> UpdateAsync(int id)
        {
            var result = await ExecuteApiResultModelAsync(() => { return _storeApi.GetByIdAsync(id); });
            if (result.Success)
            {
                return View(result.Data);
            }
            return Json(new Result() { Success = result.Success, Msg = result.Msg, Status = result.Status });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAsync(StoreDto model)
        {
            var dto = _mapper.Map<StoreUpdateDto>(model);
            var result = await ExecuteApiResultModelAsync(() => { return _storeApi.UpdateAsync(dto); });
            return Json(new Result() { Success = result.Success, Msg = result.Msg, Status = result.Status });
        }

        [ResponseCache(Duration = 0)]
        [HttpGet]
        public async Task<IActionResult> GetPageOnShopAsync(int page, int limit)
        {
            var result = await ExecuteApiResultModelAsync(() => { return _storeApi.GetPageOnShopAsync(page, limit, _userInfo.ShopId); });
            if (!result.Success)
            {
                return Json(new Result() { Success = result.Success, Status = result.Status, Msg = result.Msg });
            }
            return Json(new Table() { Data = result.Data.Item, Count = result == null ? 0 : result.Data.Total });
        }

        [ResponseCache(Duration = 0)]
        [HttpGet]
        public async Task<IActionResult> GetPageOnShopWhereQueryNameAsync(int page, int limit, string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                name = " ";
            }
            else
            {
                name = System.Web.HttpUtility.UrlEncode(name);
            }
            var result = await ExecuteApiResultModelAsync(() => { return _storeApi.GetPageOnShopWhereQueryNameAsync(page, limit, _userInfo.ShopId, name); });
            if (!result.Success)
            {
                return Json(new Result() { Success = result.Success, Status = result.Status, Msg = result.Msg });
            }
            return Json(new Table() { Data = result.Data.Item, Count = result == null ? 0 : result.Data.Total });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var storeDto = await ExecuteApiResultModelAsync(() => { return _storeApi.GetByIdAsync(id); });
            if (!storeDto.Success)
            {
                return Json(new Result() { Success = storeDto.Success, Status = storeDto.Status, Msg = storeDto.Msg });
            }
            if (storeDto.Data != null)
            {
                if (storeDto.Data.StoreId == _userInfo.ShopId)
                {
                    return Json(new Result() { Success = false, Msg = $"不能删除总部门店：{storeDto.Data.Name}" });
                }
                var result = await ExecuteApiResultModelAsync(() => { return _storeApi.DeleteAsync(id); });
                return Json(new Result() { Success = result.Success, Msg = result.Msg, Status = result.Status });
            }
            else
            {
                return Json(new Result() { Success = false, Msg = "查找不到要删除的门店", Status = (int)HttpStatusCode.NotFound });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> BatchDeleteAsync(string ids)
        {
            List<string> idsStrList = ids.Split(",").ToList();
            List<int> idsIntList = new List<int>();
            ResultModel<StoreDto> resultModel = new ResultModel<StoreDto>();
            foreach (var id in idsStrList)
            {
                resultModel = await ExecuteApiResultModelAsync(() => { return _storeApi.GetByIdAsync(int.Parse(id)); });
                if (!resultModel.Success)
                {
                    return Json(new Result() { Success = resultModel.Success, Status = resultModel.Status, Msg = resultModel.Msg });
                }
                if (resultModel.Data != null)
                {
                    idsIntList.Add(int.Parse(id));
                    if (resultModel.Data.StoreId == _userInfo.ShopId)
                    {
                        return Json(new Result() { Success = false,Msg = $"不能删除总部门店：{resultModel.Data.Name}" });
                    }
                }
            }

            if (idsIntList.Count > 0)
            {
                var result = await ExecuteApiResultModelAsync(() => { return _storeApi.BatchDeleteAsync(idsIntList); });
                return Json(new Result() { Success = result.Success, Msg = result.Msg, Status = result.Status });
            }
            else
            {
                return Json(new Result() { Success = false, Msg = "查找不到要删除的门店", Status = (int)HttpStatusCode.NotFound });
            }

        }
    }
}
