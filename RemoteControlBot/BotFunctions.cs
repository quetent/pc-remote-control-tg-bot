using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteControlBot
{
    internal static class BotFunctions
    {
        internal static readonly string[] MAIN_MENU_LABELS = { POWER, VOLUME, SCREEN };
        internal const string POWER = "Power";
        internal const string VOLUME = "Volume";
        internal const string SCREEN = "Screen";

        internal static readonly string[] POWER_LABELS = { SHUTDOWN, SLEEP, RESTART };
        internal const string SHUTDOWN = "Shutdown";
        internal const string SLEEP = "Sleep";
        internal const string RESTART = "Restart";

        internal static readonly string[] VOLUME_LABELS = { LOUDER_5, QUIETER_5, LOUDER_10, QUIETER_10, MAX, MIN, MUTE };
        internal const string LOUDER_5 = "Up (+5)";
        internal const string QUIETER_5 = "Down (-5)"; 
        internal const string LOUDER_10 = "Up (+10)";
        internal const string QUIETER_10 = "Down (-10)";
        internal const string MAX = "Max";
        internal const string MIN = "Min";
        internal const string MUTE = "Mute";

        internal static readonly string[] SCREEN_LABELS = { SCREENSHOT };
        internal const string SCREENSHOT = "Screenshot";
    }
}
