using DisplayRefreshRateBatteryChangerService.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace DisplayRefreshRateBatteryChangerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IRefreshRateChangeService _refreshRateChangeService;


        public Worker(ILogger<Worker> logger, IRefreshRateChangeService refreshRateChangeService)
        {
            _logger = logger;
            _refreshRateChangeService = refreshRateChangeService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_refreshRateChangeService.CheckChangeRefresh())
                    _logger.LogInformation("Changed main display refresh rate to: {0}", _refreshRateChangeService.LastRefreshRate);

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
