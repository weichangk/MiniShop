using Logging.Serilog;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using MiniShop.Ids.Config;
using System;

namespace MiniShop.Ids.Core
{
    public class MyHostBuilder
    {

        /// <summary>
        /// 创建主机
        /// </summary>
        /// <typeparam name="TStartup"></typeparam>
        /// <param name="args">启动参数</param>
        /// <returns></returns>
        public IHostBuilder Create<TStartup>(string[] args) where TStartup : AbstractStartup
        {
            return CreateBuilder<TStartup>(args);
        }

        private IHostBuilder CreateBuilder<TStartup>(string[] args) where TStartup : AbstractStartup
        {
            //IConfiguration对象中附加自定义配置文件方式，无法在配置文件中设置自宿主端口，在这里直接获取appsettings.json配置
            //为什么没独立创建IConfiguration对象，原因是热更新时配置中List集合数据无法使用Bind，会出现重复数据。
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("appsettings.json")
                .Build();
            configuration.GetSection("Setting").Bind(BasicSetting.Setting);

            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<TStartup>()
                    .UseLogging()
                    //.UseUrls(BasicSetting.Setting.Urls) // 使用 docker 部署需要屏蔽自宿主端口
                    ;
                });
        }
    }
}
