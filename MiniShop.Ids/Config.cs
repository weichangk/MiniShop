using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace MiniShop.Ids
{
    public static class Config
    {
        private static readonly IConfigurationRoot _configuration;
        static Config()
        {
            var builder = new ConfigurationBuilder();
            builder.AddJsonFile("appsettings.json");
            _configuration = builder.Build();
        }

        public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResources.Phone(),
            new IdentityResources.Email(),
            new IdentityResource(
                name: "user_rank",
                displayName: "user rank",
                userClaims: new[] { "rank" }),
        };

        public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
            new ApiScope("minishop_api"),
        };

        public static IEnumerable<ApiResource> ApiResources =>
        new ApiResource[]
        {
            new ApiResource("minishop_api", "minishop api")
            {
                ApiSecrets = { new Secret("minishop_api_secret".Sha256()) },
                Scopes = { "minishop_api"},
            }
        };

        public static IEnumerable<Client> Clients =>
        new Client[]
        {
            new Client
            {
                    ClientId = _configuration["MiniShopWebConfig:ClientId"],
                    ClientName = _configuration["MiniShopWebConfig:ClientName"],
                    AllowedGrantTypes = GrantTypes.CodeAndClientCredentials,
                    ClientSecrets = { new Secret(_configuration["MiniShopWebConfig:ClientSecret"].Sha256()) },
                    RedirectUris = { $"{_configuration["MiniShopWebConfig:ApplicationUrl"]}signin-oidc" },
                    FrontChannelLogoutUri = $"{_configuration["MiniShopWebConfig:ApplicationUrl"]}signout-oidc",
                    PostLogoutRedirectUris = { $"{_configuration["MiniShopWebConfig:ApplicationUrl"]}signout-callback-oidc" },
                    AlwaysIncludeUserClaimsInIdToken = true,
                    RequireConsent = false,
                    AllowOfflineAccess = true,
                    AccessTokenLifetime = 3600,

                AllowedScopes = 
                { 
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.Email,
                    IdentityServerConstants.StandardScopes.Phone,
                    "user_rank",
                    "minishop_api",
                }
            }
        };
    }
}