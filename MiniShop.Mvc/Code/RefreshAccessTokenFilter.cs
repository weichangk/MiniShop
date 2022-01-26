using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;

namespace MiniShop.Mvc.Code
{
    public class RefreshAccessTokenFilter : ActionFilterAttribute
    {
        private readonly IConfiguration _configuration;

        public RefreshAccessTokenFilter(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var expat = filterContext.HttpContext.GetTokenAsync("expires_at").Result;
            if (expat == null) return;//未登录认证

            var dataExp = DateTime.Parse(expat, null, DateTimeStyles.RoundtripKind);
            var info = filterContext.HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme).Result;

            if ((dataExp - DateTime.Now).TotalMinutes < 10)
            {
                var client = new HttpClient();
                var disco = client.GetDiscoveryDocumentAsync(_configuration["IdsConfig:Authority"]).Result;
                if (disco.IsError)
                {
                    throw new Exception(disco.Error);
                }
                // refreshToken 只能用一次，刷新后更新refreshToken
                var refreshToken = filterContext.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken).Result;

                // Refresh Access Token
                var tokenResult = client.RequestRefreshTokenAsync(new RefreshTokenRequest
                {
                    Address = disco.TokenEndpoint,
                    ClientId = _configuration["IdsConfig:ClientId"],
                    ClientSecret = _configuration["IdsConfig:ClientSecret"],
                    Scope = _configuration["IdsConfig:Scopes"],
                    GrantType = OpenIdConnectGrantTypes.RefreshToken,
                    RefreshToken = refreshToken
                }).Result;

                if (tokenResult.IsError)
                {
                    //filterContext.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    //filterContext.HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
                    throw new Exception(tokenResult.Error);
                }
                else
                {
                    //var oldIdToken = filterContext.HttpContext.GetTokenAsync("id_token").Result;
                    var newIdToken = tokenResult.IdentityToken;
                    var newAccessToken = tokenResult.AccessToken;
                    var newRefreshToken = tokenResult.RefreshToken;
                    var expiresAt = DateTime.Now + TimeSpan.FromSeconds(tokenResult.ExpiresIn);

                    var tokens = new List<AuthenticationToken>
                    {
                        new AuthenticationToken
                        {
                            Name = OpenIdConnectParameterNames.IdToken,
                            Value = newIdToken //oldIdToken
                        },
                        new AuthenticationToken
                        {
                            Name = OpenIdConnectParameterNames.AccessToken,
                            Value = newAccessToken
                        },
                        new AuthenticationToken
                        {
                            Name = OpenIdConnectParameterNames.RefreshToken,
                            Value = newRefreshToken
                        },
                        new AuthenticationToken
                        {
                            Name = "expires_at",
                            Value = expiresAt.ToString("o", CultureInfo.InvariantCulture)
                        },
                    };
                    info.Properties.StoreTokens(tokens);
                    filterContext.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, info.Principal, info.Properties);
                }
            }
        }
    }
}
