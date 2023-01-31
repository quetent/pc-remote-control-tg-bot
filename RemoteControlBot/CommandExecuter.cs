using static RemoteControlBot.BotFunctions;
using static RemoteControlBot.Keyboard;

namespace RemoteControlBot
{
    internal class CommandExecuter
    {
        internal static string? DetermineCommandType(string command)
        {
            string? commandType;

            if (MAIN_MENU_LABELS.Contains(command) || command == BACK_LABEL)
                commandType = BACK_LABEL;
            else if (POWER_LABELS.Contains(command))
                commandType = POWER_LABEL;
            else if (VOLUME_LABELS.Contains(command))
                commandType = VOLUME_LABEL;
            else if (SCREEN_LABELS.Contains(command))
                commandType = SCREEN_LABEL;
            else
                commandType = null;

            return commandType;
        }

        internal static void ExecuteCommand(string? commandType, string commandText)
        {
            if (commandType is null)
            {
                if (ENABLE_LOGGING)
                    Log.UnknownCommand(commandText);

                return;
            }

            if (commandType == BACK_LABEL)
            {
                if (ENABLE_LOGGING)
                    Log.KeyboardRequest();

                return;
            }

            if (commandType == VOLUME_LABEL)
                ExecuteVolumeCommand(commandText);
            //else if (commandType == SCREEN_LABEL)
            //else if (commandType == POWER_LABEL)

            Log.CommandExecute(commandText);
        }

        private static void ExecuteVolumeCommand(string textMessage)
        {
            switch (textMessage)
            {
                case LOUDER_5:
                    VolumeManager.ChangeVolume(5);
                    break;
                case QUIETER_5:
                    VolumeManager.ChangeVolume(-5);
                    break;
                case LOUDER_10:
                    VolumeManager.ChangeVolume(10);
                    break;
                case QUIETER_10:
                    VolumeManager.ChangeVolume(-10);
                    break;
                case MAX:
                    VolumeManager.ChangeVolume(100);
                    break;
                case MIN:
                    VolumeManager.ChangeVolume(-100);
                    break;
                case MUTE:
                    VolumeManager.Mute();
                    break;
                case UNMUTE:
                    VolumeManager.UnMute();
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
