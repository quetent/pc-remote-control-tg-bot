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

        internal Command(CommandType builderType, CommandInfo builderInfo, string commandText)
        {
            _type = builderType;
            _info = builderInfo;
            _rawText = commandText;
        }

        public override string ToString()
        {
            return $"{_type} -> {_info}";
        }

        internal static bool IsNumberForProccesManager(Command previousCommand, string messageText)
        {
            return previousCommand.Type is CommandType.Transfer
                && previousCommand.Info is CommandInfo.ToKillList
                && messageText.IsNumber();
        }

        private static CommandType DetermineCommandType(string commandText)
        {
            CommandType commandType;

            if (IsTransfer(commandText))
                commandType = CommandType.Transfer;
            else if (IsControl(commandText))
                commandType = CommandType.AdminPanel;
            else if (IsPower(commandText))
                commandType = CommandType.Power;
            else if (IsVolume(commandText))
                commandType = CommandType.Volume;
            else if (IsScreen(commandText))
                commandType = CommandType.Screen;
            else if (IsProcess(commandText))
                commandType = CommandType.Process;
            else
                commandType = CommandType.Undefined;

            return commandType;
        }

        private static bool IsTransfer(string commandText)
        {
            return MAIN_MENU_LABELS.Contains(commandText)
                || commandText == BACK_LABEL
                || commandText == KILL
                || commandText == UPDATE_KILL_LIST;
        }

        private static bool IsControl(string commandText)
        {
            return ADMIN_PANEL_LABELS.Contains(commandText);
        }

        private static bool IsPower(string commandText)
        {
            return POWER_LABELS.Contains(commandText);
        }

        private static bool IsVolume(string commandText)
        {
            return VOLUME_LABELS.Contains(commandText);
        }

        private static bool IsScreen(string commandText)
        {
            return SCREEN_LABELS.Contains(commandText);
        }

        private static bool IsProcess(string commandText)
        {
            return PROCESS_LABELS.Contains(commandText);
        }

        private static CommandInfo DetermineCommandInfo(CommandType commandType, string commandText)
        {
            return commandType switch
            {
                CommandType.Undefined => CommandInfo.Null,
                CommandType.Transfer => DetermineTranserCommandInfo(commandText),
                CommandType.AdminPanel => DetermineControlCommandInfo(commandText),
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
                BACK_LABEL => CommandInfo.ToMainMenu,
                ADMIN_PANEL_LABEL => CommandInfo.ToControl,
                POWER_LABEL => CommandInfo.ToPower,
                VOLUME_LABEL => CommandInfo.ToVolume,
                SCREEN_LABEL => CommandInfo.ToScreen,
                PROCESS_LABEL => CommandInfo.ToProcess,
                KILL => CommandInfo.ToKillList,
                UPDATE_KILL_LIST => CommandInfo.ToKillList,
                _ => Throw.CommandNotImplemented<CommandInfo>(commandText)
            };
        }

        private static CommandInfo DetermineControlCommandInfo(string commandText)
        {
            return commandText switch
            {
                BOT_TURN_OFF => CommandInfo.BotTurnOff,
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
