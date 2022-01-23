using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using MiniShopAdmin.Api.Code.Core;

namespace MiniShopAdmin.Api
{
    public class Program
    {

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return new MyHostBuilder().Create<Startup>(args)
                .Configure("initializationdata", false, true)
                .Configure("logging", false, false);
        }
    }
}
