using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace RemoteControlBot
{
    public static class ScreenManager
    {
        [DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();

        private static bool _isDPIAwareSet = false;

        public static Bitmap DoScreenshot(Size size)
        {
            var bitmap = new Bitmap(size.Width, size.Height);
            using var graphics = Graphics.FromImage(bitmap);

            graphics.CopyFromScreen(Point.Empty, Point.Empty, bitmap.Size);

            return bitmap;
        }

        public static void SaveScreenshot(Bitmap bitmap, ImageFormat format, string filepath)
        {
            // drops if trying save file with too long name
            bitmap.Save(filepath, format);
        }

        public static Size GetMonitorSize()
        {
            SetProcessDPI();

            var hwnd = Process.GetCurrentProcess().MainWindowHandle;
            using var desktop = Graphics.FromHwnd(hwnd);

            return new Size((int)desktop.VisibleClipBounds.Width, (int)desktop.VisibleClipBounds.Height);
        }

        private static void SetProcessDPI()
        {
            if (!_isDPIAwareSet)
            {
                SetProcessDPIAware();
                _isDPIAwareSet = true;
            }
        }
    }
}
