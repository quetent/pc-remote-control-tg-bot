using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using static RemoteControlBot.BotFunctions;
using static RemoteControlBot.Keyboard;

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

            var message = GetMessage(update);
            var messageText = GetMessageText(message);
            var user = GetMessageSender(message);

            if (_enableLogging)
                Log.MessageRecieved(messageText, user);

            if (!isValid && !IsAccessAllowed(user))
                return;

            var chatId = GetChatId(message);
            var text = GetTextAnswer(messageText);
            var markup = GetMarkup(messageText);

            await _botClient.SendTextMessageAsync(
                            chatId: chatId,
                            text: text,
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

        private static string GetMessageText(Message message)
        {
            return message.Text!.Trim();
        }

        private static Message GetMessage(Update update)
        {
            return update.Message!;
        }

        private static long GetChatId(Message message)
        {
            return message.Chat.Id;
        }

        private static string GetTextAnswer(string messageText)
        {
            return "temp";
        }

        private static User? GetMessageSender(Message message)
        {
            return message.From;
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

        private bool IsAccessAllowed(User? user)
        {
            return user?.Id == _ownerId;
        }
    }
}
