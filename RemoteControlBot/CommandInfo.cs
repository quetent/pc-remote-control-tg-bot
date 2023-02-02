﻿namespace RemoteControlBot
{
    internal enum CommandInfo
    {
        Null = 0,

        Shutdown = 2,
        Hibernate = 4,
        Lock = 8,
        Restart = 16,

        Louder5 = 32,
        Quieter5 = 64,
        Louder10 = 128,
        Quieter10 = 256,
        Max = 512,
        Min = 1024,
        Mute = 2048,
        Unmute = 4096,

        Screenshot = 8192
    }
}