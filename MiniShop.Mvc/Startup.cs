using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MiniShop.Mvc.Code;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using WebApiClientCore;

namespace MiniShop.Mvc
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)      
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IUserInfo, UserInfo>();

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, config => {
                config.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                config.Authority = _configuration["IdsConfig:Authority"];
                config.ClientId = _configuration["IdsConfig:ClientId"];
                config.ClientSecret = _configuration["IdsConfig:ClientSecret"];
                config.SaveTokens = true;
                config.ResponseType = _configuration["IdsConfig:ResponseType"];
                config.RequireHttpsMetadata = false;
                //config.SignedOutCallbackPath = "/Home/Index";

                // two trips to load claims in to the cookie
                // but the id token is smaller !
                config.GetClaimsFromUserInfoEndpoint = true;

                // configure scope
                config.Scope.Clear();
                var scopes = _configuration["IdsConfig:Scopes"];
                var scopeArray = scopes.Split(" ");
                for (int i = 0; i < scopeArray.Length; i++)
                {
                    config.Scope.Add(scopeArray[i]);
                }
            });

            services.Configure<CookiePolicyOptions>(option =>
            {
                option.CheckConsentNeeded = context => false;
            });

            services.AddHttpClient();

            services.AddControllersWithViews();

            //添加HttpClient相关
            var miniShopApiTypes = typeof(Startup).Assembly.GetTypes()
                        .Where(type => type.IsInterface
                        && ((System.Reflection.TypeInfo)type).ImplementedInterfaces != null
                        && type.GetInterfaces().Any(a => a.FullName == typeof(IHttpApi).FullName)
                        && type.IsDefined(typeof(MiniShopApiAttribute), false));
            foreach (var type in miniShopApiTypes)
            {
                services.AddHttpApi(type);
                services.ConfigureHttpApi(type, o =>
                {
                    o.HttpHost = new Uri(_configuration["MiniShopApi:Urls"]);
                });
            }
            var miniShopAdminApiTypes = typeof(Startup).Assembly.GetTypes()
            .Where(type => type.IsInterface
            && ((System.Reflection.TypeInfo)type).ImplementedInterfaces != null
            && type.GetInterfaces().Any(a => a.FullName == typeof(IHttpApi).FullName)
            && type.IsDefined(typeof(MiniShopAdminApiAttribute), false));
            foreach (var type in miniShopAdminApiTypes)
            {
                services.AddHttpApi(type);
                services.ConfigureHttpApi(type, o =>
                {
                    o.HttpHost = new Uri(_configuration["MiniShopAdminApi:Urls"]);
                });
            }

            services.AddScoped(typeof(RefreshAccessTokenAttribute));

            //添加AutoMapper
            services.AddAutoMapper(typeof(Dto.Profiles.AutoMapperProfiles).Assembly);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            //app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
