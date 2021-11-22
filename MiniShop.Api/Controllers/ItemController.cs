using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MiniShop.Api.Services;
using MiniShop.Dto;
using MiniShop.Model;
using System;
using System.Threading.Tasks;

namespace MiniShop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerAbstract
    {
        private readonly IMapper _mapper;
        private readonly IShopService _shopService;
        private readonly ICategorieService _categorieService;
        private readonly IItemService _itemService;
        public ItemController(ILogger<ControllerAbstract> logger, IMapper mapper,
        IShopService shopService,
        ICategorieService categorieService,
        IItemService itemService
        ) : base(logger)
        {
            _mapper = mapper;
            _shopService = shopService;
            _categorieService = categorieService;
            _itemService = itemService;
        }

        [HttpGet("{shopId}", Name = "GetItemByShopId")]
        public async Task<IActionResult> GetItemByShopId(Guid shopId)
        {
            var items = await _itemService.Select(i => i.ShopId == shopId).ToListAsync();
            if (items == null || items.Count <= 0)
            {
                return NotFound($"商店{shopId} 无商品数据");
            }
            return Ok(items);
        }

        [HttpGet("{shopId}/{categorieId}", Name = "GetItemByShopIdAndCategorieId")]
        public async Task<IActionResult> GetItemByShopIdAndCategorieId(Guid shopId, int categorieId)
        {
            var items = await _itemService.Select(i => i.ShopId == shopId && i.CategorieId.Equals(categorieId)).ToListAsync();
            if (items == null)
            {
                return NotFound($"商店{shopId} 商品类别{categorieId} 无商品数据");
            }
            return Ok(items);
        }

        [HttpPost]
        public async Task<IActionResult> CreateItemByShopIdAndCategorieId([FromBody] ItemCreateDto itemCreateDto)
        {
            var shop = await _shopService.Select(s => s.Id.Equals(itemCreateDto.ShopId)).FirstOrDefaultAsync();
            if (shop == null)
            {
                return NotFound($"没有找到商店{itemCreateDto.ShopId}");
            }
            var categorie = await _categorieService.Select(c => c.Id.Equals(itemCreateDto.CategorieId)).FirstOrDefaultAsync();
            if (categorie == null)
            {
                return NotFound($"没有找到类别{itemCreateDto.CategorieId}");
            }

            var itemModel = _mapper.Map<Item>(itemCreateDto);

            _itemService.Insert(itemModel);
            await _itemService.SaveAsync();
            return CreatedAtRoute("GetItemByShopIdAndCategorieId", new { shopId = itemCreateDto.ShopId, categorieId = itemCreateDto.CategorieId }, itemModel);
        }

    }
}
