namespace RemoteControlBot
{
    public static class PathManager
    {
        public static string GetScreenshotAbsolutePath()
        {
            var filename = $"{SCREENSHOT_FILENAME}.{SCREENSHOT_FORMAT.ToString().ToLowerInvariant()}";

            return Path.Combine(SCREENSHOT_SAVE_DIR, filename);
        }
    }
}
