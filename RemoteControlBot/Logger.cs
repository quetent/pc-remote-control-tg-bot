using System;
using Telegram.Bot.Types;
using static System.Console;

namespace RemoteControlBot
{
    internal static class Logger
    {
        public static ConsoleColor NeutralColor { get; set; } = ConsoleColor.White;
        public static ConsoleColor InfoColor { get; set; } = ConsoleColor.DarkYellow;
        public static ConsoleColor NewMessageColor { get; set; } = ConsoleColor.DarkGreen;
        public static ConsoleColor ExceptionColor { get; set; } = ConsoleColor.DarkRed;

        public static void LogBotStartup()
        {
            LogByPattern("Startup", "Bot has been started", InfoColor);
        }

        public static void LogMessageRecieved(string messageText, User? user)
        {
            var username = user?.ToString();
            var messageFrom = username == string.Empty ? "unknown" : username;

            LogByPattern("New message recieved", $"\"{messageText}\" from {messageFrom}", NewMessageColor);
        }

        public static void LogUnhandledException(Exception exception)
        {
            LogByPattern("Unhandled exception", exception.Message, ExceptionColor);
        }

        private static void LogByPattern(string eventType, string eventText, ConsoleColor eventColor)
        {
            var now = GetCurrentDateTimeAsString();

            WriteLine($"({now})");
            SetForegroundColor(eventColor);
            WriteLine($"[ {eventType} ]");
            ResetForegroundColor();
            WriteLine($"{eventText}");

            WriteLine();
        }

        private static string GetCurrentDateTimeAsString()
        {
            var now = DateTime.Now;

            return $"{now.Month:d2}.{now.Day:d2}.{now.Year} {now.Hour:d2}:{now.Minute:d2}:{now.Second:d2}";
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
