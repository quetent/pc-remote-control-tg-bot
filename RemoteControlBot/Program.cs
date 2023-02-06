using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

namespace RemoteControlBot
{
    internal class Program
    {
        private const int FINALIZE_WAITING_TIME_MS = 1000;

        static async Task Main(string[] args)
        {
            SetConsoleTitle();

            var startUpCode = StartUpCodeUtilities.ParseStartUpCode(args);

            WaitForPreviousAppFinalize(startUpCode, FINALIZE_WAITING_TIME_MS);

            var cts = new CancellationTokenSource();
            var receiverOptions = new ReceiverOptions() { AllowedUpdates = new[] { UpdateType.Message } };

            var bot = new Bot(OWNER_ID, TOKEN, receiverOptions, cts.Token);
            await bot.StartAsync(startUpCode);

            WaitKeyboard();
        }

        private static void WaitForPreviousAppFinalize(StartUpCode startUpCode, int milliseconds)
        {
            if (startUpCode is not StartUpCode.Null)
                Thread.Sleep(milliseconds);
        }

        private static void WaitKeyboard()
        {
            Console.ReadLine();
        }

        private static void SetConsoleTitle()
        {
            Console.Title = BOT_NAME;
        }
    }
}