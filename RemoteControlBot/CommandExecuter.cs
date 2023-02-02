using System.Diagnostics;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace RemoteControlBot
{
    internal static class CommandExecuter
    {
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool LockWorkStation();

        internal static void ExecuteCommand(Command command)
        {
            switch (command.Type)
            {
                case CommandType.Undefined:
                    break;
                case CommandType.Transfer:
                    break;
                case CommandType.Power:
                    ExecutePowerCommand(command);
                    break;
                case CommandType.Volume:
                    ExecuteVolumeCommand(command);
                    break;
                case CommandType.Screen:
                    ExecuteScreenCommand(command);
                    break;
                case CommandType.Process:
                    ExecuteProcessCommand(command);
                    break;
                default:
                    Throw.CommandNotImplemented(command);
                    break;
            }
        }

        private static void ExecutePowerCommand(Command command)
        {
            Throw.IfIncorrectCommandType(command, CommandType.Power);

            switch (command.Info)
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
                    Throw.CommandNotImplemented(command);
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

        private static void ExecuteVolumeCommand(Command command)
        {
            Throw.IfIncorrectCommandType(command, CommandType.Volume);

            switch (command.Info)
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
                    Throw.CommandNotImplemented(command);
                    break;
            }
        }

        private static void ExecuteScreenCommand(Command command)
        {
            Throw.IfIncorrectCommandType(command, CommandType.Screen);

            switch (command.Info)
            {
                case CommandInfo.Screenshot:
                    DoAndSaveScreenshot(PathManager.GetScreenshotAbsolutePath(), SCREENSHOT_FORMAT);
                    break;
                default:
                    Throw.CommandNotImplemented(command);
                    break;
            }
        }

        private static void DoAndSaveScreenshot(string filepath, ImageFormat imageFormat)
        {
            var size = ScreenManager.GetMonitorSize();
            using var screenshot = ScreenManager.DoScreenshot(size);

            var fileFormat = imageFormat;
            var fileFormatAsString = fileFormat.ToString().ToLowerInvariant();

            ScreenManager.SaveScreenshot(screenshot, fileFormat, filepath);
        }

        internal static void ExecuteProcessCommand(Command command)
        {
            Throw.IfIncorrectCommandType(command, CommandType.Process);

            switch (command.Info)
            {
                case CommandInfo.Kill:
                    SetVisibleProcceses();
                    break;
                default:
                    Throw.CommandNotImplemented(command);
                    break;
            }
        }

        private static void SetVisibleProcceses()
        {
            ProcessManager.SetVisibleProcceses();
        }
    }
}
