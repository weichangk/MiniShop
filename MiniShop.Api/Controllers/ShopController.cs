using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MiniShop.Api.Services;
using MiniShop.Dto;
using MiniShop.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniShop.Api.Controllers
{
    [Authorize]
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
            var shops = await _shopService.Select(s=> s.Id != null).ToListAsync();
            if (shops == null || shops.Count() <= 0)
            {
                return NotFound("没有找到商店数据");
            }
            var shopInfos = _mapper.Map<IEnumerable<ShopInfoDto>>(shops);
            return Ok(shopInfos);
        }

        [HttpGet("{shopId}", Name = "GetShopByShopId")]
        public async Task<IActionResult> GetShopByShopId(Guid shopId)
        {
            var shop = await _shopService.Select(s => s.Id == shopId).FirstOrDefaultAsync();
            if (shop == null)
            {
                return NotFound($"没有找到商店{shopId}");
            }
            return Ok(shop);
        }

        [HttpPost]
        public async Task<IActionResult> CreateShop([FromBody] ShopCreateDto shopCreateDto)
        {
            var shop = _mapper.Map<Shop>(shopCreateDto);
            shop.CreateDate = DateTime.Now;
            shop.ValidDate = DateTime.Now.AddDays(1);

            var newShop = _shopService.Insert(shop);
            await _shopService.SaveAsync();
            return CreatedAtRoute("GetShopByShopId", new { shopId = shop.Id }, shop);
        }

    }
}
