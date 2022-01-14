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
                name: _configuration["UserClaimExtras:Name"],
                displayName: _configuration["UserClaimExtras:DisplayName"],
                userClaims: _configuration["UserClaimExtras:UserClaims"].Split(" ")),
        };

        public static IEnumerable<ApiScope> ApiScopes()
        {
            List<ApiScope> apiScope = new List<ApiScope>();
            var miniShopApiScopes = _configuration["MiniShopApiResourceConfig:Scopes"].Split(" ");
            for (int i = 0; i < miniShopApiScopes.Length; i++)
            {
                apiScope.Add(new ApiScope {Name = miniShopApiScopes[i] });
            }

            return apiScope;
        }


        public static IEnumerable<ApiResource> ApiResources =>
        new ApiResource[]
        {
            new ApiResource(_configuration["MiniShopApiResourceConfig:Name"], _configuration["MiniShopApiResourceConfig:DisplayName"])
            {
                ApiSecrets = { new Secret(_configuration["MiniShopApiResourceConfig:Secret"].Sha256()) },
                Scopes = _configuration["MiniShopApiResourceConfig:Scopes"].Split(" "),
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
                RedirectUris = { $"{_configuration["MiniShopWebConfig:ApplicationUrl"]}/signin-oidc" },
                FrontChannelLogoutUri = $"{_configuration["MiniShopWebConfig:ApplicationUrl"]}/signout-oidc",
                PostLogoutRedirectUris = { $"{_configuration["MiniShopWebConfig:ApplicationUrl"]}/signout-callback-oidc" },
                AlwaysIncludeUserClaimsInIdToken = true,
                RequireConsent = false,
                AllowOfflineAccess = true,
                AccessTokenLifetime = int.Parse(_configuration["MiniShopWebConfig:AccessTokenLifetime"]),
                AllowedScopes = _configuration["MiniShopWebConfig:AllowedScopes"].Split(" "),
            }
        };
    }
}