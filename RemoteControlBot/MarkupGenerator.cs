using Telegram.Bot.Types.ReplyMarkups;

namespace RemoteControlBot
{
    internal class MarkupGenerator
    {
        internal static IReplyMarkup GetKeyboard(Command command)
        {
            return command.Type switch
            {
                CommandType.Transfer => GetTransferKeyboard(command),
                CommandType.Power => GetPowerKeyboard(),
                CommandType.Volume => GetVolumeKeyboard(),
                CommandType.Screen => GetScreenKeyboard(),
                CommandType.Process => GetProcessKeyboard(),
                _ => GetMainMenuKeyboard()
            };
        }

        private static IReplyMarkup GetTransferKeyboard(Command command)
        {
            Throw.IfIncorrectCommandType(command, CommandType.Transfer);

            return command.Info switch
            {
                CommandInfo.ToMainMenu => GetMainMenuKeyboard(),
                CommandInfo.ToControl => GetControlKeyboard(),
                CommandInfo.ToPower => GetPowerKeyboard(),
                CommandInfo.ToVolume => GetVolumeKeyboard(),
                CommandInfo.ToScreen => GetScreenKeyboard(),
                CommandInfo.ToProcess => GetProcessKeyboard(),
                CommandInfo.ToKillList => GetKillListKeyboard(),
                _ => Throw.NotImplemented<IReplyMarkup>(command.ToString())
            };
        }

        private static IReplyMarkup GetMainMenuKeyboard()
        {
            return Keyboard.MainMenu;
        }

        private static IReplyMarkup GetControlKeyboard()
        {
            return Keyboard.AdminPanel;
        }

        private static IReplyMarkup GetPowerKeyboard()
        {
            return Keyboard.Power;
        }

        private static IReplyMarkup GetVolumeKeyboard()
        {
            return Keyboard.Volume;
        }

        private static IReplyMarkup GetScreenKeyboard()
        {
            return Keyboard.Screen;
        }

        private static IReplyMarkup GetProcessKeyboard()
        {
            return Keyboard.Process;
        }

        private static IReplyMarkup GetKillListKeyboard()
        {
            return Keyboard.GenerateIndexedKeyboard(ProcessManager.VisibleProcesses.Count);
        }
    }
}