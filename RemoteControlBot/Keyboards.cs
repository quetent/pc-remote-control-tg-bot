using Telegram.Bot.Types.ReplyMarkups;

namespace RemoteControlBot
{
    internal static class Keyboards
    {
        internal static ReplyKeyboardMarkup MainMenu = new(
            new[]
            {
                new[]
                {
                    new KeyboardButton("Button1"), new KeyboardButton("Button2")
                }
            });
    }
}
