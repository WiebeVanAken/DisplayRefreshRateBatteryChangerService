using DisplayRefreshRateBatteryChangerService.DTO;
using DisplayRefreshRateBatteryChangerService.Services.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Diagnostics;

namespace DisplayRefreshRateBatteryChangerService.Services
{
    public class RefreshRateChangeService : IRefreshRateChangeService
    {
        private ILogger<Worker> _logger;
        private int _refreshRateOnBattery, _refreshRateOnPlugged, _lastRefreshRate;

        public int LastRefreshRate => _lastRefreshRate;

        public RefreshRateChangeService(IOptions<RefreshRateOptions> options, ILogger<Worker> logger)
        {
            _logger = logger;
            _lastRefreshRate = 0;
            _refreshRateOnBattery = options.Value.RefreshRateOnBattery;
            _refreshRateOnPlugged = options.Value.RefreshRateOnPlugged;
        }

        public bool CheckChangeRefresh()
        {
            var powerState = PowerState.GetPowerState();
            var currentRefreshRate = _lastRefreshRate;

            if (powerState.ACLineStatus == ACLineStatus.Online && _lastRefreshRate != _refreshRateOnPlugged)
                ExecuteChangeCommand(_refreshRateOnPlugged);

            if (powerState.ACLineStatus == ACLineStatus.Offline && _lastRefreshRate != _refreshRateOnBattery)
                ExecuteChangeCommand(_refreshRateOnBattery);

            return currentRefreshRate != _lastRefreshRate;
        }

        private void ExecuteChangeCommand(int newRefreshRate)
        {
            _logger.LogInformation(AppDomain.CurrentDomain.BaseDirectory);
            using (Process process = Process.Start(
                new ProcessStartInfo
                {
                    FileName = AppDomain.CurrentDomain.BaseDirectory + "QRes.exe",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    Arguments = String.Format("/r {0}", newRefreshRate)
                }
            ))
            
            _lastRefreshRate = newRefreshRate;
        }
    }
}
