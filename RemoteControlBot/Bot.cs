using System.Runtime.CompilerServices;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using static RemoteControlBot.Keyboard;
using static RemoteControlBot.BotFunctions;
using System.Diagnostics.SymbolStore;

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
                Log.BotStartup();
        }

        private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var isValid = IsUpdateValid(update);
            var (message, messageText) = GetMessageAndMessageText(update);
            var chatId = message.Chat.Id;

            if (_enableLogging)
                Log.MessageRecieved(messageText, message.From);

            if (!isValid && !IsAccessAllowed(chatId))
                return;

            var markup = GetMarkup(messageText);

            await _botClient.SendTextMessageAsync(
                            chatId,
                            text: "temp answer",
                            replyMarkup: markup,
                            cancellationToken: cancellationToken);
        }

        private async Task<Task> HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            if (_enableLogging)
                Log.UnhandledException(exception);

            return Task.CompletedTask;
        }


        private static bool IsUpdateValid(Update update)
        {
            return update.Message?.Text is not null;
        }

        private static (Message message, string messageText) GetMessageAndMessageText(Update update)
        {
            var message = update.Message;
            var messageText = message!.Text!;

            return (message, messageText);
        }

        private static IReplyMarkup GetMarkup(string messageText)
        {
            IReplyMarkup markup;

            if (MAIN_MENU_LABELS.Contains(messageText))
                if (messageText == POWER)
                    markup = Power;
                else if (messageText == VOLUME)
                    markup = Volume;
                else if (messageText == SCREEN)
                    markup = Screen;
                else
                    throw new NotImplementedException();
            else if (POWER_LABELS.Contains(messageText))
                markup = Power;
            else if (VOLUME_LABELS.Contains(messageText))
                markup = Volume;
            else if (SCREEN_LABELS.Contains(messageText))
                markup = Screen;
            else
                markup = MainMenu;

            return markup;
        }

        private bool IsAccessAllowed(long chatId)
        {
            return chatId == _ownerId;
        }
    }
}
