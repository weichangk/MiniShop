using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MiniShop.IServices;
using System;

namespace MiniShop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategorieController : ControllerAbstract
    {
        private readonly IMapper _mapper;
        private readonly ICategorieService _categorieServicr;
        public CategorieController(ILogger<ControllerAbstract> logger, IMapper mapper, ICategorieService categorieService) : base(logger)
        {
            _mapper = mapper;
            _categorieServicr = categorieService;
        }
    }
}
