namespace RemoteControlBot
{
    public enum CommandInfo
    {
        Null,

        BotTurnOff,
        BotRestart,

        ToMainMenu,
        ToAdminPanel,
        ToPower,
        ToVolume,
        ToScreen,
        ToProcess,

        ToKillList,

        Shutdown,
        Hibernate,
        Lock,
        Restart,

        Louder5,
        Quieter5,
        Louder10,
        Quieter10,
        Max,
        Min,
        Mute,
        Unmute,

        Screenshot,

        Kill
    }
}
