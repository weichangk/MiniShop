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
        public HomeController(ILogger<HomeController> logger, IShopApi shopApi) : base(logger)
        {
            _shopApi = shopApi;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            await SaveLoginInfo();
            return View();
        }

        public IActionResult Privacy()
        {
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
                identity.AddClaim(new Claim("LoginStoreId", result.Data.Store.Id.ToString(), ClaimValueTypes.String));
                identity.AddClaim(new Claim("LoginStoreName", result.Data.Store.Name.ToString(), ClaimValueTypes.String));
                identity.AddClaim(new Claim("LoginShopId", result.Data.Shop.Id.ToString(), ClaimValueTypes.String));
                identity.AddClaim(new Claim("LoginShopName", result.Data.Shop.Name.ToString(), ClaimValueTypes.String));
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
