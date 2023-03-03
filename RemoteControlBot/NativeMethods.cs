using System.Runtime.InteropServices;

namespace RemoteControlBot
{
    public class NativeMethods
    {
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool LockWorkStation();

        public static bool LockPC()
        {
            return LockWorkStation();
        }
    }
}
