using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MiniShop.Dto;
using MiniShop.Mvc.Code;
using MiniShop.Mvc.HttpApis;
using MiniShop.Mvc.Models;
using System;
using System.Linq;
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
                return Json(new Result() { Success = false, Msg = "商店信息不存在！", Status = (int)HttpStatusCode.NotFound });
            }
            return Json(new Result() { Success = false, Msg = result.Msg, Status = result.Status });
        }

        [HttpPost]
        public async Task<IActionResult> SaveAsync(ShopDto model)
        {
            var dto = _mapper.Map<ShopUpdateDto>(model);
            var result = await _shopApi.UpdateAsync(dto);
            return Json(new Result() { Success = result.Success, Msg = result.Msg, Status = result.Status });
        }
    }
}
