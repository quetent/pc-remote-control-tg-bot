using Telegram.Bot.Types;
using static System.Console;

namespace RemoteControlBot
{
    internal static class Log
    {
        public static ConsoleColor ExceptionColor { get; set; } = ConsoleColor.DarkRed;
        public static ConsoleColor ExecuteCommandColor { get; set; } = ConsoleColor.DarkBlue;
        public static ConsoleColor InfoColor { get; set; } = ConsoleColor.DarkYellow;
        public static ConsoleColor NotImportantColor { get; set; } = ConsoleColor.DarkGray;
        public static ConsoleColor NeutralColor { get; set; } = ConsoleColor.White;
        public static ConsoleColor NewMessageColor { get; set; } = ConsoleColor.DarkGreen;

        internal static void BotStartup()
        {
            ByPattern("Startup", "Bot has been started", InfoColor);
        }

        internal static void MessageRecieved(string messageText, User? user)
        {
            NewMessage(messageText, user, "Message recieved", NewMessageColor);
        }

        internal static void MessageSkipped(string messageText, User? user)
        {
            NewMessage(messageText, user, "Message skipped", NotImportantColor);
        }

        private static void NewMessage(string messageText, User? user, string eventText, ConsoleColor color)
        {
            var username = user?.ToString();
            var messageFrom = username == string.Empty || username is null ? "undefined" : username;

            ByPattern(eventText, $"\"{messageText}\" from {messageFrom}", color);
        }

        internal static void UnhandledException(Exception exception)
        {
            ByPattern("Unhandled exception", exception.Message, ExceptionColor);
        }

        internal static void UndefinedCommand(string commandText)
        {
            ByPattern("Undefined command skipped", commandText, NotImportantColor);
        }

        internal static void KeyboardRequest(Command command)
        {
            ByPattern("Keyboard request", command.ToString(), NotImportantColor);
        }

        internal static void CommandExecute(Command command)
        {
            ByPattern("Execute", command.ToString(), ExecuteCommandColor);
        }

        internal static void UpdateExecute(Command command, 
                                           string commandText, 
                                           bool isIndexWaiting)
        {
            if (isIndexWaiting)
                ByPattern("Kill index getted", commandText, ExecuteCommandColor);
            else if (command.Type is CommandType.Transfer)
                KeyboardRequest(command);
            else
            {
                if (command.Type is not CommandType.Undefined)
                    CommandExecute(command);
                else
                    UndefinedCommand(commandText);
            }
        }

        private static void ByPattern(string eventType, string eventText, ConsoleColor eventColor)
        {
            var now = DateTimeManager.GetCurrentDateTime();

            WriteLine($"({now})");
            SetForegroundColor(eventColor);
            WriteLine($"[ {eventType} ]");
            ResetForegroundColor();
            WriteLine($"{eventText}");

            WriteLine();
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
