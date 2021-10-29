using DisplayRefreshRateBatteryChangerService.Services.Options;
using Microsoft.Extensions.Options;
using System;
using System.Diagnostics;

namespace DisplayRefreshRateBatteryChangerService.Services
{
    public class RefreshRateChangeService : IRefreshRateChangeService, IDisposable
    {
        private Process _cmdProcess;
        private int _refreshRateOnBattery, _refreshRateOnPlugged, _lastRefreshRate;

        public int LastRefreshRate => _lastRefreshRate;

        public RefreshRateChangeService(IOptions<RefreshRateOptions> options)
        {
            _lastRefreshRate = 0;
            _refreshRateOnBattery = options.Value.RefreshRateOnBattery;
            _refreshRateOnPlugged = options.Value.RefreshRateOnPlugged;

            _cmdProcess = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    CreateNoWindow = false,
                    FileName = @"C:\Windows\System32\cmd.exe",
                    WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = false
                }
            };

            _cmdProcess.Start();
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
            _cmdProcess.StandardInput.WriteLine("QRes /r {0}", newRefreshRate);
            _lastRefreshRate = newRefreshRate;
        }

        public void Dispose()
        {
            _cmdProcess.Kill();
        }
    }
}
