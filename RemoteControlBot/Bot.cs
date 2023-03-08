using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;

namespace RemoteControlBot
{
    public class Bot
    {
        public readonly long OwnerId;

        private readonly TelegramBotClient _botClient;
        private readonly ReceiverOptions _receiverOptions;

        private DateTime _startupTime;
        private readonly CancellationToken _cancellationToken;

        private static Command PreviousExecutedCommand => Execute.LastExecutedCommand;

        public Bot(long ownerId,
                   string token,
                   ReceiverOptions recieverOptions,
                   CancellationToken cancellationToken)
        {
            OwnerId = ownerId;
            _botClient = new TelegramBotClient(token);
            _receiverOptions = recieverOptions;
            _cancellationToken = cancellationToken;

            Execute.CommandExecuted += HandleCommandExecuted;
        }

        public async Task StartAsync(StartUpCode startUpCode)
        {
            _startupTime = DateTimeManager.GetCurrentDateTime();
            var message = new StartUpAnswerGenerator(startUpCode).GetAnswer();

            await NotifyOwnerAboutStartUp(message);

            _botClient.StartReceiving(
                updateHandler: HandleUpdateAsync,
                pollingErrorHandler: HandlePollingErrorAsync,
                receiverOptions: _receiverOptions,
                cancellationToken: _cancellationToken);

            Log.If(ENABLE_LOGGING, () => Log.BotStartup(message));
        }

        private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var isUpdateValid = UpdateValidator.IsUpdateValid(update);

            if (!isUpdateValid)
            {
                Log.If(ENABLE_LOGGING, () => Log.MessageSkipped("< invalid message >", null));
                return;
            }

            var updateSendTime = GetUpdateSendDatetime(update);
            var message = GetMessage(update);
            var messageText = GetMessageText(message);
            var user = GetMessageSender(message);

            if (!UpdateValidator.IsValid(
                    isUpdateValid,
                    UpdateValidator.IsAccessAllowed(this, user),
                    UpdateValidator.IsMessageAfterStartup(_startupTime, updateSendTime)))
            {
                Log.If(ENABLE_LOGGING, () => Log.MessageSkipped(messageText, user));
                return;
            }

            Log.If(ENABLE_LOGGING, () => Log.MessageRecieved(messageText, user));

            var chatId = GetChatId(message);
            var command = GetCommand(messageText, chatId, PreviousExecutedCommand);

            Log.If(ENABLE_LOGGING, () => Log.UpdateExecute(command, messageText));

            await new Execute(command).ExecuteAsync(cancellationToken);

            var text = GetTextAnswer(command);
            var markup = GetMarkup(command);

            await SendMessageAsync(chatId, text, markup, cancellationToken);
        }

        private async Task<Task> HandlePollingErrorAsync(ITelegramBotClient botClient,
                                                         Exception exception,
                                                         CancellationToken cancellationToken)
        {
            if (exception is AppRestartRequested)
            {
                Log.If(ENABLE_LOGGING, () => Log.AppRestart());
                App.Restart(StartUpCode.RestartRequested);
            }
            else if (exception is AppExitRequested)
            {
                Log.If(ENABLE_LOGGING, () => Log.AppExit());
                App.Exit();
            }
            else if (exception is RequestException)
                await HandleConnectionLost();

            Log.If(ENABLE_LOGGING, () => Log.UnhandledException(exception));

            await Execute.ExecuteIfAsync(() => !AUTO_RESTART_IF_CRASHED, () =>
            {
                Log.If(ENABLE_LOGGING, () => Log.AppRestart());
                App.Restart(StartUpCode.Crashed);
            });

            return Task.CompletedTask;
        }

        private static async Task HandleConnectionLost()
        {
            Log.If(ENABLE_LOGGING, () => Log.ConnectionLost());

            await Execute.ExecuteIfAsync(() => AUTO_RESTART_IF_CONNECTION_LOST, async () =>
            {
                Log.If(ENABLE_LOGGING, () => Log.WaitingForInternetConnection());

                await HttpClientUtilities.WaitForInternetConnectionAsync(
                        INTERNET_CHECKING_URL,
                        5000,
                        3000);

                Log.If(ENABLE_LOGGING, () =>
                {
                    Log.ConnectionRestored();
                    Log.AppRestart();
                });

                App.Restart(StartUpCode.ConnectionLost);
            });
        }

        private async Task HandleCommandExecuted(Command command, CancellationToken cancellationToken)
        {
            switch (command.Type)
            {
                case CommandType.Screen:
                    if (command.Info is CommandInfo.Screenshot && ScreenManager.IsValidLastIndex)
                    {
                        Log.If(ENABLE_LOGGING, () => Log.ScreenshotSending());
                        await SendScreenshotAsync(command.SenderId, cancellationToken);
                    }
                    break;
                default:
                    break;
            }
        }

        private async Task SendMessageAsync(long chatId, string text, IReplyMarkup markup, CancellationToken cancellationToken)
        {
            await _botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: text,
                    replyMarkup: markup,
                    cancellationToken: cancellationToken);
        }

        private async Task NotifyOwnerAboutStartUp(string message)
        {
            await SendMessageAsync(OwnerId, message, Keyboard.MainMenu, _cancellationToken);
        }

        private async Task SendScreenshotAsync(long chatId, CancellationToken cancellationToken)
        {
            await SendScreenshotAsync(chatId, PathManager.GetScreenshotAbsolutePath(), cancellationToken);
        }

        private async Task SendScreenshotAsync(long chatId, string filepath, CancellationToken cancellationToken)
        {
            using var stream = new FileStream(filepath, FileMode.Open, FileAccess.Read);

            await _botClient.SendPhotoAsync(
                    chatId: chatId,
                    photo: new InputOnlineFile(stream),
                    cancellationToken: cancellationToken);
        }

        private static Command GetCommand(string messageText, long senderId, Command previousCommand)
        {
            Command command;

            if (CommandHelper.IsNumberForProccesManager(previousCommand, messageText))
                command = new Command(CommandType.Process, CommandInfo.Kill, messageText, senderId);
            else if (CommandHelper.IsNumberForScreenshotManager(previousCommand, messageText))
                command = new Command(CommandType.Screen, CommandInfo.Screenshot, messageText, senderId);
            else
                command = new Command(messageText, senderId);

            return command;
        }

        private static DateTime GetUpdateSendDatetime(Update update)
        {
            return update.Message!.Date.ToLocalTime();
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

        private static string GetTextAnswer(Command command)
        {
            return new TextAnswerGenerator(command).GetAnswer();
        }

        private static IReplyMarkup GetMarkup(Command command)
        {
            return new MarkupGenerator(command).GetMarkup();
        }
    }
}
