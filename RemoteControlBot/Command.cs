using static RemoteControlBot.BotFunctions;
using static RemoteControlBot.Keyboard;

namespace RemoteControlBot
{
    internal readonly struct Command
    {
        private readonly CommandType _type;
        private readonly CommandInfo _info;
        private readonly string _rawText;

        internal CommandType Type => _type;
        internal CommandInfo Info => _info;
        internal string RawText => _rawText;

        internal Command(string commandText)
        {
            _type = DetermineCommandType(commandText);
            _info = DetermineCommandInfo(_type, commandText);
            _rawText = commandText;
        }

        internal Command(CommandType commandType, CommandInfo commandInfo, string rawText = "")
        {
            _type = commandType;
            _info = commandInfo;
            _rawText = rawText;
        }

        public override string ToString()
        {
            return $"{_type} -> {_info}";
        }

        private static CommandType DetermineCommandType(string commandText)
        {
            CommandType commandType;

            if (MAIN_MENU_LABELS.Contains(commandText) || commandText == BACK_LABEL)
                commandType = CommandType.Transfer;
            else if (POWER_LABELS.Contains(commandText))
                commandType = CommandType.Power;
            else if (VOLUME_LABELS.Contains(commandText))
                commandType = CommandType.Volume;
            else if (SCREEN_LABELS.Contains(commandText))
                commandType = CommandType.Screen;
            else if (PROCESS_LABELS.Contains(commandText))
                commandType = CommandType.Process;
            else
                commandType = CommandType.Undefined;

            return commandType;
        }

        private static CommandInfo DetermineCommandInfo(CommandType commandType, string commandText)
        {
            return commandType switch
            {
                CommandType.Undefined => CommandInfo.Null,
                CommandType.Transfer => DetermineTranserCommandInfo(commandText),
                CommandType.Power => DeterminePowerCommandInfo(commandText),
                CommandType.Volume => DetermineVolumeCommandInfo(commandText),
                CommandType.Screen => DetermineScreenCommandInfo(commandText),
                CommandType.Process => DetermineProcessCommandInfo(commandText),
                _ => Throw.CommandNotImplemented<CommandInfo>(commandText)
            };
        }

        private static CommandInfo DetermineTranserCommandInfo(string commandText)
        {
            return commandText switch
            {
                POWER_LABEL => CommandInfo.ToPower,
                VOLUME_LABEL => CommandInfo.ToVolume,
                SCREEN_LABEL => CommandInfo.ToScreen,
                PROCESS_LABEL => CommandInfo.ToProcess,
                BACK_LABEL => CommandInfo.ToMainMenu,
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

        private static CommandInfo DetermineProcessCommandInfo(string commandText)
        {
            return commandText switch
            {
                KILL => CommandInfo.Kill,
                _ => Throw.CommandNotImplemented<CommandInfo>(commandText)
            };
        }
    }
}
