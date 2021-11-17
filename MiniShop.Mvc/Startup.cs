using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using WebApiClient;

namespace MiniShop.Mvc
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {

            //JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;

            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, config => {
                config.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                config.Authority = "https://localhost:5001/";
                config.ClientId = "MiniShopMvcId";
                config.ClientSecret = "MiniShopMvcClientSecret";
                config.SaveTokens = true;
                config.ResponseType = "code"; 
                //config.SignedOutCallbackPath = "/Home/Index";

                //// configure cookie claim mapping
                //config.ClaimActions.DeleteClaim("amr");
                //config.ClaimActions.DeleteClaim("s_hash");
                //config.ClaimActions.MapUniqueJsonKey("RawCoding.Grandma", "rc.garndma");

                //// two trips to load claims in to the cookie
                //// but the id token is smaller !
                //config.GetClaimsFromUserInfoEndpoint = true;

                // configure scope
                config.Scope.Clear();
                config.Scope.Add(OidcConstants.StandardScopes.OpenId);
                config.Scope.Add(OidcConstants.StandardScopes.Profile);
                config.Scope.Add(OidcConstants.StandardScopes.OfflineAccess);
                //config.Scope.Add("rc.scope");
                //config.Scope.Add("ApiOne");
                //config.Scope.Add("ApiTwo");
                //config.Scope.Add("offline_access");

            });

            services.AddHttpClient();

            services.AddControllersWithViews();

            //Ìí¼ÓHttpClientÏà¹Ø
            var types = typeof(Startup).Assembly.GetTypes()
                        .Where(type => type.IsInterface
                        && ((System.Reflection.TypeInfo)type).ImplementedInterfaces != null
                        && type.GetInterfaces().Any(a => a.FullName == typeof(IHttpApi).FullName));
            foreach (var type in types)
            {
                services.AddHttpApi(type);
                services.ConfigureHttpApi(type, o =>
                {
                    o.HttpHost = new Uri(Configuration["ApiUrl:Urls"]);
                });
            }
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
            app.UseHttpsRedirection();

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
