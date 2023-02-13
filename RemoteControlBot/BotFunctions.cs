namespace RemoteControlBot
{
    public static class BotFunctions
    {
        #region AdminPanel
        public const string BOT_TURN_OFF = "Bot turn off";
        public const string BOT_RESTART = "Bot restart";
        #endregion

        #region Power
        public const string SHUTDOWN = "Shutdown";
        public const string HIBERNATE = "Hibernate";
        public const string LOCK = "Lock";
        public const string RESTART = "Restart";
        #endregion

        #region Volume
        public const string LOUDER_5 = "Up (+5)";
        public const string QUIETER_5 = "Down (-5)";
        public const string LOUDER_10 = "Up (+10)";
        public const string QUIETER_10 = "Down (-10)";
        public const string MAX = "Max";
        public const string MIN = "Min";
        public const string MUTE = "Mute";
        public const string UNMUTE = "Unmute";
        #endregion

        #region Screen
        public const string SCREENSHOT = "Screenshot primary screen";
        #endregion

        #region Process
        public const string KILL = "Kill";
        #endregion
    }
}
