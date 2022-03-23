using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace MiniShop.Ids.Core
{
    public abstract class AbstractStartup
    {
        public AbstractStartup(IWebHostEnvironment env)
        {
            _env = env;
        }

        protected readonly IWebHostEnvironment _env;

        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddWebHost(_env);
        }

        public virtual void Configure(IApplicationBuilder app)
        {
            app.UseWebHost(_env);
        }
    }
}
