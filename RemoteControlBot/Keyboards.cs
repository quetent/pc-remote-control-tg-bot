using System.Linq;
using Telegram.Bot.Types.ReplyMarkups;

namespace RemoteControlBot
{
    internal static class Keyboards
    {
        private const string BACK_LABEL = "< Back >";

        internal static ReplyKeyboardMarkup MainMenu = new(
            new[]
            {
                new[]
                {
                    new KeyboardButton(BotFunctions.POWER), new KeyboardButton(BotFunctions.VOLUME),
                },
                new[]
                {
                    new KeyboardButton(BotFunctions.SCREEN)
                }
            });

        internal static ReplyKeyboardMarkup Power = new(
            new[]
            {
                new[]
                {
                    new KeyboardButton(BotFunctions.SHUTDOWN), new KeyboardButton(BotFunctions.SLEEP), new KeyboardButton(BotFunctions.RESTART)
                },
                new[]
                {
                    new KeyboardButton(BACK_LABEL)
                }
            });

        internal static ReplyKeyboardMarkup Volume = new(
            new[]
            {
                new[]
                {
                    new KeyboardButton(BotFunctions.LOUDER_5), new KeyboardButton(BotFunctions.QUIETER_5)
                },
                new[]
                {
                    new KeyboardButton(BotFunctions.LOUDER_10), new KeyboardButton(BotFunctions.QUIETER_10)
                },
                new[]
                {
                    new KeyboardButton(BotFunctions.MAX), new KeyboardButton(BotFunctions.MIN)
                },
                new[]
                {
                    new KeyboardButton(BotFunctions.MUTE)
                },
                new[]
                {
                    new KeyboardButton(BACK_LABEL)
                }
            });

        internal static ReplyKeyboardMarkup Screen = new(
            new[]
            {
                new[]
                {
                    new KeyboardButton(BotFunctions.SCREENSHOT)
                },
                new[]
                {
                    new KeyboardButton(BACK_LABEL)
                }
            });
    }
}
