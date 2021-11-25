using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using MiniShop.Mvc.HttpApis;
using MiniShop.Mvc.Models;
using System;
using System.Collections.Generic;
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
            //int minutes = 60 * 6;
            //SetCookies(LoginUserId, result.Id.ToString(), minutes);
            //SetCookies(LoginUserShopId, result.ShopId.ToString(), minutes);
            //SetCookies(LoginUserName, result.Name.ToString(), minutes);
            //SetCookies(LoginUserPhone, result.Phone.ToString(), minutes);
            //SetCookies(LoginUserEmail, result.Email.ToString(), minutes);
            //SetCookies(LoginUserRole, result.Role.ToString(), minutes);

            TempData[LoginUserId] = result.Id;
            TempData[LoginUserShopId] = result.ShopId;
            TempData[LoginUserName] = result.Name;
            TempData[LoginUserPhone] = result.Phone;
            TempData[LoginUserEmail] = result.Email;
            TempData[LoginUserRole] = result.Role;

            //Cookie保存马上GetCookies()不能获取到值。。

            JwtTokenModel jwtTokenModel = new JwtTokenModel
            {
                AccessToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken),
                RefreshToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken),
                ExpiresIn = 3600,
            };

            SaveUserCookie(jwtTokenModel);

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

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        /// <summary>
        /// ASP.NET CORE Cookie 保存身份信息
        /// </summary>
        private void SaveUserCookie(JwtTokenModel jwt)
        {
            // 获取身份认证的结果，包含当前的pricipal和properties
            var currentAuthenticateResult = HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme).Result;

            //创建 Claim 对象将用户信息存储在 Claim 类型的字符串键值对中，
            //将 Claim 对象传入 ClaimsIdentity 中，用来构造一个 ClaimsIdentity 对象
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim("AccessToken", jwt.AccessToken, ClaimValueTypes.String));
            identity.AddClaim(new Claim("RefreshToken", jwt.RefreshToken, ClaimValueTypes.String));
            identity.AddClaim(new Claim("ExpiresIn", jwt.ExpiresIn.ToString(), ClaimValueTypes.Integer32));

            currentAuthenticateResult.Principal.AddIdentity(identity);

            //调用 HttpContext.SignInAsync 方法，传入上面创建的 ClaimsPrincipal 对象，完成用户登录
            //HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), new AuthenticationProperties { ExpiresUtc = DateTime.UtcNow.AddHours(6) });

            //可以使用 HttpContext.SignInAsync 方法的重载来定义持久化 cookie 存储用户认证信息，如下定义了用户登录 60 分钟内cookie 都会保留在客户端计算机硬盘上
            //即使关闭浏览器，60 分钟内再次访问仍然是处于登录状态，除非调用 Logout 方法注销登录
            //注意 AllowRefresh 属性，如果 AllowRefresh 为 true， 表示如果用户登录超过 50% 的 ExpiresUtc 时间间隔内又访问了站点，就延长用户的登录时间（其实就是延长 cookie 客户端计算机硬盘的保留时间）。
            //如下，设置 ExpiresUtc 属性为 60分钟后，那么当用户登录在大于 30 分钟且小于 60 分钟内访问了站点，那么就将用户登录状态再延长到当前时间后的 60 分钟。但用户在登录后 30 分钟内访问站点是不会延长登录时间的。
            //因为 ASP.NET Core 有个硬核要求，就是用户在超过 50% 的 ExpiresUtc 时间间隔内又访问了站点，才延长用户的登录时间。
            //如果 AllowRefresh 为 false，表示用户登陆后 60 分钟内不管有没有访问站点，只要 60 分钟到了，立马就处于非登录状态（不延长 cookie 在客户端计算机硬盘上的保留时间，60 分钟到了，客户端计算机自动删除 cookie）

            //调用 HttpContext.SignInAsync 方法，传入上面创建的 ClaimsPrincipal 对象，完成用户登录
            //HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), new AuthenticationProperties
            //{
            //    //获取或设置身份验证会话是否跨多个持久化要求
            //    IsPersistent = false,
            //    ExpiresUtc = null,
            //    //AllowRefresh = true,
            //    RedirectUri = "/Home/index"
            //});
            // 登录
            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
               currentAuthenticateResult.Principal, currentAuthenticateResult.Properties);

            //如果当前 Http 请求本来登录了用户 A，现在调用 HttpContext.SignInAsync 方法登录用户 B，那么相当于注销用户 A，登录用户 B
        }
    }
}
