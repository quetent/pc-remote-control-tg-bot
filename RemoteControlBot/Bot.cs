using AudioSwitcher.AudioApi.CoreAudio;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using static RemoteControlBot.Keyboard;
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

            if (!(isValid && IsAccessAllowed(user)))
                return;

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
        }

        private async Task<Task> HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            if (_enableLogging)
                Log.UnhandledException(exception);

            return Task.CompletedTask;
        }

        private static string? DetermineCommandType(string command)
        {
            string? commandType;

            if (MAIN_MENU_LABELS.Contains(command) || command == BACK_LABEL)
                commandType = BACK_LABEL;
            else if (POWER_LABELS.Contains(command))
                commandType = POWER_LABEL;
            else if (VOLUME_LABELS.Contains(command))
                commandType = VOLUME_LABEL;
            else if (SCREEN_LABELS.Contains(command))
                commandType = SCREEN_LABEL;
            else
                commandType = null;

            return commandType;
        }

        private void ExecuteCommand(string? commandType, string commandText)
        {
            if (commandType is null)
            {
                if (_enableLogging)
                    Log.UnknownCommand(commandText);
                
                return;
            }

            if (commandType == BACK_LABEL)
            {
                if (_enableLogging)
                    Log.KeyboardRequest();

                return;
            }

            if (commandType == VOLUME_LABEL)
                ExecuteVolumeCommand(commandText);
            //else if (commandType == SCREEN_LABEL)
            //else if (commandType == POWER_LABEL)

            Log.CommandExecute(commandText);
        }

        private static void ExecuteVolumeCommand(string textMessage)
        {
            switch (textMessage)
            {
                case LOUDER_5:
                    VolumeManager.ChangeVolume(5);
                    break;
                case QUIETER_5:
                    VolumeManager.ChangeVolume(-5);
                    break;
                case LOUDER_10:
                    VolumeManager.ChangeVolume(10);
                    break;
                case QUIETER_10:
                    VolumeManager.ChangeVolume(-10);
                    break;
                case MAX:
                    VolumeManager.ChangeVolume(100);
                    break;
                case MIN:
                    VolumeManager.ChangeVolume(-100);
                    break;
                case MUTE:
                    VolumeManager.Mute();
                    break;
                case UNMUTE:
                    VolumeManager.UnMute();
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        private static bool IsUpdateValid(Update update)
        {
            return update.Message?.Text is not null;
        }

        private bool IsAccessAllowed(User? user)
        {
            return user?.Id == _ownerId;
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

        private static string GetTextAnswer(string commandType, string messageText)
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
                throw new NotImplementedException();

            return answer;
        }

        private static string GetTextAnswerByPowerCommand(string commandText)
        {
            string answer;

            if (commandText == SHUTDOWN)
                answer = "Shutdown has been requested";
            else if (commandText == RESTART)
                answer = "Restart has been requested";
            else if (commandText == SLEEP)
                answer = "Sleep has been requested";
            else
                throw new NotImplementedException();

            return answer;
        }

        private static string GetTextAnswerByVolumeCommand(string commandText)
        {
            string answer;
            string insertion;

            int volumeLevel;
            bool isBadMuteRequest = VolumeManager.IsBadMuteRequest();

            switch (commandText)
            {
                case LOUDER_5:
                    volumeLevel = VolumeManager.GetCurrentVolumeLevel();
                    if (volumeLevel == 100)
                        goto case MAX;
                    answer = $"Volume increased ({volumeLevel - 5} -> {volumeLevel})";
                    break;
                case QUIETER_5:
                    volumeLevel = VolumeManager.GetCurrentVolumeLevel();
                    if (volumeLevel == 0)
                        goto case MIN;
                    answer = $"Volume increased ({volumeLevel + 5} -> {volumeLevel})";
                    break;
                case LOUDER_10:
                    volumeLevel = VolumeManager.GetCurrentVolumeLevel();
                    if (volumeLevel == 100)
                        goto case MAX;
                    answer = $"Volume increased ({volumeLevel - 10} -> {volumeLevel})";
                    break;
                case QUIETER_10:
                    volumeLevel = VolumeManager.GetCurrentVolumeLevel();
                    if (volumeLevel == 0)
                        goto case MIN;
                    answer = $"Volume increased ({volumeLevel + 10} -> {volumeLevel})";
                    break;
                case MAX:
                    answer = "Volume is set to max";
                    break;
                case MIN:
                    answer = "Volume is set to min";
                    break;
                case MUTE:
                    if (isBadMuteRequest)
                        insertion = "already";
                    else
                        insertion = "has been";
                    answer = $"Speaker {insertion} muted";
                    break;
                case UNMUTE:
                    if (isBadMuteRequest)
                        insertion = "is not";
                    else
                        insertion = "has been";
                    answer = $"Speaker {insertion} muted";
                    break;
                default:
                    throw new NotImplementedException();
            }

            return answer;
        }

        private static string GetTextAnswerByScreenCommand(string commandText)
        {
            string answer;

            if (commandText == SCREENSHOT)
                answer = "Screenshot was taken";
            else
                throw new NotImplementedException();

            return answer;
        }

        private static IReplyMarkup GetMarkup(string messageText)
        {
            IReplyMarkup markup;

            if (MAIN_MENU_LABELS.Contains(messageText))
                if (messageText == POWER_LABEL)
                    markup = Power;
                else if (messageText == VOLUME_LABEL)
                    markup = Volume;
                else if (messageText == SCREEN_LABEL)
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
    }
}
