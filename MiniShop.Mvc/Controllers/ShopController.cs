using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MiniShop.Dto;
using MiniShop.Mvc.Code;
using MiniShop.Mvc.HttpApis;
using System;
using System.Linq;
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
        public async Task<IActionResult> EditAsync()
        {
            var id = User.Claims.FirstOrDefault(s => s.Type == "LoginShopId")?.Value;
            Guid loginShopId = Guid.Parse(id);
            var result = await _shopApi.QueryAsync(loginShopId);
            if (!result.Success)
            {
                return View();
            }
            return View(result.Data);
        }

        [HttpPost]
        public async Task<IActionResult> SaveAsync(ShopDto model)
        {
            //if (ModelState.IsValid)
            //{
            //    var result = await _shopApi.UpdateAsync(model);
            //    if (result.Success)
            //    {
            //        //return RedirectToAction("ShowMsg", "Home", new { msg = "修改成功", json = JsonHelper.SerializeJSON(result.Data) });
            //    }
            //    else
            //    {
            //        if (result.Errors.Count > 0)
            //        {
            //            ModelState.AddModelError(result.Errors[0].Id, result.Errors[0].Msg);
            //        }
            //        else
            //        {
            //            ModelState.AddModelError("error", result.Msg);
            //        }
            //    }
            //}
            return View("Edit", model);
        }
    }
}
