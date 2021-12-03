﻿// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace MiniShop.Ids
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResources.Phone(),
            new IdentityResources.Email(),
            new IdentityResources.Address(),
            new IdentityResource(
                name: "MiniShopMvc.role",
                displayName: "MiniShopMvc role",
                userClaims: new[] { JwtClaimTypes.Role }),
        };

        public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
            new ApiScope("scope1"),
            new ApiScope("scope2"),
            new ApiScope("MiniShop.Api.Scope1"),
        };

        public static IEnumerable<ApiResource> ApiResources =>
        new ApiResource[]
        {
            new ApiResource("MiniShop.Api", "MiniShop Api")
            {
                ApiSecrets = { new Secret("MiniShop.Api.Secret".Sha256()) },
                Scopes = { "MiniShop.Api.Scope1"},
            }
        };

        public static IEnumerable<Client> Clients =>
        new Client[]
        {
            // m2m client credentials flow client
            new Client
            {
                ClientId = "m2m.client",
                ClientName = "Client Credentials Client",

                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = { new Secret("511536EF-F270-4058-80CA-1C89C192F69A".Sha256()) },

                AllowedScopes = { "scope1" }
            },

            // interactive client using code flow + pkce
            new Client
            {
                ClientId = "interactive",
                ClientSecrets = { new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256()) },

                AllowedGrantTypes = GrantTypes.Code,

                RedirectUris = { "https://localhost:44300/signin-oidc" },
                FrontChannelLogoutUri = "https://localhost:44300/signout-oidc",
                PostLogoutRedirectUris = { "https://localhost:44300/signout-callback-oidc" },

                AllowOfflineAccess = true,
                AllowedScopes = { "openid", "profile", "scope2" }
            },

            new Client
            {
                ClientId = "MiniShopMvcId",
                ClientName = "MiniShopMvcClient",
                AllowedGrantTypes = GrantTypes.CodeAndClientCredentials,
                ClientSecrets = { new Secret("MiniShopMvcClientSecret".Sha256()) },
                RedirectUris = { "http://localhost:5003/signin-oidc" },
                FrontChannelLogoutUri = "http://localhost:5003/signout-oidc",
                PostLogoutRedirectUris = { "http://localhost:5003/signout-callback-oidc" },
                AlwaysIncludeUserClaimsInIdToken = true,
                RequireConsent = false,
                AllowOfflineAccess = true,
                AccessTokenLifetime = 600,
                
                AllowedScopes = 
                { 
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.Email,
                    IdentityServerConstants.StandardScopes.Phone,
                    IdentityServerConstants.StandardScopes.Address,
                    "MiniShopMvc.role",
                    "MiniShop.Api.Scope1",
                }
            }
        };
    }
}