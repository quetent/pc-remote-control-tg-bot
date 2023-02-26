using Telegram.Bot.Types;

namespace RemoteControlBot
{
    public static class UpdateValidator
    {
        public static bool IsValid(params bool[] sequence)
        {
            return sequence.All(x => x);
        }

        public static bool IsUpdateValid(Update update)
        {
            return update.Message?.Text is not null;
        }

        public static bool IsAccessAllowed(Bot bot, User? user)
        {
            return user?.Id == bot.OwnerId;
        }

        public static bool IsMessageAfterStartup(DateTime startupTime, DateTime sendTime)
        {
            return startupTime < sendTime;
        }
    }
}
