using Telegram.Bot.Types;

namespace RemoteControlBot
{
    internal static class UpdateValidator
    {
        internal static bool IsSequenceValid(IEnumerable<bool> sequence)
        {
            return sequence.All(x => x);
        }

        internal static bool IsUpdateValid(Update update)
        {
            return update.Message?.Text is not null;
        }

        internal static bool IsAccessAllowed(Bot bot, User? user)
        {
            return user?.Id == bot.OwnerId;
        }

        internal static bool IsMessageAfterStartup(DateTime startupTime, DateTime sendTime)
        {
            return startupTime < sendTime;
        }
    }
}
