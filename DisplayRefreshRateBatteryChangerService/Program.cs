using DisplayRefreshRateBatteryChangerService.Services;
using DisplayRefreshRateBatteryChangerService.Services.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace DisplayRefreshRateBatteryChangerService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(ConfigureAppConfiguration)
                .UseWindowsService()
                .ConfigureServices(ConfigureServices);

        public static void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services) {
            services.AddOptions();
            services.Configure<RefreshRateOptions>(hostContext.Configuration.GetSection("RefreshRateConfig"));

            services.AddSingleton<IRefreshRateChangeService, RefreshRateChangeService>();
            services.AddHostedService<Worker>();
        }

        public static void ConfigureAppConfiguration(HostBuilderContext hostContext, IConfigurationBuilder config)
        {
            config
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("appSettings.json", true, true);

            config.AddEnvironmentVariables();
            config.Build();
        }
    }
}
