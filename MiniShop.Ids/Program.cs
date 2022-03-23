using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using MiniShop.Ids.Core;

namespace MiniShop.Ids
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
                .Configure("logging", false, false);
        }
    }

    //public class Program
    //{
    //    public static int Main(string[] args)
    //    {
    //        Log.Logger = new LoggerConfiguration()
    //            .MinimumLevel.Debug()
    //            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    //            .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
    //            .MinimumLevel.Override("System", LogEventLevel.Warning)
    //            .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
    //            .Enrich.FromLogContext()
    //            .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", theme: AnsiConsoleTheme.Code)
    //            .CreateLogger();

    //        try
    //        {
    //            var seed = args.Contains("/seed");
    //            if (seed)
    //            {
    //                args = args.Except(new[] { "/seed" }).ToArray();
    //            }

    //            var host = CreateHostBuilder(args).Build();

    //            if (seed)
    //            {
    //                Log.Information("Seeding database...");
    //                var config = host.Services.GetRequiredService<IConfiguration>();
    //                var connectionString = config.GetConnectionString("DefaultConnection");
    //                SeedData.EnsureSeedData(connectionString);
    //                Log.Information("Done seeding database.");
    //                return 0;
    //            }

    //            Log.Information("Starting host...");
    //            host.Run();
    //            return 0;
    //        }
    //        catch (Exception ex)
    //        {
    //            Log.Fatal(ex, "Host terminated unexpectedly.");
    //            return 1;
    //        }
    //        finally
    //        {
    //            Log.CloseAndFlush();
    //        }
    //    }

    //    public static IHostBuilder CreateHostBuilder(string[] args) =>
    //        Host.CreateDefaultBuilder(args)
    //            .UseSerilog()
    //            .ConfigureWebHostDefaults(webBuilder =>
    //            {
    //                webBuilder.UseStartup<Startup>();
    //            });
    //}
}