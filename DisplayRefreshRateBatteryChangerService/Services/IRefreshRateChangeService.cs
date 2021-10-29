namespace DisplayRefreshRateBatteryChangerService.Services
{
    public interface IRefreshRateChangeService
    {
        /// <summary>
        /// Check if the refresh rate should be changed
        /// </summary>
        /// <returns>Returns true if the refresh rate has changed</returns>
        public bool CheckChangeRefresh();

        /// <summary>
        /// Retrieve the last known refresh rate
        /// </summary>
        public int LastRefreshRate { get; }
    }
}
