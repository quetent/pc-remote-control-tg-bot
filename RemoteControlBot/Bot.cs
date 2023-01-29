using System.Runtime.CompilerServices;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using static RemoteControlBot.Keyboards;
using static RemoteControlBot.BotFunctions;

namespace RemoteControlBot
{
    public class Bot
    {
        private readonly long _ownerId;
        private readonly bool _enableLogging;

        private readonly TelegramBotClient _botClient;
        private readonly ReceiverOptions _receiverOptions;

        private readonly CancellationToken _cancellationToken;

        public Bot(long ownerId,
                   bool enableLogging,
                   string token,
                   ReceiverOptions recieverOptions,
                   CancellationToken cancellationToken)
        {
            _ownerId = ownerId;
            _enableLogging = enableLogging;
            _botClient = new TelegramBotClient(token);
            _receiverOptions = recieverOptions;
            _cancellationToken = cancellationToken;
        }

        public void Start()
        {
            _botClient.StartReceiving(
                updateHandler: HandleUpdateAsync,
                pollingErrorHandler: HandlePollingErrorAsync,
                receiverOptions: _receiverOptions,
                cancellationToken: _cancellationToken);

            if (_enableLogging)
                Logger.LogBotStartup();
        }

        private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Message is not { } message)
                return;
            if (message.Text is not { } messageText)
                return;

            Logger.LogMessageRecieved(messageText, update.Message.From);

            var chatId = message.Chat.Id;

            if (chatId != _ownerId)
                return;

            IReplyMarkup markup;
            
            if (MAIN_MENU_LABELS.Contains(messageText))
            {
                if (messageText == POWER)
                {
                    markup = Keyboards.Power;
                }
                else if (messageText == VOLUME)
                {
                    markup = Keyboards.Volume;
                }
                else if (messageText == SCREEN)
                {
                    markup = Keyboards.Screen;
                }
                else
                {
                    throw new ArgumentOutOfRangeException();
                }
            }
            else if (POWER_LABELS.Contains(messageText))
            {
                markup = Keyboards.Power;
            }
            else if (VOLUME_LABELS.Contains(messageText))
            {
                markup = Keyboards.Volume;
            }
            else if (SCREEN_LABELS.Contains(messageText))
            {
                markup = Keyboards.Screen;
            }
            else
            {
                markup = Keyboards.MainMenu;
            }

            await _botClient.SendTextMessageAsync(
                            message.Chat.Id,
                            text: "sss",
                            replyMarkup: markup);
        }

        private async Task<Task> HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            if (_enableLogging)
                Logger.LogUnhandledException(exception);

            return Task.CompletedTask;
        }
    }
}
