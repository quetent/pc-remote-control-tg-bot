using static RemoteControlBot.BotFunctions;
using static RemoteControlBot.Keyboard;

namespace RemoteControlBot
{
    public readonly struct Command
    {
        public readonly CommandType Type;
        public readonly CommandInfo Info;
        public readonly long SenderId;
        public readonly string RawText;

        public Command(string commandText, long senderId)
        {
            Type = DefineCommandType(commandText);
            Info = DefineCommandInfo(Type, commandText);
            SenderId = senderId;
            RawText = commandText;
        }

        public Command(CommandType builderType, CommandInfo builderInfo, string commandText, long senderId)
        {
            Type = builderType;
            Info = builderInfo;
            SenderId = senderId;
            RawText = commandText;
        }

        public override string ToString()
        {
            return $"{Type} -> {Info}";
        }

        public static bool IsNumberForProccesManager(Command previousCommand, string messageText)
        {
            return previousCommand.Type is CommandType.Transfer
                && previousCommand.Info is CommandInfo.ToKillList
                && messageText.IsNumber();
        }

        public static bool IsNumberForScreenshotManager(Command previousCommand, string messageText)
        {
            return previousCommand.Type is CommandType.Transfer
                && previousCommand.Info is CommandInfo.ToScreensList
                && messageText.IsNumber();
        }

        private static CommandType DefineCommandType(string commandText)
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
                || commandText == UPDATE_KILL_LIST
                || commandText == SCREENSHOT;
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

        private static CommandInfo DefineCommandInfo(CommandType commandType, string commandText)
        {
            return commandType switch
            {
                CommandType.Undefined => CommandInfo.Null,
                CommandType.Transfer => DefineTranserCommandInfo(commandText),
                CommandType.AdminPanel => DefineControlCommandInfo(commandText),
                CommandType.Power => DefinePowerCommandInfo(commandText),
                CommandType.Volume => DefineVolumeCommandInfo(commandText),
                CommandType.Screen => DefineScreenCommandInfo(commandText),
                CommandType.Process => DefineProcessCommandInfo(commandText),
                _ => Throw.NotImplemented<CommandInfo>(commandText)
            };
        }

        private static CommandInfo DefineTranserCommandInfo(string commandText)
        {
            return commandText switch
            {
                BACK_LABEL => CommandInfo.ToMainMenu,
                ADMIN_PANEL_LABEL => CommandInfo.ToAdminPanel,
                POWER_LABEL => CommandInfo.ToPower,
                VOLUME_LABEL => CommandInfo.ToVolume,
                SCREEN_LABEL => CommandInfo.ToScreen,
                PROCESS_LABEL => CommandInfo.ToProcess,
                SCREENSHOT => CommandInfo.ToScreensList,
                KILL => CommandInfo.ToKillList,
                UPDATE_KILL_LIST => CommandInfo.ToKillList,
                _ => Throw.NotImplemented<CommandInfo>(commandText)
            };
        }

        private static CommandInfo DefineControlCommandInfo(string commandText)
        {
            return commandText switch
            {
                BOT_TURN_OFF => CommandInfo.BotTurnOff,
                BOT_RESTART => CommandInfo.BotRestart,
                _ => Throw.NotImplemented<CommandInfo>(commandText)
            };
        }

        private static CommandInfo DefinePowerCommandInfo(string commandText)
        {
            return commandText switch
            {
                SHUTDOWN => CommandInfo.Shutdown,
                HIBERNATE => CommandInfo.Hibernate,
                LOCK => CommandInfo.Lock,
                RESTART => CommandInfo.Restart,
                _ => Throw.NotImplemented<CommandInfo>(commandText)
            };
        }

        private static CommandInfo DefineVolumeCommandInfo(string commandText)
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
                _ => Throw.NotImplemented<CommandInfo>(commandText)
            };
        }

        private static CommandInfo DefineScreenCommandInfo(string commandText)
        {
            return commandText switch
            {
                SCREENSHOT => CommandInfo.Screenshot,
                _ => Throw.NotImplemented<CommandInfo>(commandText)
            };
        }

        private static CommandInfo DefineProcessCommandInfo(string commandText)
        {
            return commandText switch
            {
                KILL => CommandInfo.Kill,
                _ => Throw.NotImplemented<CommandInfo>(commandText)
            };
        }
    }
}
