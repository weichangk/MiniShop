using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MiniShop.Mvc.Code;

namespace MiniShop.Mvc.Controllers
{
    [AllowAnonymous]
    public class ErrorController : BaseController
    {
        public ErrorController(ILogger<ErrorController> logger, IMapper mapper, IUserInfo userInfo) : base(logger, mapper, userInfo)
        {
        }

        public IActionResult Error500()
        {
            return View();
        }
    }
}
