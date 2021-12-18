using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using MiniShop.Api.Code.Core;

namespace MiniShop.Api
{
    /// <summary>
    /// 
    /// </summary>
    public class Program
    {
        //////public static void Main(string[] args)
        //////{
        //////    CreateHostBuilder(args).Build().Run();
        //////}

        //////public static IHostBuilder CreateHostBuilder(string[] args) =>
        //////    Host.CreateDefaultBuilder(args)
        //////        .ConfigureWebHostDefaults(webBuilder =>
        //////        {
        //////            webBuilder.UseStartup<Startup>();
        //////        });
 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Run();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return new MyHostBuilder().Create<Startup>(args)
                .Configure("initializationdata", false, true)
                .Configure("logging", false, false);
        }
    }
}
