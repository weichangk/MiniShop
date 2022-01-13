using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MiniShop.Mvc.Code;
using MiniShop.Mvc.Models;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MiniShop.Mvc.Controllers
{

    public class HomeController : BaseController
    {
        public HomeController(ILogger<HomeController> logger, IMapper mapper, IUserInfo userInfo) : base(logger, mapper, userInfo)
        {
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        public async Task Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
        }

        public IActionResult Privacy()
        {
            ViewBag.IdToken = _userInfo.IdToken;
            ViewBag.AccessToken = _userInfo.AccessToken;
            ViewBag.RefreshToken = _userInfo.RefreshToken;
            ViewBag.UserName = _userInfo.UserName;
            ViewBag.Rank = _userInfo.Rank;
            ViewBag.ShopId = _userInfo.ShopId;
            ViewBag.StoreId = _userInfo.StoreId;
            ViewBag.PhoneNumber = _userInfo.PhoneNumber;
            ViewBag.Email = _userInfo.Email;
            ViewBag.IsFreeze = _userInfo.IsFreeze;
            ViewBag.CreatedTime = _userInfo.CreatedTime;

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
