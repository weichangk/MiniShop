using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MiniShop.Mvc.Controllers
{
    [AllowAnonymous]
    public class ErrorController : BaseController
    {
        public ErrorController(ILogger<ErrorController> logger) : base(logger)
        {
        }

        public IActionResult Error500()
        {
            return View();
        }
    }
}
