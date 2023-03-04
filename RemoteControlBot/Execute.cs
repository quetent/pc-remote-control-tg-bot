using System.Drawing;
using System.Drawing.Imaging;

namespace RemoteControlBot
{
    public class Execute : IAsyncExecutable
    {
        private readonly Command _command;

        public static Command LastExecutedCommand { get; private set; }

        public delegate Task CommandHandle(Command command, CancellationToken cancellation);
        public static event CommandHandle? CommandExecuted;

        public Execute(Command command)
        {
            _command = command;
        }

        public static async Task ExecuteIfAsync(Func<bool> condition, Action instructions)
        {
            if (condition.Invoke())
                await Task.Run(() => instructions.Invoke());
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            Task task;

            task = _command.Type switch
            {
                CommandType.Undefined => Task.CompletedTask,
                CommandType.Transfer => Task.Run(() => new TransferExecute(_command).Execute(), cancellationToken),
                CommandType.AdminPanel => Task.Run(() => new AdminPanelExecute(_command).Execute(), cancellationToken),
                CommandType.Power => Task.Run(() => new PowerExecute(_command).Execute(), cancellationToken),
                CommandType.Volume => Task.Run(() => new VolumeExecute(_command).Execute(), cancellationToken),
                CommandType.Screen => Task.Run(() => new ScreenExecute(_command).Execute(), cancellationToken),
                CommandType.Process => Task.Run(() => new ProcessExecute(_command).Execute(), cancellationToken),
                _ => Throw.NotImplemented<Task>($"{nameof(Execute)} -> {_command}")
            };

            await task;

            SetLastExecutedCommand(_command);
            CommandExecuted?.Invoke(_command, cancellationToken);
        }

        private static void SetLastExecutedCommand(Command command)
        {
            LastExecutedCommand = command;
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
                    ProcessManager.ScanProcesses();
                    break;
                case CommandInfo.ToScreensList:
                    ScreenManager.DetectScreens();
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
                    RequestAppExit();
                    break;
                case CommandInfo.BotRestart:
                    RequestAppRestart();
                    break;
                default:
                    Throw.NotImplemented($"{nameof(AdminPanelExecute)} -> {_commandInfo}");
                    break;
            }
        }

        private static void RequestAppExit()
        {
            throw new AppExitRequested();
        }

        private static void RequestAppRestart()
        {
            throw new AppRestartRequested();
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
        private readonly string _commandText;

        public ScreenExecute(Command command)
        {
            Throw.IfIncorrectCommandType(command, CommandType.Screen);

            _commandInfo = command.Info;
            _commandText = command.RawText;
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

        private void DoAndSaveScreenshot(string filepath, ImageFormat imageFormat)
        {
            if (ScreenManager.TryDoScreenshotByIndex(int.Parse(_commandText) - 1, out Bitmap screenshot))
                ScreenManager.SaveScreenshot(screenshot, imageFormat, filepath);
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
