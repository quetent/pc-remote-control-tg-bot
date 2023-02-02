using static RemoteControlBot.BotFunctions;

namespace RemoteControlBot
{
    internal static class AnswerGenerator
    {
        internal static string GetTextAnswerByPowerCommand(Command command)
        {
            Throw.IfIncorrectCommandType(command, CommandType.Power);

            return command.Info switch
            {
                CommandInfo.Shutdown => "Shutdown has been requested",
                CommandInfo.Hibernate => "Hibernate has been requested",
                CommandInfo.Lock => "Lock has been requested",
                CommandInfo.Restart => "Restart has been requested",
                _ => Throw.CommandNotImplemented<string>(command)
            };
        }

        internal static string GetTextAnswerByVolumeCommand(Command command)
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
                _ => Throw.CommandNotImplemented<string>(command)
            };
        }

        internal static string GetTextAnswerByScreenCommand(Command command)
        {
            Throw.IfIncorrectCommandType(command, CommandType.Screen);

            return command.Info switch
            {
                CommandInfo.Screenshot => "Screenshot was taken",
                _ => Throw.CommandNotImplemented<string>(command)
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
            var volumeLevel = VolumeManager.GetCurrentVolumeLevel();
            string answer;

            if (volumeLevel == 100)
                answer = GetVolumeIsMaxAnswer();
            else if (volumeLevel == 0)
                answer = GetVolumeIsMinAnswer();
            else
            {
                int previous, current;

                if (changeLevel > 0)
                    (previous, current) = (volumeLevel - changeLevel, volumeLevel);
                else
                    (previous, current) = (volumeLevel - changeLevel, volumeLevel);

                answer = $"Volume {change} ({previous} -> {current})";
            }

            return answer;
        }

        private static string GetMuteRequestAnswer(string requestType, string caseBad)
        {
            string insertion;
            bool isBadMuteRequest = VolumeManager.IsBadMuteRequest();

            if (isBadMuteRequest)
                insertion = $"{caseBad} ";
            else
                insertion = "has been "
                        + (requestType == UNMUTE ? "un" : string.Empty);

            return $"Speakers {insertion}muted";
        }
    }
}
