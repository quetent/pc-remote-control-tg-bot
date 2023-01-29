using System.Runtime.Intrinsics.Arm;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace RemoteControlBot
{
    internal class Bot
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

            var chatId = message.Chat.Id;

            Logger.LogMessageRecieved(messageText, update.Message.From);

            await _botClient.SendTextMessageAsync(
                            message.Chat.Id,
                            text: "sss",
                            replyMarkup: Keyboards.MainMenu);
        }

        private async Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            if (_enableLogging)
                Logger.LogUnhandledException(exception);
        }
    }
}
