using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace RemoteControlBot
{
    internal class Program
    {
        static void Main()
        {
            var cts = new CancellationTokenSource();
            var receiverOptions = new ReceiverOptions() { AllowedUpdates = new[] { UpdateType.Message } };

            var bot = new Bot(Config.OWNER_ID, Config.ENABLE_LOGGING, Config.TOKEN, receiverOptions, cts.Token);
            bot.Start();

            Wait();
        }

        private static void Wait()
        {
            Console.ReadLine();
        }
    }
}