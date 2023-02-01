using System.Diagnostics;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using static RemoteControlBot.BotFunctions;
using static RemoteControlBot.Keyboard;

namespace RemoteControlBot
{
    internal static class CommandExecuter
    {
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool LockWorkStation();

        internal static CommandType DetermineCommandType(string command)
        {
            CommandType commandType;

            if (MAIN_MENU_LABELS.Contains(command) || command == BACK_LABEL)
                commandType = CommandType.Transfer;
            else if (POWER_LABELS.Contains(command))
                commandType = CommandType.Power;
            else if (VOLUME_LABELS.Contains(command))
                commandType = CommandType.Volume;
            else if (SCREEN_LABELS.Contains(command))
                commandType = CommandType.Screen;
            else
                commandType = CommandType.Undefined;

            return commandType;
        }

        internal static void ExecuteCommand(CommandType commandType, string commandText)
        {
            if (commandType is CommandType.Undefined)
            {
                if (ENABLE_LOGGING)
                    Log.UnknownCommand(commandText);

                return;
            }

            if (commandType is CommandType.Transfer)
            {
                if (ENABLE_LOGGING)
                    Log.KeyboardRequest();

                return;
            }

            if (commandType is CommandType.Power)
                ExecutePowerCommand(commandText);
            else if (commandType is CommandType.Volume)
                ExecuteVolumeCommand(commandText);
            else if (commandType is CommandType.Screen)
                ExecuteScreenCommand(commandText);
            else
            {
                if (ENABLE_LOGGING)
                    Log.FunctionNotImplemented($"{commandType} -> {commandText}");

                return;
            }

            if (ENABLE_LOGGING)
                Log.CommandExecute(commandText);
        }

        private static void ExecutePowerCommand(string textMessage)
        {
            switch (textMessage)
            {
                case SHUTDOWN:
                    ShutdownPC();
                    break;
                case HIBERNATE:
                    HibernatePC();
                    break;
                case RESTART:
                    RestartPC();
                    break;
                case LOCK:
                    LockPC();
                    break;
                default:
                    if (ENABLE_LOGGING)
                        Log.FunctionNotImplemented(textMessage);
                    break;
            }
        }

        private static void StartCommandLineProcess(string command, string args)
        {
            var processInfo = new ProcessStartInfo(command, args)
            {
                CreateNoWindow = true,
                UseShellExecute = false
            };

            Process.Start(processInfo);
        }

        private static void ShutdownPC()
        {
            StartCommandLineProcess("shutdown.exe", "/s /f /t 0");
        }

        private static void HibernatePC()
        {
            StartCommandLineProcess("shutdown.exe", "/h");
        }

        private static void RestartPC()
        {
            StartCommandLineProcess("shutdown.exe", "/r /f /t 0");
        }

        private static void LockPC()
        {
            LockWorkStation();
        }

        private static void ExecuteVolumeCommand(string textMessage)
        {
            switch (textMessage)
            {
                case LOUDER_5:
                    VolumeManager.ChangeVolume(5);
                    break;
                case QUIETER_5:
                    VolumeManager.ChangeVolume(-5);
                    break;
                case LOUDER_10:
                    VolumeManager.ChangeVolume(10);
                    break;
                case QUIETER_10:
                    VolumeManager.ChangeVolume(-10);
                    break;
                case MAX:
                    VolumeManager.ChangeVolume(100);
                    break;
                case MIN:
                    VolumeManager.ChangeVolume(-100);
                    break;
                case MUTE:
                    VolumeManager.Mute();
                    break;
                case UNMUTE:
                    VolumeManager.UnMute();
                    break;
                default:
                    if (ENABLE_LOGGING)
                        Log.FunctionNotImplemented(textMessage);
                    break;
            }
        }

        private static void ExecuteScreenCommand(string textMessage)
        {
            switch (textMessage)
            {
                case SCREENSHOT:
                    DoAndSaveScreenshot(PathManager.GetScreenshotAbsolutePath(), SCREENSHOT_FORMAT);
                    break;
                default:
                    if (ENABLE_LOGGING)
                        Log.FunctionNotImplemented(textMessage);
                    break;
            }
        }

        private static void DoAndSaveScreenshot(string filepath, ImageFormat imageFormat)
        {
            var size = ScreenManager.GetMonitorSize();
            using var screenshot = ScreenManager.DoScreenshot(size);

            var fileFormat = imageFormat;
            var fileFormatAsString = fileFormat.ToString().ToLowerInvariant();

            var now = DateTimeManager.GetCurrentDateTime();

            ScreenManager.SaveScreenshot(screenshot, fileFormat, filepath);
        }
    }
}
