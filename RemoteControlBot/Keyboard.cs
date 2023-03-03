using System.Collections.ObjectModel;
using Telegram.Bot.Types.ReplyMarkups;
using static RemoteControlBot.BotFunctions;

namespace RemoteControlBot
{
    public static class Keyboard
    {
        public const int MAX_ROW_LENGTH = 5;

        public const string POWER_LABEL = "Power";
        public const string VOLUME_LABEL = "Volume";
        public const string SCREEN_LABEL = "Screen";
        public const string PROCESS_LABEL = "Process";
        public const string ADMIN_PANEL_LABEL = "Admin panel";

        public const string UPDATE_SCREENS_LIST = "Update screens list";
        public const string UPDATE_KILL_LIST = "Update kill list";

        public const string BACK_LABEL = "< Back >";

        public static readonly ReadOnlyCollection<string> MAIN_MENU_LABELS
            = new List<string>() { POWER_LABEL, ADMIN_PANEL_LABEL, VOLUME_LABEL, SCREEN_LABEL, PROCESS_LABEL }.AsReadOnly();
        public static readonly ReadOnlyCollection<string> ADMIN_PANEL_LABELS
            = new List<string>() { BOT_TURN_OFF, BOT_RESTART }.AsReadOnly();
        public static readonly ReadOnlyCollection<string> POWER_LABELS
            = new List<string>() { SHUTDOWN, HIBERNATE, LOCK, RESTART }.AsReadOnly();
        public static readonly ReadOnlyCollection<string> VOLUME_LABELS
            = new List<string>() { LOUDER_5, QUIETER_5, LOUDER_10, QUIETER_10, MAX, MIN, MUTE, UNMUTE }.AsReadOnly();
        public static readonly ReadOnlyCollection<string> SCREEN_LABELS
            = new List<string>() { SCREENSHOT }.AsReadOnly();
        public static readonly ReadOnlyCollection<string> PROCESS_LABELS
            = new List<string>() { KILL }.AsReadOnly();

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

        public static ReplyKeyboardMarkup GenerateIndexedKeyboard(int index, string updateButtonLabel)
        {
            var buttonValue = 1;
            var buttons = new List<List<KeyboardButton>>();
            var (height, width) = GetKeyboardSize(index);

            buttons.Add(new List<KeyboardButton> { new KeyboardButton(BACK_LABEL) });
            buttons.Add(new List<KeyboardButton> { new KeyboardButton(updateButtonLabel) });

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

            for (int i = minRowLength; i <= MAX_ROW_LENGTH; i++)
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
