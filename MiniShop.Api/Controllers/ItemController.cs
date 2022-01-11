using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MiniShop.IServices;

namespace MiniShop.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerAbstract
    {
        private readonly IMapper _mapper;
        private readonly IShopService _shopService;
        private readonly ICategorieService _categorieService;
        private readonly IItemService _itemService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        /// <param name="shopService"></param>
        /// <param name="categorieService"></param>
        /// <param name="itemService"></param>
        public ItemController(ILogger<ItemController> logger, IMapper mapper,
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
    }
}
