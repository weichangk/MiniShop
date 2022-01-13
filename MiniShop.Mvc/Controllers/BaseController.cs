using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MiniShop.Mvc.Code;

namespace MiniShop.Mvc.Controllers
{
    [Authorize]
    [ServiceFilter(typeof(RefreshAccessTokenFilter))]
    public class BaseController : Controller
    {
        protected readonly ILogger<BaseController> _logger;
        protected readonly IMapper _mapper;
        protected readonly IUserInfo _userInfo;
        public BaseController(ILogger<BaseController> logger, IMapper mapper, IUserInfo userInfo)
        {
            _logger = logger;
            _mapper = mapper;
            _userInfo = userInfo;
        }
    }
}
