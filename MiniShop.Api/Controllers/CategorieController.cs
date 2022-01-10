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
    public class CategorieController : ControllerAbstract
    {
        private readonly IMapper _mapper;
        private readonly ICategorieService _categorieServicr;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        /// <param name="categorieService"></param>
        public CategorieController(ILogger<ControllerAbstract> logger, IMapper mapper, ICategorieService categorieService) : base(logger)
        {
            _mapper = mapper;
            _categorieServicr = categorieService;
        }
    }
}
