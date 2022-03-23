﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using MiniShop.Ids.Core;
using MiniShop.Ids.Config;

namespace MiniShop.Ids
{
    public class Startup : AbstractStartup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env) : base(env)
        {
            //绑定配置信息
            configuration.Binding<BasicSetting>("Setting")
                .OnChange(BasicSetting.Setting);
        }
    }

    //public class Startup
    //{
    //    public IWebHostEnvironment Environment { get; }
    //    public IConfiguration Configuration { get; }

    //    public Startup(IWebHostEnvironment environment, IConfiguration configuration)
    //    {
    //        Environment = environment;
    //        Configuration = configuration;
    //    }

    //    public void ConfigureServices(IServiceCollection services)
    //    {
    //        services.AddControllersWithViews().AddRazorRuntimeCompilation();

    //        services.AddDbContext<ApplicationDbContext>(options =>
    //            options.UseMySql(Configuration.GetConnectionString("DefaultConnection")));

    //        services.AddMigrations();

    //        services.AddIdentity<ApplicationUser, IdentityRole>()
    //            .AddEntityFrameworkStores<ApplicationDbContext>()
    //            .AddDefaultTokenProviders();

    //        services.Configure<IdentityOptions>(options =>
    //        {
    //            options.User.RequireUniqueEmail = false;//为true时发现注册时不能有重复邮件，但是修改时可以修改为重复邮件！！！
    //            //最少6位，包括至少1个大写字母，1个小写字母，1个数字，1个特殊字符
    //            options.Password.RequiredLength = 6;
    //            options.Password.RequiredUniqueChars = 1;
    //            options.Password.RequireDigit = true;
    //            options.Password.RequireNonAlphanumeric = true;
    //            options.Password.RequireUppercase = true;
    //            options.Password.RequireLowercase = true;

    //            //options.SignIn.RequireConfirmedEmail = true;
    //            //指 在帐户被锁定之前允许的失败登录的次数。默认值为 5。
    //            options.Lockout.MaxFailedAccessAttempts = 5;
    //            //默认锁定时间为 15 分钟。
    //            options.Lockout.DefaultLockoutTimeSpan = System.TimeSpan.FromMinutes(15);
    //        });

    //        services.ConfigureNonBreakingSameSiteCookies();

    //        var builder = services.AddIdentityServer(options =>
    //        {
    //            options.Events.RaiseErrorEvents = true;
    //            options.Events.RaiseInformationEvents = true;
    //            options.Events.RaiseFailureEvents = true;
    //            options.Events.RaiseSuccessEvents = true;

    //            // see https://identityserver4.readthedocs.io/en/latest/topics/resources.html
    //            options.EmitStaticAudienceClaim = true;
    //        })
    //            .AddInMemoryIdentityResources(IdentityServerConfig.IdentityResources)
    //            .AddInMemoryApiScopes(IdentityServerConfig.ApiScopes())
    //            .AddInMemoryApiResources(IdentityServerConfig.ApiResources)
    //            .AddInMemoryClients(IdentityServerConfig.Clients)
    //            .AddAspNetIdentity<ApplicationUser>();

    //        // not recommended for production - you need to store your key material somewhere secure
    //        builder.AddDeveloperSigningCredential();

    //        services.AddAuthentication()
    //            .AddGoogle(options =>
    //            {
    //                options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;

    //                // register your IdentityServer with Google at https://console.developers.google.com
    //                // enable the Google+ API
    //                // set the redirect URI to https://localhost:5001/signin-google
    //                options.ClientId = "copy client ID from Google here";
    //                options.ClientSecret = "copy client secret from Google here";
    //            });
    //    }

    //    public void Configure(IApplicationBuilder app)
    //    {
    //        // Add this before any other middleware that might write cookies
    //        app.UseCookiePolicy();

    //        if (Environment.IsDevelopment())
    //        {
    //            app.UseDeveloperExceptionPage();
    //            app.UseDatabaseErrorPage();
    //        }

    //        app.UseStaticFiles();

    //        app.UseRouting();
    //        app.UseIdentityServer();
    //        app.UseAuthorization();
    //        app.UseEndpoints(endpoints =>
    //        {
    //            endpoints.MapDefaultControllerRoute();
    //        });
    //    }
    //}

}