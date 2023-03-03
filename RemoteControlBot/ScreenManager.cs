using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace RemoteControlBot
{
    public static class ScreenManager
    {
        public static int ScreensCount => _screens.Count;

        public static bool IsValidLastIndex { get; private set; }

        private static readonly List<Screen> _screens;
        public static ReadOnlyCollection<Screen> Screens => _screens.AsReadOnly();

        static ScreenManager()
        {
            _screens = new List<Screen>();
        }

        public static void SetVisibleScreen()
        {
            if (_screens.Count != 0)
                _screens.Clear();

            foreach (var screen in Screen.AllScreens)
                _screens.Add(screen);
        }

        public static bool TryDoScreenshotByIndex(int index, out Bitmap screenshot)
        {
            if (index < _screens.Count)
            {
                IsValidLastIndex = true;
                screenshot = DoScreenshot(_screens[index]);
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
