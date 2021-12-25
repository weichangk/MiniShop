using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using MiniShop.Api.Code.Core;
using MiniShop.Api.Config;
using MiniShop.Model.Code;

namespace MiniShop.Api
{
    /// <summary>
    /// 
    /// </summary>
    public class Startup : AbstractStartup
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="env"></param>
        public Startup(IConfiguration configuration, IWebHostEnvironment env) : base(env)
        {
            //绑定配置信息
            configuration.Binding<BasicSetting>("Setting")
                .Binding<InitializationData>("Initialization")
                .OnChange(BasicSetting.Setting, InitializationData.Initialization);
        }
    }
}
