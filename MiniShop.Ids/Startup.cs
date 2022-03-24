using Microsoft.AspNetCore.Hosting;
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
}