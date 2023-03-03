using Telegram.Bot.Types.ReplyMarkups;

namespace RemoteControlBot
{
    public class MarkupGenerator : IMarkupGetable
    {
        private readonly Command _command;

        public MarkupGenerator(Command command)
        {
            _command = command;
        }

        public IReplyMarkup GetMarkup()
        {
            return _command.Type switch
            {
                CommandType.Transfer => new TransferMarkup(_command).GetMarkup(),
                CommandType.Power => new PowerMarkup().GetMarkup(),
                CommandType.Volume => new VolumeMarkup().GetMarkup(),
                CommandType.Screen => new ScreenMarkup().GetMarkup(),
                CommandType.Process => new ProcessMarkup().GetMarkup(),
                _ => new MainMenuMarkup().GetMarkup()
            };
        }
    }

    public class MainMenuMarkup : IMarkupGetable
    {
        public IReplyMarkup GetMarkup()
        {
            return GetMainMenuKeyboard();
        }

        private static IReplyMarkup GetMainMenuKeyboard()
        {
            return Keyboard.MainMenu;
        }
    }

    public class TransferMarkup : IMarkupGetable
    {
        private readonly Command _command;

        public TransferMarkup(Command command)
        {
            Throw.IfIncorrectCommandType(command, CommandType.Transfer);

            _command = command;
        }

        public IReplyMarkup GetMarkup()
        {
            return _command.Info switch
            {
                CommandInfo.ToMainMenu => new MainMenuMarkup().GetMarkup(),
                CommandInfo.ToAdminPanel => new AdminPanelMarkup().GetMarkup(),
                CommandInfo.ToPower => new PowerMarkup().GetMarkup(),
                CommandInfo.ToVolume => new VolumeMarkup().GetMarkup(),
                CommandInfo.ToScreen => new ScreenMarkup().GetMarkup(),
                CommandInfo.ToProcess => new ProcessMarkup().GetMarkup(),
                CommandInfo.ToScreensList => GetScreensListKeyboard(),
                CommandInfo.ToKillList => GetKillListKeyboard(),
                _ => Throw.NotImplemented<IReplyMarkup>($"{nameof(TransferMarkup)} -> {_command}")
            };
        }

        private static IReplyMarkup GetScreensListKeyboard()
        {
            return Keyboard.GenerateIndexedKeyboard(ScreenManager.VisibleScreensCount, Keyboard.UPDATE_SCREENS_LIST);
        }

        private static IReplyMarkup GetKillListKeyboard()
        {
            return Keyboard.GenerateIndexedKeyboard(ProcessManager.VisibleProcessesCount, Keyboard.UPDATE_KILL_LIST);
        }
    }

    public class AdminPanelMarkup : IMarkupGetable
    {
        public IReplyMarkup GetMarkup()
        {
            return GetKeyboard();
        }

        private static IReplyMarkup GetKeyboard()
        {
            return Keyboard.AdminPanel;
        }
    }

    public class PowerMarkup : IMarkupGetable
    {
        public IReplyMarkup GetMarkup()
        {
            return GetPowerKeyboard();
        }

        private static IReplyMarkup GetPowerKeyboard()
        {
            return Keyboard.Power;
        }
    }

    public class VolumeMarkup : IMarkupGetable
    {
        public IReplyMarkup GetMarkup()
        {
            return GetVolumeKeyboard();
        }

        private static IReplyMarkup GetVolumeKeyboard()
        {
            return Keyboard.Volume;
        }
    }

    public class ScreenMarkup : IMarkupGetable
    {
        public IReplyMarkup GetMarkup()
        {
            return GetScreenKeyboard();
        }

        private static IReplyMarkup GetScreenKeyboard()
        {
            return Keyboard.Screen;
        }
    }

    public class ProcessMarkup : IMarkupGetable
    {
        public IReplyMarkup GetMarkup()
        {
            return GetProcessKeyboard();
        }

        private static IReplyMarkup GetProcessKeyboard()
        {
            return Keyboard.Process;
        }
    }
}