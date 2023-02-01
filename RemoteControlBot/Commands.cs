namespace RemoteControlBot
{
    internal enum CommandType
    {
        Power = 0,
        Volume = 2,
        Screen = 4,
        Transfer = 8,
        Undefined = 16
    }

    internal enum PowerCommand
    {
        Shutdown = 0,
        Hibernate = 2,
        Lock = 4,
        Restart = 8
    }

    internal enum VolumeCommand
    {
        Louder5 = 0,
        Quieter5 = 2,
        Louder10 = 4,
        Quiter10 = 8,
        Max = 16,
        Min = 32,
        Mute = 64,
        Unmute = 128
    }

    internal enum ScreenCommand
    {
        Screenshot = 0
    }
}
