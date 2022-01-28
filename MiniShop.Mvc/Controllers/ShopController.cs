﻿using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MiniShop.Dto;
using MiniShop.Mvc.Code;
using MiniShop.Mvc.HttpApis;
using MiniShop.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading.Tasks;
using WebApiClientCore.Exceptions;

namespace MiniShop.Mvc.Controllers
{
    public class ShopController : BaseController
    {
        private readonly IShopApi _shopApi;
        private readonly IRenewPackageApi _renewPackageApi;
        public ShopController(ILogger<ShopController> logger, IMapper mapper, IUserInfo userInfo, 
            IShopApi shopApi, IRenewPackageApi renewPackageApi) : base(logger, mapper, userInfo)
        {
            _shopApi = shopApi;
            _renewPackageApi = renewPackageApi;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var result = await _shopApi.GetByShopIdAsync(_userInfo.ShopId);
            if (result.Success && result.Data != null)
            {
                if (result.Data != null)
                {
                    return View(result.Data);
                }
                return Json(new Result() { Success = false, Msg = "商店不存在！", Status = (int)HttpStatusCode.NotFound });
            }
            return Json(new Result() { Success = false, Msg = result.Msg, Status = result.Status });
        }

        [HttpPost]
        public async Task<IActionResult> SaveAsync(ShopDto model)
        {
            var dto = _mapper.Map<ShopUpdateDto>(model);
            try
            {
                var result = await _shopApi.PutUpdateAsync(dto);
                return Json(new Result() { Success = result.Success, Msg = result.Msg, Status = result.Status });
            }
            catch (HttpRequestException ex) when (ex.InnerException is ApiInvalidConfigException configException)
            {
                // 请求配置异常
                return Json(new Result() { Success = false, Msg = configException.Message, Status = 500});
            }
            catch (HttpRequestException ex) when (ex.InnerException is ApiResponseStatusException statusException)
            {
                // 响应状态码异常
                Redirect("Error/Error500");
            }
            catch (HttpRequestException ex) when (ex.InnerException is ApiException apiException)
            {
                // 抽象的api异常
                return Json(new Result() { Success = false, Msg = apiException.Message, Status = 500 });
            }
            catch (HttpRequestException ex) when (ex.InnerException is SocketException socketException)
            {
                // socket连接层异常
                return Json(new Result() { Success = false, Msg = socketException.Message, Status = 500 });
            }
            catch (HttpRequestException ex)
            {
                // 请求异常
                return Json(new Result() { Success = false, Msg = ex.Message, Status = 500 });
            }
            catch (Exception ex)
            {
                return Json(new Result() { Success = false, Msg = ex.Message, Status = 500 });
            }
            return Json(new Result() { Success = false, Msg = "请求异常", Status = 500 });
        }

        [HttpGet]
        public async Task<IActionResult> Renew()
        { 
            var shop = (await _shopApi.GetByShopIdAsync(_userInfo.ShopId)).Data;
            if (shop == null)
            {
                return Json(new Result() { Success = false, Msg = "商店不存在！", Status = (int)HttpStatusCode.NotFound });
            }
            ViewBag.ShopKey = shop.Id;
            ViewBag.ShopValidDate = shop.ValidDate;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetRenews()
        {
            var renews = await _renewPackageApi.GetRenewPackagesAsync();
            List<CardInfo> cards = new List<CardInfo>();
            foreach (var item in renews.Data)
            {
                CardInfo card = new CardInfo
                {
                    Id = item.Id,
                    Title = $"￥{item.Price:F2}元 / {item.Name}",
                    Image = item.Image,
                    Remark = item.Remark,
                    Time = $"{item.Price:F2}元 / {item.Months}月",
                };
                cards.Add(card);
            }
            return Json(new Card() { Count = cards.Count, Data = cards });
        }

        [HttpPatch]
        public async Task<IActionResult> RenewAsync(int id, DateTime validDate, int day)
        {
            var doc = new JsonPatchDocument<ShopUpdateDto>();
            doc.Replace(item => item.ValidDate, validDate.AddDays(day));
            var result = await _shopApi.PatchUpdateByIdAsync(id, doc);
            return Json(new Result() { Success = result.Success, Msg = result.Msg, Status = result.Status });
        }
    }
}
