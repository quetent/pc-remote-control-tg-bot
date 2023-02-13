using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace RemoteControlBot
{
    public static class ScreenManager
    {
        static ScreenManager()
        {
            NativeMethods.SetAppDPIAware();
        }

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

        public static Size GetPrimaryScreenSize()
        {
            return new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
        }
    }
}
