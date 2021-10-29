using System;

namespace DisplayRefreshRateBatteryChangerService.Services.Options
{
    public class RefreshRateOptions
    {
        private int _refreshRateOnBattery, _refreshRateOnPlugged;

        /// <summary>
        /// The refresh rate when the host device is on battery power
        /// </summary>
        public int RefreshRateOnBattery
        {
            get => _refreshRateOnBattery;
            set
            {
                if (value >= 1)
                    _refreshRateOnBattery = value;
                else
                    throw new ArgumentException("ConfigError - RefreshRateOnBattery config value must be greater than 1");
            }
        }

        /// <summary>
        /// The refresh rate when the host device is plugged in
        /// </summary>
        public int RefreshRateOnPlugged {
            get => _refreshRateOnPlugged;
            set
            {
                if (value >= 1)
                    _refreshRateOnPlugged = value;
                else
                    throw new ArgumentException("ConfigError - RefreshRateOnPlugged config value must be greater than 1");
            }
        }
    }
}
