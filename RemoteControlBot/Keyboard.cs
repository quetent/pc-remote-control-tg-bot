using Telegram.Bot.Types.ReplyMarkups;
using static RemoteControlBot.BotFunctions;

namespace RemoteControlBot
{
    public static class Keyboard
    {
        public const int ROW_LENGTH = 5;

        public const string POWER_LABEL = "Power";
        public const string VOLUME_LABEL = "Volume";
        public const string SCREEN_LABEL = "Screen";
        public const string PROCESS_LABEL = "Process";
        public const string ADMIN_PANEL_LABEL = "Admin panel";

        public const string UPDATE_KILL_LIST = "Update";

        public const string BACK_LABEL = "< Back >";

        public static readonly string[] MAIN_MENU_LABELS = { POWER_LABEL, ADMIN_PANEL_LABEL, VOLUME_LABEL, SCREEN_LABEL, PROCESS_LABEL };
        public static readonly string[] ADMIN_PANEL_LABELS = { BOT_TURN_OFF, BOT_RESTART };
        public static readonly string[] POWER_LABELS = { SHUTDOWN, HIBERNATE, LOCK, RESTART };
        public static readonly string[] VOLUME_LABELS = { LOUDER_5, QUIETER_5, LOUDER_10, QUIETER_10, MAX, MIN, MUTE, UNMUTE };
        public static readonly string[] SCREEN_LABELS = { SCREENSHOT };
        public static readonly string[] PROCESS_LABELS = { KILL };

        public static readonly ReplyKeyboardMarkup MainMenu = new(
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

        public static readonly ReplyKeyboardMarkup AdminPanel = new(
            new[]
            {
                new[]
                {
                    new KeyboardButton(BACK_LABEL)
                },
                new[]
                {
                    new KeyboardButton(BOT_RESTART)
                },
                new[]
                {
                    new KeyboardButton(BOT_TURN_OFF)
                }
            });

        public static readonly ReplyKeyboardMarkup Power = new(
            new[]
            {
                new[]
                {
                    new KeyboardButton(BACK_LABEL)
                },
                new[]
                {
                    new KeyboardButton(SHUTDOWN), new KeyboardButton(HIBERNATE),
                },
                new[]
                {
                    new KeyboardButton(RESTART), new KeyboardButton(LOCK)
                }
            });

        public static readonly ReplyKeyboardMarkup Volume = new(
            new[]
            {
                new[]
                {
                    new KeyboardButton(BACK_LABEL)
                },
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
                }
            });

        public static readonly ReplyKeyboardMarkup Screen = new(
            new[]
            {
                new[]
                {
                    new KeyboardButton(BACK_LABEL)
                },
                new[]
                {
                    new KeyboardButton(SCREENSHOT)
                }
            });

        public static readonly ReplyKeyboardMarkup Process = new(
            new[]
            {
                new[]
                {
                    new KeyboardButton(BACK_LABEL)
                },
                new[]
                {
                    new KeyboardButton(KILL)
                }
            });

        public static ReplyKeyboardMarkup GenerateIndexedKeyboard(int index)
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

            return index >= minSeparateCount ? minLengthIfMore : minLengthIfLess;
        }
    }
}
