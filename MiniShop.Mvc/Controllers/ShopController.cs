using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MiniShop.Dto;
using MiniShop.Mvc.Code;
using MiniShop.Mvc.HttpApis;
using MiniShop.Mvc.Models;
using System;
using System.Net;
using System.Threading.Tasks;

namespace MiniShop.Mvc.Controllers
{
    public class ShopController : BaseController
    {
        private readonly IShopApi _shopApi;
        public ShopController(ILogger<ShopController> logger, IMapper mapper, IUserInfo userInfo, IShopApi shopApi) : base(logger, mapper, userInfo)
        {
            _shopApi = shopApi;
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
            var result = await _shopApi.PutUpdateAsync(dto);
            return Json(new Result() { Success = result.Success, Msg = result.Msg, Status = result.Status });
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
