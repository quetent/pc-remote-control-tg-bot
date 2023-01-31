using Telegram.Bot.Types;
using static System.Console;

namespace RemoteControlBot
{
    public static class Log
    {
        public static ConsoleColor NeutralColor { get; set; } = ConsoleColor.White;
        public static ConsoleColor InfoColor { get; set; } = ConsoleColor.DarkYellow;
        public static ConsoleColor NewMessageColor { get; set; } = ConsoleColor.DarkGreen;
        public static ConsoleColor ExceptionColor { get; set; } = ConsoleColor.DarkRed;
        public static ConsoleColor ExecuteCommandColor { get; set; } = ConsoleColor.DarkBlue;
        public static ConsoleColor NotImportantColor { get; set; } = ConsoleColor.DarkGray;

        public static void BotStartup()
        {
            ByPattern("Startup", "Bot has been started", InfoColor);
        }

        public static void MessageRecieved(string messageText, User? user)
        {
            NewMessage(messageText, user, "Message recieved", NewMessageColor);
        }

        public static void MessageSkipped(string messageText, User? user)
        {
            NewMessage(messageText, user, "Message skipped", NotImportantColor);
        }

        public static void UnhandledException(Exception exception)
        {
            ByPattern("Unhandled exception", exception.Message, ExceptionColor);
        }

        public static void UnknownCommand(string commandText)
        {
            ByPattern("Unknown command", commandText, ExecuteCommandColor);
        }

        public static void KeyboardRequest()
        {
            ExecuteByPattern("Keyboard request", NotImportantColor);
        }

        public static void CommandExecute(string commandText)
        {
            ExecuteByPattern(commandText);
        }

        public static void FunctionNotImplemented(string commandText)
        {
            ExecuteByPattern($"Function not implemented: {commandText}", InfoColor);
        }

        private static void ByPattern(string eventType, string eventText, ConsoleColor eventColor)
        {
            var now = GetCurrentDateTimeAsString();

            WriteLine($"({now})");
            SetForegroundColor(eventColor);
            WriteLine($"[ {eventType} ]");
            ResetForegroundColor();
            WriteLine($"{eventText}");

            WriteLine();
        }

        private static void ExecuteByPattern(string commandText)
        {
            ExecuteByPattern(commandText, ExecuteCommandColor);
        }

        private static void ExecuteByPattern(string commandText, ConsoleColor color)
        {
            ByPattern("Execute", commandText, color);
        }

        private static void NewMessage(string messageText, User? user, string eventText, ConsoleColor color)
        {
            var username = user?.ToString();
            var messageFrom = username == string.Empty || username is null ? "unknown" : username;

            ByPattern(eventText, $"\"{messageText}\" from {messageFrom}", color);
        }

        private static string GetCurrentDateTimeAsString()
        {
            return $"{DateTime.Now:G}";
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
