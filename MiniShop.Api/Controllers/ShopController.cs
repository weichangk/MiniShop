using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MiniShop.Dto;
using MiniShop.IServices;
using MiniShop.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace MiniShop.Api.Controllers
{
    [Description("商店信息")]
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

        [Description("获取所有商店")]
        [HttpGet]
        public async Task<IActionResult> GetShops()
        {
            _logger.LogDebug("获取所有商店");
            var shops = await _shopService.Select(s=> s.Id != null).ToListAsync();
            if (shops == null || shops.Count() <= 0)
            {
                return NotFound("没有找到商店数据");
            }
            var shopInfos = _mapper.Map<IEnumerable<ShopDto>>(shops);
            return Ok(shopInfos);
        }

        [Description("根据商店ID，返回商店信息")]
        [HttpGet("{shopId}")]
        public async Task<IActionResult> GetShopByShopId(Guid shopId)
        {
            _logger.LogDebug($"根据商店ID:{shopId}获取指定部门");
            var shop = await _shopService.Select(s => s.Id == shopId).FirstOrDefaultAsync();
            if (shop == null)
            {
                return NotFound($"没有找到商店{shopId}");
            }
            var shopInfo = _mapper.Map<ShopDto>(shop);
            return Ok(shopInfo);
        }

        [Description("修改商店")]
        [HttpPut]
        public async Task<IActionResult> UpdateShop([FromBody] ShopDto shopDto)
        {
            _logger.LogDebug("修改商店");
            var shop = _mapper.Map<Shop>(shopDto);
            _shopService.Update(shop);
            await _shopService.SaveAsync();
            return Ok(shopDto);
        }

    }
}
