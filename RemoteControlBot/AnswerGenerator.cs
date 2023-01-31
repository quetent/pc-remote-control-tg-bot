using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
                throw new NotImplementedException();

            return answer;
        }

        internal static string GetTextAnswerByScreenCommand(string commandText)
        {
            string answer;

            if (commandText == SCREENSHOT)
                answer = "Screenshot was taken";
            else
                throw new NotImplementedException();

            return answer;
        }

        internal static string GetTextAnswerByVolumeCommand(string commandText)
        {
            string answer;
            int volumeLevel;

            switch (commandText)
            {
                case LOUDER_5:
                    volumeLevel = VolumeManager.GetCurrentVolumeLevel();
                    if (volumeLevel == 100)
                        goto case MAX;
                    answer = GetVolumeChangeAnswer("increased", volumeLevel - 5, volumeLevel);
                    break;
                case QUIETER_5:
                    volumeLevel = VolumeManager.GetCurrentVolumeLevel();
                    if (volumeLevel == 0)
                        goto case MIN;
                    answer = GetVolumeChangeAnswer("decreased", volumeLevel + 5, volumeLevel);
                    break;
                case LOUDER_10:
                    volumeLevel = VolumeManager.GetCurrentVolumeLevel();
                    if (volumeLevel == 100)
                        goto case MAX;
                    answer = GetVolumeChangeAnswer("increased", volumeLevel - 10, volumeLevel);
                    break;
                case QUIETER_10:
                    volumeLevel = VolumeManager.GetCurrentVolumeLevel();
                    if (volumeLevel == 0)
                        goto case MIN;
                    answer = GetVolumeChangeAnswer("decreased", volumeLevel + 10, volumeLevel);
                    break;
                case MAX:
                    answer = "Volume is set to max (100)";
                    break;
                case MIN:
                    answer = "Volume is set to min (0)";
                    break;
                case MUTE:
                    answer = GetMuteRequestAnswer(MUTE, "already");
                    break;
                case UNMUTE:
                    answer = GetMuteRequestAnswer(UNMUTE, "is not");
                    break;
                default:
                    throw new NotImplementedException();
            }

            return answer;
        }

        private static string GetVolumeChangeAnswer(string change, int previous, int current)
        {
            return $"Volume {change} ({previous} -> {current})";
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
    }
}
