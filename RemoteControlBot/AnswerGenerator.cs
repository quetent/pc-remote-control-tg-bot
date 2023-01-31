using static RemoteControlBot.BotFunctions;

namespace RemoteControlBot
{
    internal static class AnswerGenerator
    {
        internal static string GetTextAnswerByPowerCommand(string commandText)
        {
            string answer;

            if (commandText == SHUTDOWN)
                answer = "Shutdown has been requested";
            else if (commandText == RESTART)
                answer = "Restart has been requested";
            else if (commandText == SLEEP)
                answer = "Sleep has been requested";
            else
                answer = GetBotFunctionNotImplementedAnswer();

            return answer;
        }

        internal static string GetTextAnswerByScreenCommand(string commandText)
        {
            string answer;

            if (commandText == SCREENSHOT)
                answer = "Screenshot was taken";
            else
                answer = GetBotFunctionNotImplementedAnswer();

            return answer;
        }

        internal static string GetTextAnswerByVolumeCommand(string commandText)
        {
            string answer;

            switch (commandText)
            {
                case LOUDER_5:
                    answer = GetVolumeChangeAnswer("increased", 5);
                    break;
                case QUIETER_5:
                    answer = GetVolumeChangeAnswer("decreased", -5);
                    break;
                case LOUDER_10:
                    answer = GetVolumeChangeAnswer("increased", 10);
                    break;
                case QUIETER_10:
                    answer = GetVolumeChangeAnswer("decreased", -10);
                    break;
                case MAX:
                    answer = GetVolumeIsMaxAnswer();
                    break;
                case MIN:
                    answer = GetVolumeIsMinAnswer();
                    break;
                case MUTE:
                    answer = GetMuteRequestAnswer(MUTE, "already");
                    break;
                case UNMUTE:
                    answer = GetMuteRequestAnswer(UNMUTE, "is not");
                    break;
                default:
                    answer = GetBotFunctionNotImplementedAnswer();
                    break;
            }

            return answer;
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

            return $"Speaker {insertion}muted";
        }

        private static string GetBotFunctionNotImplementedAnswer()
        {
            return "Selected function is not implemented";
        }
    }
}
