using System.Diagnostics;

namespace DisplayRefreshRateBatteryChangerService.Services
{
    public class RefreshRateChangeService : IRefreshRateChangeService
    {
        private Process _cmdProcess;
        private int lastRefreshRate = 0;

        public int LastRefreshRate => lastRefreshRate;

        public RefreshRateChangeService()
        {
            _cmdProcess = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    CreateNoWindow = true,
                    FileName = @"C:\Windows\System32\cmd.exe",
                    WorkingDirectory = @"C:\Windows\System32\",
                    RedirectStandardInput = true
                }
            };

            _cmdProcess.Start();
        }

        public bool CheckChangeRefresh()
        {
            var powerState = PowerState.GetPowerState();
            var currentRefreshRate = lastRefreshRate;

            if (powerState.ACLineStatus == ACLineStatus.Online && lastRefreshRate != 90)
                ExecuteChangeCommand(90);

            if (powerState.ACLineStatus == ACLineStatus.Offline && lastRefreshRate != 60)
                ExecuteChangeCommand(60);

            return currentRefreshRate != lastRefreshRate;
        }

        private void ExecuteChangeCommand(int newRefreshRate)
        {
            _cmdProcess.StandardInput.WriteLine("qres /r {0}", newRefreshRate);
            lastRefreshRate = newRefreshRate;
        }
    }
}
