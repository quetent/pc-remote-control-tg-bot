using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;
using System.Management;

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
        private Command _previousCommand;

        public Bot(long ownerId,
                   string token,
                   ReceiverOptions recieverOptions,
                   CancellationToken cancellationToken)
        {
            _ownerId = ownerId;
            _botClient = new TelegramBotClient(token);
            _receiverOptions = recieverOptions;
            _cancellationToken = cancellationToken;

            CommandExecuter.CommandExecuted += HandleCommandExecuted;

            //if (ENABLE_PRE_INIT)
            //    VolumeManager.PreInit();
        }

        public async Task StartAsync()
        {
            _startupTime = DateTimeManager.GetCurrentDateTime();

            await NotifyAboutStartReceivingAsync(OWNER_ID, "Bot has been started", Keyboard.MainMenu, _cancellationToken);

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

            if (!isUpdateValid)
            {
                if (ENABLE_LOGGING)
                    Log.MessageSkipped("< invalid message >", null);

                return;
            }

            var recieveTime = GetUpdateSendDatetime(update);

            var message = GetMessage(update);
            var messageText = GetMessageText(message);
            var user = GetMessageSender(message);

            var sequence = new[]
            {
                isUpdateValid,
                UpdateValidator.IsAccessAllowed(this, user),
                UpdateValidator.IsMessageAfterStartup(_startupTime, recieveTime)
            };

            if (!UpdateValidator.IsSequenceValid(sequence))
            {
                if (ENABLE_LOGGING)
                    Log.MessageSkipped(messageText, user);

                return;
            }

            if (ENABLE_LOGGING)
                Log.MessageRecieved(messageText, user);

            var command = GetCommand(messageText, _previousCommand);

            if (ENABLE_LOGGING)
                Log.UpdateExecute(command, messageText);

            var chatId = GetChatId(message);

            await CommandExecuter.ExecuteCommandAsync(command, chatId, cancellationToken);

            var text = GetTextAnswer(command);
            var markup = GetMarkup(command);

            await _botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: text,
                    replyMarkup: markup,
                    cancellationToken: cancellationToken);

            SetPreviousCommand(command);
        }

        private async Task<Task> HandlePollingErrorAsync(ITelegramBotClient botClient,
                                                         Exception exception,
                                                         CancellationToken cancellationToken)
        {
            if (ENABLE_LOGGING)
                Log.UnhandledException(exception);

            return Task.CompletedTask;
        }

        private async Task HandleCommandExecuted(Command command, long commandSenderId, CancellationToken cancellationToken)
        {
            switch (command.Type)
            {
                case CommandType.Screen:
                    if (command.Info is CommandInfo.Screenshot)
                        await SendScreenshotAsync(commandSenderId, cancellationToken);
                    break;
                default:
                    break;
            }
        }

        private async Task NotifyAboutStartReceivingAsync(long chatId, string text, IReplyMarkup markup, CancellationToken cancellationToken)
        {
            await _botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: text,
                    replyMarkup: markup,
                    cancellationToken: cancellationToken);
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

        private static Command GetCommand(string messageText, Command previousCommand)
        {
            Command command;

            if (Command.IsNumberForProccesManager(previousCommand, messageText))
                command = new Command(CommandType.Process, CommandInfo.Kill, messageText);
            else
                command = new Command(messageText);

            return command;
        }

        private void SetPreviousCommand(Command command)
        {
            _previousCommand = command;
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
            return command.Type switch
            {
                CommandType.Undefined => TextAnswerGenerator.GetAnswerByUndefinedCommand(command),
                CommandType.Transfer => TextAnswerGenerator.GetAnswerByTransferCommand(command),
                CommandType.Power => TextAnswerGenerator.GetAnswerByPowerCommand(command),
                CommandType.Volume => TextAnswerGenerator.GetAnswerByVolumeCommand(command),
                CommandType.Screen => TextAnswerGenerator.GetAnswerByScreenCommand(command),
                CommandType.Process => TextAnswerGenerator.GetAnswerByProcessCommand(command),
                CommandType.AdminPanel => TextAnswerGenerator.GetAnswerByControlCommand(command),
                _ => Throw.CommandNotImplemented<string>(command)
            };
        }

        private static IReplyMarkup GetMarkup(Command command)
        {
            return MarkupGenerator.GetKeyboard(command);
        }
    }
}
