using Telegram.Bot.Types.ReplyMarkups;
using static RemoteControlBot.Keyboard;

namespace RemoteControlBot
{
    internal class MarkupGenerator
    {
        internal static IReplyMarkup GetKeyboard(string messageText)
        {
            IReplyMarkup markup;

            if (MAIN_MENU_LABELS.Contains(messageText))
                if (messageText == POWER_LABEL)
                    markup = Power;
                else if (messageText == VOLUME_LABEL)
                    markup = Volume;
                else if (messageText == SCREEN_LABEL)
                    markup = Screen;
                else
                    throw new NotImplementedException();
            else if (POWER_LABELS.Contains(messageText))
                markup = Power;
            else if (VOLUME_LABELS.Contains(messageText))
                markup = Volume;
            else if (SCREEN_LABELS.Contains(messageText))
                markup = Screen;
            else
                markup = MainMenu;

            return markup;
        }
    }
}
