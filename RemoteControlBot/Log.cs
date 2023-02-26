using System.Text;
using Telegram.Bot.Types;
using static System.Console;

namespace RemoteControlBot
{
    public static class Log
    {
        public static ConsoleColor ExceptionColor { get; set; } = ConsoleColor.DarkRed;
        public static ConsoleColor ExecuteColor { get; set; } = ConsoleColor.DarkBlue;
        public static ConsoleColor InfoColor { get; set; } = ConsoleColor.DarkYellow;
        public static ConsoleColor NeutralColor { get; set; } = ConsoleColor.White;
        public static ConsoleColor NewMessageColor { get; set; } = ConsoleColor.DarkGreen;
        public static ConsoleColor NotImportantColor { get; set; } = ConsoleColor.DarkGray;

        public static void If(Func<bool> condition, Action logMethod)
        {
            if (condition())
                logMethod.Invoke();
        }

        public static void BotStartup(string text)
        {
            ByPattern("Startup", text, InfoColor);
        }

        public static void AppRestart()
        {
            ByPattern("Restart", "Restarting app", InfoColor);
        }

        public static void AppExit()
        {
            ByPattern("Exit", "Exiting app", InfoColor);
        }

        public static void ConnectionLost()
        {
            ByPattern("Connection", "Internet connection lost", ExceptionColor);
        }

        public static void NoConnection()
        {
            ByPattern("Connection", "No internet connection", InfoColor);
        }

        public static void WaitingForInternetConnection()
        {
            ByPattern("Connection", "Waiting for internet connection", InfoColor);
        }

        public static void ConnectionRestored()
        {
            ByPattern("Connection", "Internet connection restored", InfoColor);
        }

        public static void ScreenshotSending()
        {
            ByPattern("Screenshot", "Sending screenshot", ExecuteColor);
        }

        public static void MessageRecieved(string messageText, User? user)
        {
            NewMessage(messageText, user, "Message recieved", NewMessageColor);
        }

        public static void MessageSkipped(string messageText, User? user)
        {
            NewMessage(messageText, user, "Message skipped", NotImportantColor);
        }

        private static void NewMessage(string messageText, User? user, string eventText, ConsoleColor color)
        {
            var username = user?.ToString();
            var messageFrom = username == string.Empty || username is null ? "undefined" : username;

            ByPattern(eventText, $"\"{messageText}\" from {messageFrom}", color);
        }

        public static void UnhandledException(Exception exception)
        {
            var exceptionData = $"{exception.GetType()}:\n{exception.Message}";

            ByPattern("Unhandled exception", exceptionData, ExceptionColor, true);
        }

        public static void UndefinedCommand(string commandText)
        {
            ByPattern("Undefined command skipped", commandText, NotImportantColor);
        }

        public static void KeyboardRequest(Command command)
        {
            ByPattern("Keyboard request", command.ToString(), NotImportantColor);
        }

        public static void CommandExecute(Command command)
        {
            ByPattern("Execute", command.ToString(), ExecuteColor);
        }

        public static void UpdateExecute(Command command, string commandText)
        {
            if (command.Type is CommandType.Transfer)
                KeyboardRequest(command);
            else
            {
                if (command.Type is not CommandType.Undefined)
                    CommandExecute(command);
                else
                    UndefinedCommand(commandText);
            }
        }

        public static void LogToFile(StreamWriter stream, DateTime now, string eventType, string eventText)
        {
            var logString = $"({now})\n[ {eventType} ]\n{eventText}\n";

            stream.WriteLine(logString);
        }

        private static void LogToConsole(DateTime now, string eventType, string eventText, ConsoleColor eventColor)
        {
            WriteLine($"({now})");
            SetForegroundColor(eventColor);
            WriteLine($"[ {eventType} ]");
            ResetForegroundColor();
            WriteLine($"{eventText}");

            WriteLine();
        }

        private static void ByPattern(string eventType, string eventText, ConsoleColor eventColor, bool isException = false)
        {
            var now = DateTimeManager.GetCurrentDateTime();

            if (LOG_TO_CONSOLE)
                LogToConsole(now, eventType, eventText, eventColor);

            if (LOG_TO_FILE)
            {
                var filename = isException ? BUG_REPORT_FILENAME : LOG_FILENAME;
                using var stream = new StreamWriter(filename, true, Encoding.UTF8);

                LogToFile(stream, now, eventType, eventText);
            }
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
