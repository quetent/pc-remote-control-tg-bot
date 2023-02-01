namespace RemoteControlBot
{
    internal static class PathManager
    {
        internal static string GetScreenshotAbsolutePath()
        {
            var filename = $"{SCREENSHOT_FILENAME}.{SCREENSHOT_FORMAT.ToString().ToLowerInvariant()}";

            return Path.Combine(SCREENSHOT_SAVE_DIR, filename);
        }
    }
}
