using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;
using static RemoteControlBot.AnswerGenerator;
using static RemoteControlBot.CommandExecuter;
using static RemoteControlBot.Keyboard;
using static RemoteControlBot.MarkupGenerator;

namespace RemoteControlBot
{
    public class Bot
    {
        private readonly long _ownerId;
        public long OwnerId => _ownerId;

        private readonly TelegramBotClient _botClient;
        private readonly ReceiverOptions _receiverOptions;
        private DateTime _startupTime;

        private readonly CancellationToken _cancellationToken;

        public Bot(long ownerId,
                   string token,
                   ReceiverOptions recieverOptions,
                   CancellationToken cancellationToken)
        {
            _ownerId = ownerId;
            _botClient = new TelegramBotClient(token);
            _receiverOptions = recieverOptions;
            _cancellationToken = cancellationToken;
        }

        public void Start()
        {
            _startupTime = DateTimeManager.GetCurrentDateTime();

            _botClient.StartReceiving(
                updateHandler: HandleUpdateAsync,
                pollingErrorHandler: HandlePollingErrorAsync,
                receiverOptions: _receiverOptions,
                cancellationToken: _cancellationToken);

            if (ENABLE_LOGGING)
                Log.BotStartup();
        }

        private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var isUpdateValid = UpdateValidator.IsUpdateValid(update);
            var sendTime = update.Message!.Date.ToLocalTime();

            if (!isUpdateValid)
            {
                Log.MessageSkipped("< no message text >", null);
                return;
            }

            var message = GetMessage(update);
            var messageText = GetMessageText(message);
            var user = GetMessageSender(message);

            var sequence = new[]
            {
                isUpdateValid,
                UpdateValidator.IsAccessAllowed(this, user),
                UpdateValidator.IsMessageAfterStartup(_startupTime, sendTime)
            };

            if (!UpdateValidator.IsSequenceValid(sequence))
            {
                Log.MessageSkipped(messageText, user);
                return;
            }

            if (ENABLE_LOGGING)
                Log.MessageRecieved(messageText, user);

            var commandType = DetermineCommandType(messageText);

            ExecuteCommand(commandType, messageText);

            var chatId = GetChatId(message);
            var text = GetTextAnswer(commandType, messageText);
            var markup = GetMarkup(messageText);

            await _botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: text,
                    replyMarkup: markup,
                    cancellationToken: cancellationToken);

            if (commandType == SCREEN_LABEL)
                if (messageText == BotFunctions.SCREENSHOT)
                    await SendScreenshotAsync(chatId, PathManager.GetScreenshotAbsolutePath(), cancellationToken);
        }

        private async Task<Task> HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            if (ENABLE_LOGGING)
                Log.UnhandledException(exception);

            return Task.CompletedTask;
        }

        private async Task SendScreenshotAsync(long chatId, string filepath, CancellationToken cancellationToken)
        {
            using var stream = new FileStream(filepath, FileMode.Open, FileAccess.Read);

            await _botClient.SendPhotoAsync(
                chatId: chatId,
                photo: new InputOnlineFile(stream),
                cancellationToken: cancellationToken);
        }

        private static Message GetMessage(Update update)
        {
            return update.Message!;
        }

        private static User? GetMessageSender(Message message)
        {
            return message.From;
        }

        private static string GetMessageText(Message message)
        {
            return message.Text!.Trim();
        }

        private static long GetChatId(Message message)
        {
            return message.Chat.Id;
        }

        private static string GetTextAnswer(string? commandType, string messageText)
        {
            string answer;

            if (commandType is null)
                answer = "Unknown command";
            else if (commandType == BACK_LABEL)
                answer = "...";
            else if (commandType == POWER_LABEL)
                answer = GetTextAnswerByPowerCommand(messageText);
            else if (commandType == VOLUME_LABEL)
                answer = GetTextAnswerByVolumeCommand(messageText);
            else if (commandType == SCREEN_LABEL)
                answer = GetTextAnswerByScreenCommand(messageText);
            else
                answer = AnswerGenerator.GetBotFunctionNotImplementedAnswer();

            return answer;
        }

        private static IReplyMarkup GetMarkup(string messageText)
        {
            return GetKeyboard(messageText);
        }
    }
}
