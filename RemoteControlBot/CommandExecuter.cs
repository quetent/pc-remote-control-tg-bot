using System.Diagnostics;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace RemoteControlBot
{
    internal static class CommandExecuter
    {
        internal delegate Task CommandHandler(Command command, long commandSenderId, CancellationToken cancellation);
        internal static event CommandHandler? CommandExecuted;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool LockWorkStation();

        internal static async Task ExecuteCommandAsync(Command command, long commandSenderId, CancellationToken cancellationToken)
        {
            Task task;

            task = command.Type switch
            {
                CommandType.Undefined => Task.CompletedTask,
                CommandType.Transfer => Task.Run(() => ExecuteTransferCommand(command), cancellationToken),
                CommandType.AdminPanel => Task.Run(() => ExecuteControlCommand(command), cancellationToken),
                CommandType.Power => Task.Run(() => ExecutePowerCommand(command), cancellationToken),
                CommandType.Volume => Task.Run(() => ExecuteVolumeCommand(command), cancellationToken),
                CommandType.Screen => Task.Run(() => ExecuteScreenCommand(command), cancellationToken),
                CommandType.Process => Task.Run(() => ExecuteProcessCommand(command), cancellationToken),
                _ => Throw.CommandNotImplemented<Task>(command)
            };

            await task;
            CommandExecuted?.Invoke(command, commandSenderId, cancellationToken);
        }

        private static void ExecuteTransferCommand(Command command)
        {
            Throw.IfIncorrectCommandType(command, CommandType.Transfer);

            switch (command.Info)
            {
                case CommandInfo.ToKillList:
                    ProcessManager.SetVisibleProcceses();
                    break;
                default:
                    break;
            }
        }

        private static void ExecuteControlCommand(Command command)
        {
            Throw.IfIncorrectCommandType(command, CommandType.AdminPanel);

            switch (command.Info)
            {
                case CommandInfo.BotTurnOff:
                    ExitApp();
                    break;
                default:
                    Throw.CommandNotImplemented(command);
                    break;
            }
        }

        private static void ExitApp()
        {
            Environment.Exit(0);
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
                    VolumeManager.ChangeVolumeLevel(5);
                    break;
                case CommandInfo.Quieter5:
                    VolumeManager.ChangeVolumeLevel(-5);
                    break;
                case CommandInfo.Louder10:
                    VolumeManager.ChangeVolumeLevel(10);
                    break;
                case CommandInfo.Quieter10:
                    VolumeManager.ChangeVolumeLevel(-10);
                    break;
                case CommandInfo.Max:
                    VolumeManager.ChangeVolumeLevel(100);
                    break;
                case CommandInfo.Min:
                    VolumeManager.ChangeVolumeLevel(-100);
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
                    ProcessManager.TryKillProcessByIndex(int.Parse(command.RawText) - 1);
                    break;
                default:
                    Throw.CommandNotImplemented(command);
                    break;
            }
        }
    }
}
