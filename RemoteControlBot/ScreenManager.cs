using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace RemoteControlBot
{
    public static class ScreenManager
    {
        public static int ScreensCount => _Screens.Count;

        public static bool IsValidLastIndex { get; private set; }

        private static readonly List<Screen> _Screens;
        public static IReadOnlyCollection<Screen> VisibleScreens => _Screens.AsReadOnly();

        static ScreenManager()
        {
            _Screens = new List<Screen>();
        }

        public static void SetVisibleScreen()
        {
            if (_Screens.Count != 0)
                _Screens.Clear();

            foreach (var screen in Screen.AllScreens)
                _Screens.Add(screen);
        }

        public static bool TryDoScreenshotByIndex(int index, out Bitmap screenshot)
        {
            if (index < _Screens.Count)
            {
                IsValidLastIndex = true;
                screenshot = DoScreenshot(_Screens[index]);
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
