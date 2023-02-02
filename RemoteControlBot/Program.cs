using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

namespace RemoteControlBot
{
    internal class Program
    {
        static void Main()
        {
            var cts = new CancellationTokenSource();
            var receiverOptions = new ReceiverOptions() { AllowedUpdates = new[] { UpdateType.Message } };

            var bot = new Bot(OWNER_ID, TOKEN, receiverOptions, cts.Token);
            bot.StartAsync();

            Wait();
        }

        private static void Wait()
        {
            Console.ReadLine();
        }
    }
}