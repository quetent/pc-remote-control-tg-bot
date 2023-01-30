using Telegram.Bot.Types.ReplyMarkups;
using static RemoteControlBot.BotFunctions;

namespace RemoteControlBot
{
    internal static class Keyboard 
    { 
        internal const string POWER_LABEL = "Power";
        internal const string VOLUME_LABEL = "Volume";
        internal const string SCREEN_LABEL = "Screen";

        internal const string BACK_LABEL = "< Back >";

        internal static readonly string[] MAIN_MENU_LABELS = { POWER_LABEL, VOLUME_LABEL, SCREEN_LABEL };
        internal static readonly string[] POWER_LABELS = { SHUTDOWN, SLEEP, RESTART };
        internal static readonly string[] VOLUME_LABELS = { LOUDER_5, QUIETER_5, LOUDER_10, QUIETER_10, MAX, MIN, MUTE, UNMUTE };
        internal static readonly string[] SCREEN_LABELS = { SCREENSHOT };

        internal static ReplyKeyboardMarkup MainMenu = new(
            new[]
            {
                new[]
                {
                    new KeyboardButton(POWER_LABEL), new KeyboardButton(VOLUME_LABEL),
                },
                new[]
                {
                    new KeyboardButton(SCREEN_LABEL)
                }
            });

        internal static ReplyKeyboardMarkup Power = new(
            new[]
            {
                new[]
                {
                    new KeyboardButton(SHUTDOWN), new KeyboardButton(SLEEP), new KeyboardButton(RESTART)
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
                    new KeyboardButton(LOUDER_5), new KeyboardButton(QUIETER_5)
                },
                new[]
                {
                    new KeyboardButton(LOUDER_10), new KeyboardButton(QUIETER_10)
                },
                new[]
                {
                    new KeyboardButton(MAX), new KeyboardButton(MIN)
                },
                new[]
                {
                    new KeyboardButton(MUTE), new KeyboardButton(UNMUTE)
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
                    new KeyboardButton(SCREENSHOT)
                },
                new[]
                {
                    new KeyboardButton(BACK_LABEL)
                }
            });
    }
}
