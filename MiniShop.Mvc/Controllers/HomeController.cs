using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using MiniShop.Mvc.HttpApis;
using MiniShop.Mvc.Models;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
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
            await SaveLoginInfo();
            return View();
        }

        public IActionResult Privacy()
        {
            //var accessToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
            //var idToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.IdToken);
            //var refreshToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);
            //ViewData["accessToken"] = accessToken;
            //ViewData["idToken"] = idToken;
            //ViewData["refreshToken"] = refreshToken;

            //ViewData["LoginIdToken"] = User.Claims.FirstOrDefault(s => s.Type == "LoginIdToken")?.Value;
            //ViewData["LoginAccessToken"] = User.Claims.FirstOrDefault(s => s.Type == "LoginAccessToken")?.Value;
            //ViewData["LoginRefreshToken"] = User.Claims.FirstOrDefault(s => s.Type == "LoginRefreshToken")?.Value;
            //ViewData["LoginExpiresAt"] = User.Claims.FirstOrDefault(s => s.Type == "LoginExpiresAt")?.Value;
            //ViewData["LoginId"] = User.Claims.FirstOrDefault(s => s.Type == "LoginId")?.Value;
            //ViewData["LoginShopId"] = User.Claims.FirstOrDefault(s => s.Type == "LoginShopId")?.Value;
            //ViewData["LoginName"] = User.Claims.FirstOrDefault(s => s.Type == "LoginName")?.Value;
            //ViewData["LoginPhone"] = User.Claims.FirstOrDefault(s => s.Type == "LoginPhone")?.Value;
            //ViewData["LoginEmail"] = User.Claims.FirstOrDefault(s => s.Type == "LoginEmail")?.Value;
            //ViewData["LoginRole"] = User.Claims.FirstOrDefault(s => s.Type == "LoginRole")?.Value;

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private async Task SaveLoginInfo()
        {
            var userClaims = User.Claims;
            var loginId = userClaims.FirstOrDefault(s => s.Type == "LoginId")?.Value;
            if (string.IsNullOrEmpty(loginId))
            {
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
                var result = await _userApi.GetLoginInfoOrShopManagerFirstRegister(jwtPreferredUserName, jwtRole, jwtPhoneNumber, jwtEmail);

                var info = HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme).Result;
                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

                var idToken = HttpContext.GetTokenAsync(OpenIdConnectParameterNames.IdToken).Result;
                identity.AddClaim(new Claim("LoginIdToken", idToken, ClaimValueTypes.String));
                var accessToken = HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken).Result;
                identity.AddClaim(new Claim("LoginAccessToken", accessToken, ClaimValueTypes.String));
                var refreshToken = HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken).Result;
                identity.AddClaim(new Claim("LoginRefreshToken", refreshToken, ClaimValueTypes.String));
                var expiresAt = HttpContext.GetTokenAsync("expires_at").Result;
                identity.AddClaim(new Claim("LoginExpiresAt", expiresAt, ClaimValueTypes.String));
                identity.AddClaim(new Claim("LoginId", result.Data.Id.ToString(), ClaimValueTypes.String));
                identity.AddClaim(new Claim("LoginShopId", result.Data.ShopId.ToString(), ClaimValueTypes.String));
                identity.AddClaim(new Claim("LoginName", result.Data.Name, ClaimValueTypes.String));
                identity.AddClaim(new Claim("LoginPhone", result.Data.Phone, ClaimValueTypes.String));
                identity.AddClaim(new Claim("LoginEmail", result.Data.Email, ClaimValueTypes.String));
                identity.AddClaim(new Claim("LoginRole", result.Data.Role.ToString(), ClaimValueTypes.String));

                //追加会导致重复
                //info.Principal.AddIdentity(identity);          
                //await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, info.Principal, info.Properties);

                //使用新的new ClaimsPrincipal(identity)代替
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), info.Properties);

            }
        }
    }
}
