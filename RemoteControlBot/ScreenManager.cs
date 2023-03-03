using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace RemoteControlBot
{
    public static class ScreenManager
    {
        public static int VisibleScreensCount => _visibleScreens.Count;

        public static bool IsValidLastIndex { get; private set; }

        private static readonly List<Screen> _visibleScreens;
        public static List<Screen> VisibleScreens => _visibleScreens.Copy();

        static ScreenManager()
        {
            _visibleScreens = new List<Screen>();
        }

        public static void SetVisibleScreen()
        {
            if (_visibleScreens.Count != 0)
                _visibleScreens.Clear();

            foreach (var screen in Screen.AllScreens)
                _visibleScreens.Add(screen);
        }

        public static bool TryDoScreenshotByIndex(int index, out Bitmap screenshot)
        {
            if (index < _visibleScreens.Count)
            {
                IsValidLastIndex = true;
                screenshot = DoScreenshot(_visibleScreens[index]);
            }
            else
            {
                IsValidLastIndex = false;
                screenshot = null!;
            }

            return IsValidLastIndex;
        }

        public static Bitmap DoScreenshot(Screen screen)
        {
            var bitmap = new Bitmap(screen.Bounds.Width, screen.Bounds.Height, PixelFormat.Format32bppArgb);
            using var graphics = Graphics.FromImage(bitmap);

            graphics.CopyFromScreen(
                screen.Bounds.X, screen.Bounds.Y,
                0, 0,
                screen.Bounds.Size,
                CopyPixelOperation.SourceCopy);

            return bitmap;
        }

        public static void SaveScreenshot(Bitmap bitmap, ImageFormat format, string filepath)
        {
            // drops if trying save file with too long name
            bitmap.Save(filepath, format);
        }
    }
}
