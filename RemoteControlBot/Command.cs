using static RemoteControlBot.BotFunctions;
using static RemoteControlBot.Keyboard;

namespace RemoteControlBot
{
    internal readonly struct Command
    {
        private readonly CommandType _type;
        private readonly CommandInfo _info;

        public CommandType Type => _type;
        public CommandInfo Info => _info;

        public Command(string commandText)
        {
            _type = DetermineCommandType(commandText);
            _info = DetermineCommandInfo(_type, commandText);
        }

        public override string ToString()
        {
            return $"{_type} -> {_info}";
        }

        private static CommandType DetermineCommandType(string command)
        {
            CommandType commandType;

            if (MAIN_MENU_LABELS.Contains(command) || command == BACK_LABEL)
                commandType = CommandType.Transfer;
            else if (POWER_LABELS.Contains(command))
                commandType = CommandType.Power;
            else if (VOLUME_LABELS.Contains(command))
                commandType = CommandType.Volume;
            else if (SCREEN_LABELS.Contains(command))
                commandType = CommandType.Screen;
            else
                commandType = CommandType.Undefined;

            return commandType;
        }

        private static CommandInfo DetermineCommandInfo(CommandType commandType, string commandText)
        {
            return commandType switch
            {
                CommandType.Undefined => CommandInfo.Null,
                CommandType.Transfer => CommandInfo.Null,
                CommandType.Power => DeterminePowerCommandInfo(commandText),
                CommandType.Volume => DetermineVolumeCommandInfo(commandText),
                CommandType.Screen => DetermineScreenCommandInfo(commandText),
                _ => Throw.CommandNotImplemented<CommandInfo>(commandText)
            };
        }

        private static CommandInfo DeterminePowerCommandInfo(string commandText)
        {
            return commandText switch
            {
                SHUTDOWN => CommandInfo.Shutdown,
                HIBERNATE => CommandInfo.Hibernate,
                LOCK => CommandInfo.Lock,
                RESTART => CommandInfo.Restart,
                _ => Throw.CommandNotImplemented<CommandInfo>(commandText)
            };
        }

        private static CommandInfo DetermineVolumeCommandInfo(string commandText)
        {
            return commandText switch
            {
                LOUDER_5 => CommandInfo.Louder5,
                QUIETER_5 => CommandInfo.Quieter5,
                LOUDER_10 => CommandInfo.Louder10,
                QUIETER_10 => CommandInfo.Quieter10,
                MAX => CommandInfo.Max,
                MIN => CommandInfo.Min,
                MUTE => CommandInfo.Mute,
                UNMUTE => CommandInfo.Unmute,
                _ => Throw.CommandNotImplemented<CommandInfo>(commandText)
            };
        }

        private static CommandInfo DetermineScreenCommandInfo(string commandText)
        {
            return commandText switch
            {
                SCREENSHOT => CommandInfo.Screenshot,
                _ => Throw.CommandNotImplemented<CommandInfo>(commandText)
            };
        }
    }
}
