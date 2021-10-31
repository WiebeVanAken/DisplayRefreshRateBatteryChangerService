using System;
using System.Runtime.InteropServices;

namespace DisplayRefreshRateBatteryChangerService.DTO
{
    [StructLayout(LayoutKind.Sequential)]
    public class PowerState
    {
        public ACLineStatus ACLineStatus;
        public BatteryFlag BatteryFlag;
        public Byte BatteryLifePercent;
        public Byte Reserved1;
        public Int32 BatteryLifeTime;
        public Int32 BatteryFullLifeTime;

        private PowerState() { }

        public static PowerState GetPowerState()
        {
            PowerState state = new PowerState();
            if (GetSystemPowerStatusRef(state))
                return state;

            throw new ApplicationException("Unable to get power state");
        }

        public override string ToString()
        {
            return String.Format("ACLineStatus {0}, BatteryFlag {1}, BatteryLifePercent {2}", ACLineStatus, BatteryFlag, BatteryLifePercent);
        }

        [DllImport("Kernel32", EntryPoint = "GetSystemPowerStatus")]
        private static extern bool GetSystemPowerStatusRef(PowerState sps);
    }
}
