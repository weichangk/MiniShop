using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MiniShop.Mvc.HttpApis;
using System;
using System.Collections.Generic;
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
        public async Task<IActionResult> Index()
        {        
            var result = await _shopApi.GetShops();
            return View(result);
        }
    }
}
