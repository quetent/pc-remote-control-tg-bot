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

        internal static CommandInfo DetermineCommandInfo(CommandType commandType, string commandText)
        {
            return commandType switch
            {
                CommandType.Undefined => CommandInfo.Null,
                CommandType.Transfer => CommandInfo.Null,
                CommandType.Power => DeterminePowerCommandInfo(commandText),
                CommandType.Volume => DetermineVolumeCommandInfo(commandText),
                CommandType.Screen => DetermineScreenCommandInfo(commandText),
                _ => throw new NotImplementedException()
            };
        }

        private static CommandInfo DeterminePowerCommandInfo(string commandText)
        {
            return commandText switch
            {
                SHUTDOWN => CommandInfo.Shutdown,
                HIBERNATE => CommandInfo.Hibernate,
                LOCK => CommandInfo.Lock,
                RESTART => CommandInfo.Restart,
                _ => throw new NotImplementedException()
            };
        }

        private static CommandInfo DetermineVolumeCommandInfo(string commandText)
        {
            return commandText switch
            {
                LOUDER_5 => CommandInfo.Louder5,
                QUIETER_5 => CommandInfo.Quieter5,
                LOUDER_10 => CommandInfo.Louder10,
                QUIETER_10 => CommandInfo.Quieter10,
                MAX => CommandInfo.Max,
                MIN => CommandInfo.Min,
                MUTE => CommandInfo.Mute,
                UNMUTE => CommandInfo.Unmute,
                _ => throw new NotImplementedException()
            };
        }

        private static CommandInfo DetermineScreenCommandInfo(string commandText)
        {
            return commandText switch
            {
                SCREENSHOT => CommandInfo.Screenshot,
                _ => throw new NotImplementedException()
            };
        }

        internal static void ExecuteCommand(CommandType commandType, CommandInfo commandInfo)
        {
            if (commandType is CommandType.Undefined)
                return;

            if (commandType is CommandType.Transfer)
                return;

            if (commandType is CommandType.Power)
                ExecutePowerCommand(commandInfo);
            else if (commandType is CommandType.Volume)
                ExecuteVolumeCommand(commandInfo);
            else if (commandType is CommandType.Screen)
                ExecuteScreenCommand(commandInfo);
            else
                throw new NotImplementedException();
        }

        private static void ExecutePowerCommand(CommandInfo commandInfo)
        {
            switch (commandInfo)
            {
                case CommandInfo.Shutdown:
                    ShutdownPC();
                    break;
                case CommandInfo.Hibernate:
                    HibernatePC();
                    break;
                case CommandInfo.Lock:
                    LockPC();
                    break;
                case CommandInfo.Restart:
                    RestartPC();
                    break;
                default:
                    throw new NotImplementedException();
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

        private static void ExecuteVolumeCommand(CommandInfo commandInfo)
        {
            switch (commandInfo)
            {
                case CommandInfo.Louder5:
                    VolumeManager.ChangeVolume(5);
                    break;
                case CommandInfo.Quieter5:
                    VolumeManager.ChangeVolume(-5);
                    break;
                case CommandInfo.Louder10:
                    VolumeManager.ChangeVolume(10);
                    break;
                case CommandInfo.Quieter10:
                    VolumeManager.ChangeVolume(-10);
                    break;
                case CommandInfo.Max:
                    VolumeManager.ChangeVolume(100);
                    break;
                case CommandInfo.Min:
                    VolumeManager.ChangeVolume(-100);
                    break;
                case CommandInfo.Mute:
                    VolumeManager.Mute();
                    break;
                case CommandInfo.Unmute:
                    VolumeManager.UnMute();
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        private static void ExecuteScreenCommand(CommandInfo commandInfo)
        {
            switch (commandInfo)
            {
                case CommandInfo.Screenshot:
                    DoAndSaveScreenshot(PathManager.GetScreenshotAbsolutePath(), SCREENSHOT_FORMAT);
                    break;
                default:
                    throw new NotImplementedException();
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
