using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

namespace RemoteControlBot
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            if (!COMPILE_BACKGROUND)
                SetConsoleTitle();

            var startUpCode = StartUpCodeUtilities.ParseStartUpCode(args);

            WaitForPreviousAppFinalize(startUpCode, FINALIZE_WAITING_TIME_MS);

            var cts = new CancellationTokenSource();
            var receiverOptions = new ReceiverOptions() { AllowedUpdates = new[] { UpdateType.Message } };

            var bot = new Bot(OWNER_ID, TOKEN, receiverOptions, cts.Token);

            while (true)
            {
                try
                {
                    await bot.StartAsync(startUpCode);
                    break;
                }
                catch (RequestException)
                {
                    if (!WAIT_INTERNET_TO_START)
                        throw;

                    if (ENABLE_LOGGING)
                        Log.NoConnection();

                    await HttpClientUtilities.WaitForInternetConnectionAsync(
                            INTERNET_CHECKING_URL,
                            5000, 3000);

                    if (ENABLE_LOGGING)
                        Log.ConnectionRestored();
                }
            }

            Wait();
        }

        private static void WaitForPreviousAppFinalize(StartUpCode startUpCode, int milliseconds)
        {
            if (startUpCode is not StartUpCode.Null)
                Thread.Sleep(milliseconds);
        }

        private static void Wait()
        {
            if (COMPILE_BACKGROUND)
                while (true)
                    Thread.Sleep((int)10e6);
            else
                Console.ReadLine();
        }

        private static void SetConsoleTitle()
        {
            Console.Title = BOT_NAME;
        }
    }
}