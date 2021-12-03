using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;

namespace MiniShop.Mvc.Code
{
    public class TokenFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            LoginInfos.AccessToken = filterContext.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken).Result;

            var expat = filterContext.HttpContext.GetTokenAsync("expires_at").Result;
            var dataExp = DateTime.Parse(expat, null, DateTimeStyles.RoundtripKind);

            if ((dataExp - DateTime.Now).TotalMinutes < 1)
            {
                var client = new HttpClient();
                var disco = client.GetDiscoveryDocumentAsync("http://localhost:5001").Result;
                if (disco.IsError)
                {
                    throw new Exception(disco.Error);
                }
                var refreshToken = filterContext.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken).Result;

                // Refresh Access Token
                var tokenResult = client.RequestRefreshTokenAsync(new RefreshTokenRequest
                {
                    Address = disco.TokenEndpoint,
                    ClientId = "MiniShopMvcId",
                    ClientSecret = "MiniShopMvcClientSecret",
                    Scope = "openid profile email phone address MiniShopMvc.role MiniShop.Api.Scope1",
                    GrantType = OpenIdConnectGrantTypes.RefreshToken,
                    RefreshToken = refreshToken
                }).Result;

                if (tokenResult.IsError)
                {
                    throw new Exception(tokenResult.Error);
                }
                else
                {
                    //var oldIdToken = filterContext.HttpContext.GetTokenAsync("id_token").Result;
                    var newIdToken = tokenResult.IdentityToken;
                    var newAccessToken = tokenResult.AccessToken;
                    var newRefreshToken = tokenResult.RefreshToken;
                    var expiresAt = DateTime.Now + TimeSpan.FromSeconds(tokenResult.ExpiresIn);
                    LoginInfos.AccessToken = newAccessToken;

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

                    var info = filterContext.HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme).Result;
                    info.Properties.StoreTokens(tokens);
                    filterContext.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, info.Principal, info.Properties);
                }
            }
        }
    }
}
