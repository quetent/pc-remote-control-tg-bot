using Telegram.Bot.Types.ReplyMarkups;

namespace RemoteControlBot
{
    public interface IMarkupGetable
    {
        IReplyMarkup GetMarkup();
    }
}
