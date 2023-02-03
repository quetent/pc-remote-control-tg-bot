using Telegram.Bot.Types.ReplyMarkups;
using static RemoteControlBot.BotFunctions;

namespace RemoteControlBot
{
    internal static class Keyboard
    {
        internal const int ROW_LENGTH = 5;

        internal const string POWER_LABEL = "Power";
        internal const string VOLUME_LABEL = "Volume";
        internal const string SCREEN_LABEL = "Screen";
        internal const string PROCESS_LABEL = "Process";
        internal const string ADMIN_PANEL_LABEL = "Admin panel";

        internal const string UPDATE_KILL_LIST = "Update";

        internal const string BACK_LABEL = "< Back >";

        internal static readonly string[] MAIN_MENU_LABELS = { POWER_LABEL, ADMIN_PANEL_LABEL, VOLUME_LABEL, SCREEN_LABEL, PROCESS_LABEL };
        internal static readonly string[] ADMIN_PANEL_LABELS = { BOT_TURN_OFF };
        internal static readonly string[] POWER_LABELS = { SHUTDOWN, HIBERNATE, LOCK, RESTART };
        internal static readonly string[] VOLUME_LABELS = { LOUDER_5, QUIETER_5, LOUDER_10, QUIETER_10, MAX, MIN, MUTE, UNMUTE };
        internal static readonly string[] SCREEN_LABELS = { SCREENSHOT };
        internal static readonly string[] PROCESS_LABELS = { KILL };

        internal static ReplyKeyboardMarkup MainMenu = new(
            new[]
            {
                new[]
                {
                    new KeyboardButton(POWER_LABEL), new KeyboardButton(VOLUME_LABEL),
                },
                new[]
                {
                    new KeyboardButton(SCREEN_LABEL), new KeyboardButton(PROCESS_LABEL),
                },
                new[]
                {
                    new KeyboardButton(ADMIN_PANEL_LABEL)
                }
            });

        internal static ReplyKeyboardMarkup Control = new(
            new[]
            {
                new[]
                {
                    new KeyboardButton(BOT_TURN_OFF)
                },
                new[]
                {
                    new KeyboardButton(BACK_LABEL)
                }
            });

        internal static ReplyKeyboardMarkup Power = new(
            new[]
            {
                new[]
                {
                    new KeyboardButton(SHUTDOWN), new KeyboardButton(HIBERNATE),
                },
                new[]
                {
                    new KeyboardButton(RESTART), new KeyboardButton(LOCK)
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

        internal static ReplyKeyboardMarkup Process = new(
            new[]
            {
                new[]
                {
                    new KeyboardButton(KILL)
                },
                new[]
                {
                    new KeyboardButton(BACK_LABEL)
                }
            });

        internal static ReplyKeyboardMarkup GenerateIndexedKeyboard(int index)
        {
            var buttonValue = 1;
            var buttons = new List<List<KeyboardButton>>();
            var (height, width) = GetKeyboardSize(index);

            buttons.Add(new List<KeyboardButton> { new KeyboardButton(BACK_LABEL) });
            buttons.Add(new List<KeyboardButton> { new KeyboardButton(UPDATE_KILL_LIST) });

            for (int i = 0; i < height; i++)
            {
                var row = new List<KeyboardButton>();

                for (int j = 0; j < width; j++)
                {
                    if (buttonValue == index + 1)
                        break;

                    row.Add(new KeyboardButton(buttonValue.ToString()));
                    buttonValue++;
                }

                buttons.Add(row);
            }

            return new ReplyKeyboardMarkup(buttons);
        }

        private static (int height, int width) GetKeyboardSize(int index)
        {
            var minRowLength = GetMinRowLength(index);
            int height = 0, width = 0;

            for (int i = minRowLength; i <= ROW_LENGTH; i++)
            {
                width = i;
                var remains = index % width;

                if (remains == 0)
                {
                    height = index / width;
                    break;
                }

                height = index / width + remains;
            }

            return (height, width);
        }

        private static int GetMinRowLength(int index)
        {
            var minSeparateCount = 6;

            var minLengthIfMore = 3;
            var minLengthIfLess = 2;

            return index > minSeparateCount ? minLengthIfMore : minLengthIfLess;
        }
    }
}
