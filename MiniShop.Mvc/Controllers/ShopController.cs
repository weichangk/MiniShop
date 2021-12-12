using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MiniShop.Dto;
using MiniShop.Mvc.HttpApis;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MiniShop.Mvc.Controllers
{
    public class ShopController : BaseController
    {
        private readonly IShopApi _shopApi;
        public ShopController(ILogger<ShopController> logger, IShopApi shopApi) : base(logger)
        {
            _shopApi = shopApi;
        }

        [HttpGet]
        public async Task<IActionResult> EditAsync()
        {
            var id = User.Claims.FirstOrDefault(s => s.Type == "LoginShopId")?.Value;
            Guid loginShopId = Guid.Parse(id);
            var result = await _shopApi.GetShopByShopId(loginShopId);
            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> SaveAsync(ShopDto model)
        {
            if (ModelState.IsValid)
            {
                var result = await _shopApi.UpdateShop(model);
            }
            return View("Edit", model);
        }
    }
}
