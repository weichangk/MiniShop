using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using MiniShop.Mvc.Code;
using MiniShop.Mvc.HttpApis;
using MiniShop.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MiniShop.Mvc.Controllers
{

    public class HomeController : BaseController
    {
        private readonly IShopApi _shopApi;
        private readonly IUserApi _userApi;
        public HomeController(ILogger<HomeController> logger, IShopApi shopApi, IUserApi userApi) : base(logger)
        {
            _shopApi = shopApi;
            _userApi = userApi;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (!LoginInfos.LoginSuccess)
            {
                var userClaims = User.Claims;
                var jwtPreferredUserName = userClaims.FirstOrDefault(s => s.Type == JwtClaimTypes.PreferredUserName)?.Value;
                var jwtPhoneNumber = userClaims.FirstOrDefault(s => s.Type == JwtClaimTypes.PhoneNumber)?.Value;
                var jwtEmail = userClaims.FirstOrDefault(s => s.Type == JwtClaimTypes.Email)?.Value;
                var jwtRole = userClaims.FirstOrDefault(s => s.Type == JwtClaimTypes.Role)?.Value;
                if (jwtPreferredUserName == null)
                {
                    //
                }
                //防止null值时url中出现//导致接口无法匹配，使用 " "代替null值，才能成功传到api那边且参数为null
                if (jwtPhoneNumber == null) jwtPhoneNumber = " ";
                if (jwtEmail == null) jwtEmail = " ";
                if (jwtRole == null) jwtRole = " ";
                var result = await _userApi.UserLogin(jwtPreferredUserName, jwtPhoneNumber, jwtEmail, jwtRole);
                LoginInfos.LoginSuccess = true;
                LoginInfos.Id = result.Id;
                LoginInfos.ShopId = result.ShopId;
                LoginInfos.Name = result.Name;
                LoginInfos.Phone = result.Phone;
                LoginInfos.Email = result.Email;
                LoginInfos.Role = result.Role;
            }

            return View();
        }

        public async Task<IActionResult> Privacy()
        {
            var accessToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
            var idToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.IdToken);
            var refreshToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);
            ViewData["accessToken"] = accessToken;
            ViewData["idToken"] = idToken;
            ViewData["refreshToken"] = refreshToken;

            ViewData["LoginInfos.Id"] = LoginInfos.Id;
            ViewData["LoginInfos.ShopId"] = LoginInfos.ShopId;
            ViewData["LoginInfos.Name"] = LoginInfos.Name;
            ViewData["LoginInfos.Phone"] = LoginInfos.Phone;
            ViewData["LoginInfos.Email"] = LoginInfos.Email;
            ViewData["LoginInfos.Role"] = LoginInfos.Role;

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
