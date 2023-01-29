﻿using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace RemoteControlBot
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var cts = new CancellationTokenSource();
            var receiverOptions = new ReceiverOptions() { AllowedUpdates = Array.Empty<UpdateType>() };

            var bot = new Bot(Config.OWNER_ID, Config.ENABLE_LOGGING, Config.TOKEN, receiverOptions, cts.Token);
            bot.Start();

            Console.ReadKey();
        }
    }
}