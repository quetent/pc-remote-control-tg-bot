using System.Runtime.InteropServices;

namespace RemoteControlBot
{
    public class NativeMethods
    {
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool LockWorkStation();

        [DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();

        public static bool LockPC()
        {
            return LockWorkStation();
        }

        public static bool SetAppDPIAware()
        {
            return SetProcessDPIAware();
        }
    }
}
