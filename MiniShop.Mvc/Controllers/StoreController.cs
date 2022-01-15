﻿using Microsoft.AspNetCore.Mvc;
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
            var result = await _storeApi.AddAsync(model);
            return Json(new Result() { Success = result.Success, Msg = result.Msg, Status = result.Status });
        }

        public async Task<IActionResult> Update(int id)
        {
            var result = await _storeApi.GetByIdAsync(id);
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
            var result = await _storeApi.UpdateAsync(dto);
            return Json(new Result() { Success = result.Success, Msg = result.Msg, Status = result.Status });
        }

        [ResponseCache(Duration = 0)]
        public async Task<IActionResult> GetPageByShopIdAsync(int page, int limit)
        {
            var result = await _storeApi.GetPageByShopIdAsync(page, limit, _userInfo.ShopId);
            return Json(new Table() { Data = result.Data.Item, Count = result == null ? 0 : result.Data.Total });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var storeDto = await _storeApi.GetByIdAsync(id);
            if (storeDto.Data != null)
            {
                if (storeDto.Data.StoreId == _userInfo.ShopId)
                {
                    return Json(new Result() { Success = false, Msg = $"不能删除总部门店：{storeDto.Data.Name}" });
                }
                var result = await _storeApi.DeleteAsync(id);
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
                resultModel = await _storeApi.GetByIdAsync(int.Parse(id));
                if (resultModel.Data != null)
                {
                    idsIntList.Add(int.Parse(id));
                    if (resultModel.Data.Name == "总部门店")
                    {
                        return Json(new Result() { Success = false,Msg = $"不能删除总部门店：{resultModel.Data.Name}" });
                    }
                    var loginStoreId = User.Claims.FirstOrDefault(s => s.Type == "LoginStoreId")?.Value;
                    Guid storeId = Guid.Parse(loginStoreId);
                    if (resultModel.Data.StoreId == storeId)
                    {
                        return Json(new Result() { Success = false, Msg = $"不能删除当前登录门店：{resultModel.Data.Name}" });
                    }
                }
            }

            if (idsIntList.Count > 0)
            {
                var result = await _storeApi.BatchDeleteAsync(idsIntList);
                return Json(new Result() { Success = result.Success, Msg = result.Msg, Status = result.Status });
            }
            else
            {
                return Json(new Result() { Success = false, Msg = "查找不到要删除的门店", Status = (int)HttpStatusCode.NotFound });
            }

        }
    }
}
