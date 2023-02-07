using static RemoteControlBot.BotFunctions;

namespace RemoteControlBot
{
    public class StartUpAnswerGenerator : IAnswerable
    {
        private readonly StartUpCode _startUpCode;

        public StartUpAnswerGenerator(StartUpCode startUpCode)
        {
            _startUpCode = startUpCode;
        }

        public string GetAnswer()
        {
            return _startUpCode switch
            {
                StartUpCode.Null => GetBotStartedAnswer(),
                StartUpCode.Crashed => GetAppCrashedAnwer(),
                StartUpCode.RestartRequested => GetAppRestartedAnswer(),
                _ => Throw.NotImplemented<string>($"{nameof(StartUpAnswerGenerator)} -> {_startUpCode}")
            };
        }

        private string GetBotStartedAnswer()
        {
            return "Bot was started";
        }

        private string GetAppCrashedAnwer()
        {
            return "Bot was restarted due unhandled error";
        }

        private string GetAppRestartedAnswer()
        {
            return "Bot was restarted";
        }
    }

    public class TextAnswerGenerator : IAnswerable
    {
        private readonly Command _command;

        public TextAnswerGenerator(Command command)
        {
            _command = command;
        }

        public string GetAnswer()
        {
            return _command.Type switch
            {
                CommandType.Undefined => new UndefinedAnswerGenerator(_command).GetAnswer(),
                CommandType.Transfer => new TransferAnswerGenerator(_command).GetAnswer(),
                CommandType.AdminPanel => new AdminPanelAnswerGenerator(_command).GetAnswer(),
                CommandType.Power => new PowerAnswerGenerator(_command).GetAnswer(),
                CommandType.Volume => new VolumeAnswerGenerator(_command).GetAnswer(),
                CommandType.Screen => new ScreenAnswerGenerator(_command).GetAnswer(),
                CommandType.Process => new ProcessAnswerGenerator(_command).GetAnswer(),
                _ => Throw.NotImplemented<string>($"{nameof(TextAnswerGenerator)} -> {_command}")
            };
        }
    }

    public class UndefinedAnswerGenerator : IAnswerable
    {
        public UndefinedAnswerGenerator(Command command)
        {
            Throw.IfIncorrectCommandType(command, CommandType.Undefined);
        }

        public string GetAnswer()
        {
            return "Undefined command";
        }
    }

    public class TransferAnswerGenerator
    {
        private readonly CommandInfo _commandInfo;

        public TransferAnswerGenerator(Command command)
        {
            Throw.IfIncorrectCommandType(command, CommandType.Transfer);

            _commandInfo = command.Info;
        }

        public string GetAnswer()
        {
            return _commandInfo switch
            {
                CommandInfo.ToKillList => GetProcessesListAnswer(),
                _ => GetDefaultAnswer()
            };
        }

        private static string GetDefaultAnswer()
        {
            return "...";
        }

        private static string GetProcessesListAnswer()
        {
            var counter = 1;
            var result = string.Empty;

            foreach (var process in ProcessManager.VisibleProcesses)
            {
                result += $"{counter}. {process.ProcessName}\n";
                counter++;
            }

            if (result == string.Empty)
                result = GetNoVisibleProccessesFoundAnswer();

            return result;
        }

        private static string GetNoVisibleProccessesFoundAnswer()
        {
            return "No visible proccesses found";
        }
    }

    public class AdminPanelAnswerGenerator : IAnswerable
    {
        private readonly CommandInfo _commandInfo;

        public AdminPanelAnswerGenerator(Command command)
        {
            Throw.IfIncorrectCommandType(command, CommandType.Process);

            _commandInfo = command.Info;
        }

        public string GetAnswer()
        {
            return _commandInfo switch
            {
                CommandInfo.Shutdown => Throw.ShouldBeNotReachable<string>(),
                CommandInfo.BotRestart => Throw.ShouldBeNotReachable<string>(),
                _ => Throw.NotImplemented<string>($"{nameof(AdminPanelAnswerGenerator)} -> {_commandInfo}")
            };
        }
    }

    public class PowerAnswerGenerator : IAnswerable
    {
        private readonly CommandInfo _commandInfo;

        public PowerAnswerGenerator(Command command)
        {
            Throw.IfIncorrectCommandType(command, CommandType.Power);

            _commandInfo = command.Info;
        }

        public string GetAnswer()
        {
            return _commandInfo switch
            {
                CommandInfo.Shutdown => GetShutdownRequestedAnswer(),
                CommandInfo.Hibernate => GetHibernateRequestedAnswer(),
                CommandInfo.Lock => GetLockRequestedAnswer(),
                CommandInfo.Restart => GetRestartRequestedAnswer(),
                _ => Throw.NotImplemented<string>($"{nameof(PowerAnswerGenerator)} -> {_commandInfo}")
            };
        }

