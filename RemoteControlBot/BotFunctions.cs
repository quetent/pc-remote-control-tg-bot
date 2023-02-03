namespace RemoteControlBot
{
    internal static class BotFunctions
    {
        // CONTROL
        internal const string TURN_OFF = "Turn off";

        // POWER
        internal const string SHUTDOWN = "Shutdown";
        internal const string HIBERNATE = "Hibernate";
        internal const string LOCK = "Lock";
        internal const string RESTART = "Restart";

        // VOLUME
        internal const string LOUDER_5 = "Up (+5)";
        internal const string QUIETER_5 = "Down (-5)";
        internal const string LOUDER_10 = "Up (+10)";
        internal const string QUIETER_10 = "Down (-10)";
        internal const string MAX = "Max";
        internal const string MIN = "Min";
        internal const string MUTE = "Mute";
        internal const string UNMUTE = "Unmute";

        // SCREEN
        internal const string SCREENSHOT = "Screenshot";

        // PROCESS
        internal const string KILL = "Kill";
    }
}
