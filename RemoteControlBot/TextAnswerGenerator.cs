using static RemoteControlBot.BotFunctions;

namespace RemoteControlBot
{
    internal static class TextAnswerGenerator
    {
        internal static string GetAnswerByUndefinedCommand(Command command)
        {
            Throw.IfIncorrectCommandType(command, CommandType.Undefined);

            return "Undefined command";
        }

        internal static string GetAnswerByTransferCommand(Command command)
        {
            Throw.IfIncorrectCommandType(command, CommandType.Transfer);

            return command.Info switch
            {
                CommandInfo.ToKillList => GetProcessesListAnswer(),
                _ => GetTransferDefaultAnswer()
            };
        }

        private static string GetTransferDefaultAnswer()
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

        internal static string GetAnswerByPowerCommand(Command command)
        {
            Throw.IfIncorrectCommandType(command, CommandType.Power);

            return command.Info switch
            {
                CommandInfo.Shutdown => GetShutdownRequestedAnswer(),
                CommandInfo.Hibernate => GetHibernateRequestedAnswer(),
                CommandInfo.Lock => GetLockRequestedAnswer(),
                CommandInfo.Restart => GetRestartRequestedAnswer(),
                _ => Throw.NotImplemented<string>(command)
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

        internal static string GetAnswerByVolumeCommand(Command command)
        {
            Throw.IfIncorrectCommandType(command, CommandType.Volume);

            return command.Info switch
            {
                CommandInfo.Louder5 => GetVolumeChangeAnswer("increased", 5),
                CommandInfo.Quieter5 => GetVolumeChangeAnswer("decreased", -5),
                CommandInfo.Louder10 => GetVolumeChangeAnswer("increased", 10),
                CommandInfo.Quieter10 => GetVolumeChangeAnswer("decreased", -10),
                CommandInfo.Max => GetVolumeIsMaxAnswer(),
                CommandInfo.Min => GetVolumeIsMinAnswer(),
                CommandInfo.Mute => GetMuteRequestAnswer(MUTE, "already"),
                CommandInfo.Unmute => GetMuteRequestAnswer(UNMUTE, "is not"),
                _ => Throw.NotImplemented<string>(command)
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

        internal static string GetAnswerByScreenCommand(Command command)
        {
            Throw.IfIncorrectCommandType(command, CommandType.Screen);

            return command.Info switch
            {
                CommandInfo.Screenshot => GetScreenshotDoneAnswer(),
                _ => Throw.NotImplemented<string>(command)
            };
        }

        private static string GetScreenshotDoneAnswer()
        {
            return "Screenshot was taken";
        }

        internal static string GetAnswerByProcessCommand(Command command)
        {
            Throw.IfIncorrectCommandType(command, CommandType.Process);

            return command.Info switch
            {
                CommandInfo.Kill => GetProcessKillingResultAnswer(),
                _ => Throw.NotImplemented<string>(command)
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

        internal static string GetAnswerByAdminPanelCommand(Command command)
        {
            Throw.IfIncorrectCommandType(command, CommandType.AdminPanel);

            return command.Info switch
            {
                CommandInfo.Shutdown => Throw.ShouldBeNotReachable<string>(),
                CommandInfo.BotRestart => Throw.ShouldBeNotReachable<string>(),
                _ => Throw.NotImplemented<string>(command)
            };
        }
    }
}
