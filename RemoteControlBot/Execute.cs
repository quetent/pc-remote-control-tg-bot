using System.Drawing.Imaging;

namespace RemoteControlBot
{
    internal static class Execute
    {
        internal delegate Task CommandHandler(Command command, long commandSenderId, CancellationToken cancellation);
        internal static event CommandHandler? CommandExecuted;

        internal static async Task ExecuteAsync(Command command, long commandSenderId, CancellationToken cancellationToken)
        {
            Task task;

            task = command.Type switch
            {
                CommandType.Undefined => Task.CompletedTask,
                CommandType.Transfer => Task.Run(() => new TransferExecute(command).Execute(), cancellationToken),
                CommandType.AdminPanel => Task.Run(() => new AdminPanelExecute(command).Execute(), cancellationToken),
                CommandType.Power => Task.Run(() => new PowerExecute(command).Execute(), cancellationToken),
                CommandType.Volume => Task.Run(() => new VolumeExecute(command).Execute(), cancellationToken),
                CommandType.Screen => Task.Run(() => new ScreenExecute(command).Execute(), cancellationToken),
                CommandType.Process => Task.Run(() => new ProcessExecute(command).Execute(), cancellationToken),
                _ => Throw.NotImplemented<Task>(command.ToString())
            };

            await task;
            CommandExecuted?.Invoke(command, commandSenderId, cancellationToken);
        }
    }

    public class TransferExecute : IExecutable
    {
        private readonly CommandInfo _commandInfo;

        public TransferExecute(Command command)
        {
            Throw.IfIncorrectCommandType(command, CommandType.Transfer);

            _commandInfo = command.Info;
        }

        public void Execute()
        {
            switch (_commandInfo)
            {
                case CommandInfo.ToKillList:
                    ProcessManager.SetVisibleProcceses();
                    break;
                default:
                    break;
            }
        }
    }

    public class AdminPanelExecute : IExecutable
    {
        private readonly CommandInfo _commandInfo;

        public AdminPanelExecute(Command command)
        {
            Throw.IfIncorrectCommandType(command, CommandType.AdminPanel);

            _commandInfo = command.Info;
        }

        public void Execute()
        {
            switch (_commandInfo)
            {
                case CommandInfo.BotTurnOff:
                    ExitApp();
                    break;
                case CommandInfo.BotRestart:
                    RequestAppRestart();
                    break;
                default:
                    Throw.NotImplemented($"{nameof(AdminPanelExecute)} -> {_commandInfo}");
                    break;
            }
        }

        private static void ExitApp()
        {
            Environment.Exit(0);
        }

        private static void RequestAppRestart()
        {
            ProcessManager.StartProcess(Environment.ProcessPath!, "LastError=Restart", false);

            ExitApp();
        }
    }

    public class PowerExecute : IExecutable
    {
        private readonly CommandInfo _commandInfo;

        public PowerExecute(Command command)
        {
            Throw.IfIncorrectCommandType(command, CommandType.Power);

            _commandInfo = command.Info;
        }

        public void Execute()
        {
            switch (_commandInfo)
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
                    Throw.NotImplemented($"{nameof(PowerExecute)} -> {_commandInfo}");
                    break;
            }
        }

        private static void ShutdownPC()
        {
            ProcessManager.StartProcess("shutdown.exe", "/s /f /t 0", true);
        }

        private static void HibernatePC()
        {
            ProcessManager.StartProcess("shutdown.exe", "/h", true);
        }

        private static void RestartPC()
        {
            ProcessManager.StartProcess("shutdown.exe", "/r /f /t 0", true);
        }

        private static void LockPC()
        {
            NativeMethods.LockPC();
        }
    }

    public class VolumeExecute : IExecutable
    {
        private readonly CommandInfo _commandInfo;

        public VolumeExecute(Command command)
        {
            Throw.IfIncorrectCommandType(command, CommandType.Volume);

            _commandInfo = command.Info;
        }

        public void Execute()
        {
            switch (_commandInfo)
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
                    Throw.NotImplemented($"{nameof(VolumeExecute)} -> {_commandInfo}");
                    break;
            }
        }
    }

    public class ScreenExecute : IExecutable
    {
        private readonly CommandInfo _commandInfo;

        public ScreenExecute(Command command)
        {
            Throw.IfIncorrectCommandType(command, CommandType.Screen);

            _commandInfo = command.Info;
        }

        public void Execute()
        {
            switch (_commandInfo)
            {
                case CommandInfo.Screenshot:
                    DoAndSaveScreenshot(PathManager.GetScreenshotAbsolutePath(), SCREENSHOT_FORMAT);
                    break;
                default:
                    Throw.NotImplemented($"{nameof(VolumeExecute)} -> {_commandInfo}");
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
    }

    public class ProcessExecute : IExecutable
    {
        private readonly CommandInfo _commandInfo;
        private readonly string _commandText;

        public ProcessExecute(Command command)
        {
            Throw.IfIncorrectCommandType(command, CommandType.Process);

            _commandInfo = command.Info;
            _commandText = command.RawText;
        }

        public void Execute()
        {
            switch (_commandInfo)
            {
                case CommandInfo.Kill:
                    ProcessManager.TryKillProcessByIndex(int.Parse(_commandText) - 1);
                    break;
                default:
                    Throw.NotImplemented($"{nameof(ProcessExecute)} -> {_commandInfo}");
                    break;
            }
        }
    }
}
