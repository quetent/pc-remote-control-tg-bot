using static RemoteControlBot.BotFunctions;

namespace RemoteControlBot
{
    internal static class AnswerGenerator
    {
        internal static string GetBotFunctionNotImplementedAnswer()
        {
            return "Selected function is not implemented";
        }

        internal static string GetTextAnswerByPowerCommand(CommandInfo commandText)
        {
            return commandText switch
            {
                CommandInfo.Shutdown => "Shutdown has been requested",
                CommandInfo.Hibernate => "Hibernate has been requested",
                CommandInfo.Lock => "Lock has been requested",
                CommandInfo.Restart => "Restart has been requested",
                _ => GetBotFunctionNotImplementedAnswer()
            };
        }

        internal static string GetTextAnswerByScreenCommand(CommandInfo commandInfo)
        {
            return commandInfo switch
            {
                CommandInfo.Screenshot => "Screenshot was taken",
                _ => GetBotFunctionNotImplementedAnswer()
            };
        }

        internal static string GetTextAnswerByVolumeCommand(CommandInfo commandInfo)
        {
            return commandInfo switch
            {
                CommandInfo.Louder5 => GetVolumeChangeAnswer("increased", 5),
                CommandInfo.Quieter5 => GetVolumeChangeAnswer("decreased", -5),
                CommandInfo.Louder10 => GetVolumeChangeAnswer("increased", 10),
                CommandInfo.Quieter10 => GetVolumeChangeAnswer("decreased", -10),
                CommandInfo.Max => GetVolumeIsMaxAnswer(),
                CommandInfo.Min => GetVolumeIsMinAnswer(),
                CommandInfo.Mute => GetMuteRequestAnswer(MUTE, "already"),
                CommandInfo.Unmute => GetMuteRequestAnswer(UNMUTE, "is not"),
                _ => GetBotFunctionNotImplementedAnswer(),
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
