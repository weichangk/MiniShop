using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MiniShopAdmin.Api.Code.Core
{

    public abstract class AbstractStartup
    {
        public AbstractStartup(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }


        protected readonly IConfiguration _configuration;
        protected readonly IWebHostEnvironment _env;


        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddWebHost(_env);
        }


        public virtual void Configure(IApplicationBuilder app, IHostApplicationLifetime lifetime)
        {
            app.UseWebHost(_configuration, _env, lifetime);
        }
    }
}
