using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Security.Claims;

namespace MiniShop.Mvc.Code
{
    /// <summary>
    /// 刷新登录状态
    /// </summary>
    public class TokenFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var expat = filterContext.HttpContext.GetTokenAsync("expires_at").Result;
            var dataExp = DateTime.Parse(expat, null, DateTimeStyles.RoundtripKind);
            var info = filterContext.HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme).Result;
            var loginId = info.Principal.FindFirst("LoginId")?.Value;

            if ((dataExp - DateTime.Now).TotalMinutes < 10 && !string.IsNullOrEmpty(loginId))
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

                    var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                    identity.AddClaim(new Claim("LoginIdToken", newIdToken, ClaimValueTypes.String));
                    identity.AddClaim(new Claim("LoginAccessToken", newAccessToken, ClaimValueTypes.String));
                    identity.AddClaim(new Claim("LoginRefreshToken", newRefreshToken, ClaimValueTypes.String));
                    identity.AddClaim(new Claim("LoginExpiresAt", expiresAt.ToString("o", CultureInfo.InvariantCulture), ClaimValueTypes.String));
                    identity.AddClaim(new Claim("LoginId", info.Principal.FindFirst("LoginId")?.Value, ClaimValueTypes.String));
                    identity.AddClaim(new Claim("LoginShopId", info.Principal.FindFirst("LoginShopId")?.Value, ClaimValueTypes.String));
                    identity.AddClaim(new Claim("LoginName", info.Principal.FindFirst("LoginName")?.Value, ClaimValueTypes.String));
                    identity.AddClaim(new Claim("LoginPhone", info.Principal.FindFirst("LoginPhone")?.Value, ClaimValueTypes.String));
                    identity.AddClaim(new Claim("LoginEmail", info.Principal.FindFirst("LoginEmail")?.Value, ClaimValueTypes.String));
                    identity.AddClaim(new Claim("LoginRole", info.Principal.FindFirst("LoginRole")?.Value, ClaimValueTypes.String));

                    //info.Principal.AddIdentity(identity);
                    //filterContext.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, info.Principal, info.Properties);

                    filterContext.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), info.Properties);
                    
                }
            }
        }
    }
}