        private static string GetShutdownRequestedAnswer()
        {
            return "Shutdown has been requested";
        }

        private static string GetHibernateRequestedAnswer()
        {
            return "Hibernate has been requested";
        }

        private static string GetLockRequestedAnswer()
        {
            return "Lock has been requested";
        }

        private static string GetRestartRequestedAnswer()
        {
            return "Restart has been requested";
        }
    }

    public class VolumeAnswerGenerator : IAnswerable
    {
        private readonly CommandInfo _commandInfo;

        public VolumeAnswerGenerator(Command command)
        {
            Throw.IfIncorrectCommandType(command, CommandType.Volume);

            _commandInfo = command.Info;
        }

        public string GetAnswer()
        {
            return _commandInfo switch
            {
                CommandInfo.Louder5 => GetVolumeChangeAnswer("increased", 5),
                CommandInfo.Quieter5 => GetVolumeChangeAnswer("decreased", -5),
                CommandInfo.Louder10 => GetVolumeChangeAnswer("increased", 10),
                CommandInfo.Quieter10 => GetVolumeChangeAnswer("decreased", -10),
                CommandInfo.Max => GetVolumeIsMaxAnswer(),
                CommandInfo.Min => GetVolumeIsMinAnswer(),
                CommandInfo.Mute => GetMuteRequestAnswer(MUTE, "already"),
                CommandInfo.Unmute => GetMuteRequestAnswer(UNMUTE, "is not"),
                _ => Throw.NotImplemented<string>($"{nameof(VolumeAnswerGenerator)} -> {_commandInfo}")
            };
        }

        private static string GetVolumeIsMaxAnswer()
        {
            return "Volume is set to max (100)";
        }

        private static string GetVolumeIsMinAnswer()
        {
            return "Volume is set to min (0)";
        }

        private static string GetVolumeChangeAnswer(string change, int changeLevel)
        {
            var volumeLevel = VolumeManager.VolumeLevel;
            string answer;

            if (volumeLevel == 100)
                answer = GetVolumeIsMaxAnswer();
            else if (volumeLevel == 0)
                answer = GetVolumeIsMinAnswer();
            else
            {
                var (previous, current) = (volumeLevel - changeLevel, volumeLevel);

                answer = $"Volume {change} ({previous} -> {current})";
            }

            return answer;
        }

        private static string GetMuteRequestAnswer(string requestType, string caseBad)
        {
            string insertion;
            bool isBadMuteRequest = VolumeManager.IsBadMuteRequest;

            if (isBadMuteRequest)
                insertion = $"{caseBad} ";
            else
                insertion = "has been "
                        + (requestType == UNMUTE ? "un" : string.Empty);

            return $"Speakers {insertion}muted";
        }
    }

    public class ScreenAnswerGenerator : IAnswerable
    {
        private readonly CommandInfo _commandInfo;

        public ScreenAnswerGenerator(Command command)
        {
            Throw.IfIncorrectCommandType(command, CommandType.Screen);

            _commandInfo = command.Info;
        }

        public string GetAnswer()
        {
            return _commandInfo switch
            {
                CommandInfo.Screenshot => GetScreenshotDoneAnswer(),
                _ => Throw.NotImplemented<string>($"{nameof(ScreenAnswerGenerator)} -> {_commandInfo}")
            };
        }

        private static string GetScreenshotDoneAnswer()
        {
            return "Screenshot was taken";
        }
    }

    public class ProcessAnswerGenerator : IAnswerable
    {
        private readonly CommandInfo _commandInfo;

        public ProcessAnswerGenerator(Command command)
        {
            Throw.IfIncorrectCommandType(command, CommandType.Process);

            _commandInfo = command.Info;
        }

        public string GetAnswer()
        {
            return _commandInfo switch
            {
                CommandInfo.Kill => GetProcessKillingResultAnswer(),
                _ => Throw.NotImplemented<string>($"{nameof(ProcessAnswerGenerator)} -> {_commandInfo}")
            };
        }

        private static string GetProcessKillingResultAnswer()
        {
            string result;

            if (ProcessManager.IsSuccessfulLastKill)
                result = GetProcessKilledAnswer();
            else
            {
                if (ProcessManager.IsLastKillSystemProcess)
                    result = GetProcessIsSystemAnwer();
                else if (ProcessManager.IsValidLastIndex)
                    result = GetProcessFinishedAnswer();
                else
                    result = GetInvalidKillingIndexAnswer();
            }

            return result;
        }

        private static string GetInvalidKillingIndexAnswer()
        {
            return "Invalid index";
        }

        private static string GetProcessKilledAnswer()
        {
            return "Process was killed";
        }

        private static string GetProcessFinishedAnswer()
        {
            return "Process already finished";
        }

        private static string GetProcessIsSystemAnwer()
        {
            return "Cannot kill system process";
        }
    }
}
