using System.Drawing.Imaging;

namespace RemoteControlBot
{
    internal static class Config_
    {
        internal const long OWNER_ID = default;

        internal const int FINALIZE_WAITING_TIME_MS = default;

        internal const bool AUTO_RESTART_IF_CRASHED = default;
        internal const bool AUTO_RESTART_IF_CONNECTION_LOST = default;
        internal const bool ENABLE_LOGGING = default;
        internal const bool LOG_TO_CONSOLE = default;
        internal const bool LOG_TO_FILE = default;

        internal const bool WAIT_INTERNET_TO_START = default;

        internal const bool COMPILE_BACKGROUND = default;

        internal const string INTERNET_CHECKING_URL = default;

        internal const string TOKEN = default;
        internal const string BOT_NAME = default;

        internal const string SCREENSHOT_SAVE_DIR = default;
        internal const string SCREENSHOT_FILENAME = default;
        internal static readonly ImageFormat SCREENSHOT_FORMAT = default;

        internal const string LOG_FILENAME = default;
        internal const string BUG_REPORT_FILENAME = default;
    }
}
