namespace RemoteControlBot
{
    internal enum CommandInfo
    {
        Null,

        ToMainMenu,
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
