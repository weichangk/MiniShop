using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MiniShop.IServices;
using MiniShop.Orm;
using MiniShop.Services;
using Newtonsoft.Json.Serialization;

namespace MiniShop.Api
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
            services.AddDbContext<AppDbContext>(option =>
            {
                option.UseMySql(Configuration["DbContext:MiniShopDBMySqlConnectionString"], option => option.MigrationsAssembly("MiniShop.Api"));
            });

            services.AddControllers(setupAction =>
            {
                setupAction.ReturnHttpNotAcceptable = true;
            })    
            .AddNewtonsoftJson(setupAction => {
                setupAction.SerializerSettings.ContractResolver =
                    new CamelCasePropertyNamesContractResolver();
            })
            .AddXmlDataContractSerializerFormatters()
            .ConfigureApiBehaviorOptions(setupAction =>
            {
                setupAction.InvalidModelStateResponseFactory = context =>
                {
                    var problemDetail = new ValidationProblemDetails(context.ModelState)
                    {
                        //Type = "无所谓",
                        Title = "数据验证失败",
                        Status = StatusCodes.Status422UnprocessableEntity,
                        Detail = "请看详细说明",
                        Instance = context.HttpContext.Request.Path
                    };
                    problemDetail.Extensions.Add("traceId", context.HttpContext.TraceIdentifier);
                    return new UnprocessableEntityObjectResult(problemDetail)
                    {
                        ContentTypes = { "application/problem+json" }
                    };
                };
            })
            //添加 Microsoft.AspNetCore.Mvc.NewtonsoftJson 包
            //.AddNewtonsoftJson(option =>
            //    //序列化器忽略循环引用
            //    option.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            //)
            ;

            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
            .AddIdentityServerAuthentication(options =>
            {
                options.Authority = "http://localhost:5001";
                options.ApiName = "MiniShop.Api";
                options.RequireHttpsMetadata = false;
                options.ApiSecret = "MiniShop.Api.Secret";
                //options.JwtValidationClockSkew = TimeSpan.FromSeconds(0);//时间偏移
            });


            //services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            //添加AutoMapper
            services.AddAutoMapper(typeof(Dto.Profiles.AutoMapperProfiles).Assembly);

            services.AddScoped<IShopService, ShopService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IItemService, ItemService>();
            services.AddScoped<ICategorieService, CategorieService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
