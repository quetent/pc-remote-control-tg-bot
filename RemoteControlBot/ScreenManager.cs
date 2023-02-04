using System.Drawing;
using System.Drawing.Imaging;
using System.Management;

namespace RemoteControlBot
{
    public static class ScreenManager
    {
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
            uint width = 0, height = 0;

            var query = "SELECT * FROM Win32_VideoController";

            using (var wmiSearcher = new ManagementObjectSearcher(query))
            {
                foreach (var videoController in wmiSearcher.Get())
                {
                    var horizontalRes = videoController["CurrentHorizontalResolution"];
                    var verticalRes = videoController["CurrentVerticalResolution"];

                    if (horizontalRes is not null && verticalRes is not null)
                    {
                        width = (uint)horizontalRes;
                        height = (uint)verticalRes;
                    }
                }
            }

            return new Size((int)width, (int)height);
        }
    }
}
