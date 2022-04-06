using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using MiniShopAdmin.Api.Code.Core;
using MiniShopAdmin.Api.Config;
using MiniShopAdmin.Model.Code;

namespace MiniShopAdmin.Api
{
    public class Startup : AbstractStartup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env) : base(configuration, env)
        {
            //∞Û∂®≈‰÷√–≈œ¢
            configuration.Binding<BasicSetting>("Setting")
                .Binding<InitializationData>("Initialization")
                .OnChange(BasicSetting.Setting, InitializationData.Initialization);
        }
    }
}
