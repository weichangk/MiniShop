using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MiniShop.Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniShop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShopController : ControllerAbstract
    {
        private readonly IMapper _mapper;
        private readonly IShopService _shopService;
        public ShopController(ILogger<ControllerAbstract> logger, IMapper mapper, IShopService shopService) : base(logger)
        {
            _mapper = mapper;
            _shopService = shopService;
        }

        [HttpGet]
        public async Task<IActionResult> GetShops()
        {
            var shops = await _shopService.Select(t => t.Id >= 1).ToListAsync();
            if (shops == null || shops.Count() <= 0)
            {
                return NotFound("没有找到商店数据");
            }
            return Ok(shops);
        }

        [HttpGet("{shopId}", Name = "GetShopByShopId")]
        public async Task<IActionResult> GetShopByShopId(int shopId)
        {
            var shop = await _shopService.Select(t => t.Id == shopId).FirstOrDefaultAsync();
            if (shop == null)
            {
                return NotFound($"没有找到商店{shopId}数据");
            }
            return Ok(shop);
        }



    }
}
