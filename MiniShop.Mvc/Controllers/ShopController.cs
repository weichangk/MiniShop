using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MiniShop.Mvc.HttpApis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniShop.Mvc.Controllers
{
    public class ShopController : Controller
    {
        private readonly ILogger<ShopController> _logger;
        public readonly IShopApi _shopApi;


        public ShopController(ILogger<ShopController> logger, IShopApi shopApi)
        {
            _logger = logger;
            _shopApi = shopApi;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var result = await _shopApi.GetShops();
            return View(result);
        }
    }
}
