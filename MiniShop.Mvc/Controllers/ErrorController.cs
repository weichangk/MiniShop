using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MiniShop.Mvc.Code;
using MiniShop.Mvc.Models;

namespace MiniShop.Mvc.Controllers
{
    [AllowAnonymous]
    public class ErrorController : BaseController
    {
        public ErrorController(ILogger<ErrorController> logger, IMapper mapper, IUserInfo userInfo) : base(logger, mapper, userInfo)
        {
        }

        public IActionResult Error(string statusCode, string errorMsg)
        {
            if (string.IsNullOrEmpty(statusCode))
            {
                statusCode = "500";
            }

            return View(new ErrorViewModel(statusCode, errorMsg));
        }
    }
}
