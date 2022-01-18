using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MiniShop.Ids.Data;
using MiniShop.Ids.Models;
using Serilog;
using System;
using System.Linq;
using System.Security.Claims;

namespace MiniShop.Ids
{
    public class SeedData
    {
        public static void EnsureSeedData(string connectionString)
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddDbContext<ApplicationDbContext>(options =>
               options.UseMySql(connectionString));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            using (var serviceProvider = services.BuildServiceProvider())
            {
                using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
                    context.Database.Migrate();

                    var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                    var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                    var role = roleMgr.FindByNameAsync("ShopManager").Result;
                    if (role == null)
                    {
                        IdentityRole IdentityRole = new IdentityRole
                        {
                            Name = "ShopManager",
                        };
                        var result = roleMgr.CreateAsync(IdentityRole).Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }
                    }
                    role = roleMgr.FindByNameAsync("ShopAssistant").Result;
                    if (role == null)
                    {
                        IdentityRole IdentityRole = new IdentityRole
                        {
                            Name = "ShopAssistant",
                        };
                        var result = roleMgr.CreateAsync(IdentityRole).Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }
                    }
                    role = roleMgr.FindByNameAsync("StoreManager").Result;
                    if (role == null)
                    {
                        IdentityRole IdentityRole = new IdentityRole
                        {
                            Name = "StoreManager",
                        };
                        var result = roleMgr.CreateAsync(IdentityRole).Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }
                    }
                    role = roleMgr.FindByNameAsync("StoreAssistant").Result;
                    if (role == null)
                    {
                        IdentityRole IdentityRole = new IdentityRole
                        {
                            Name = "StoreAssistant",
                        };
                        var result = roleMgr.CreateAsync(IdentityRole).Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }
                    }
                    role = roleMgr.FindByNameAsync("Cashier").Result;
                    if (role == null)
                    {
                        IdentityRole IdentityRole = new IdentityRole
                        {
                            Name = "Cashier",
                        };
                        var result = roleMgr.CreateAsync(IdentityRole).Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }
                    }

                    var mini = userMgr.FindByNameAsync("mini").Result;
                    if (mini == null)
                    {
                        mini = new ApplicationUser
                        {
                            UserName = "mini",
                            PhoneNumber = "18276743761",
                            Email = "18276743761@163.com",
                            //EmailConfirmed = true,
                        };
                        var result = userMgr.CreateAsync(mini, "Mini123$").Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        result = userMgr.AddToRolesAsync(mini, new System.Collections.Generic.List<string>{ "ShopManager", "Admin", "Cashier" }).Result; ;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        var shopId = Guid.NewGuid();
                        result = userMgr.AddClaimsAsync(mini, new Claim[]{
                            new Claim("rank", "ShopManager"),
                            new Claim("shopid", shopId.ToString()),
                            new Claim("storeid", shopId.ToString()),
                            new Claim("isfreeze", "false"),
                            new Claim("createdtime", DateTime.Now.ToString()),
                        }).Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        Log.Debug("mini created");
                    }
                    else
                    {
                        Log.Debug("mini already exists");
                    }

                }
            }
        }
    }
}
