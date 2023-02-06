using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

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
