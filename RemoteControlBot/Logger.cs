using static System.Console;

namespace RemoteControlBot
{
    internal static class Logger
    {
        public static ConsoleColor NeutralColor { get; set; } = ConsoleColor.White;
        public static ConsoleColor ExceptionColor { get; set; } = ConsoleColor.DarkRed;

        public static void LogUnhandledException(Exception exception)
        {
            SetForegroundColor(ExceptionColor);
            WriteLine($"[ Unhandled exception ]");
            ResetForegroundColor();
            WriteLine($" {exception.Message}");
        }

        private static void SetForegroundColor(ConsoleColor color)
        {
            ForegroundColor = color;
        }

        private static void ResetForegroundColor()
        {
            ForegroundColor = NeutralColor;
        }
    }
}
