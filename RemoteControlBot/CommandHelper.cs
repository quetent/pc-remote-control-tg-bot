namespace RemoteControlBot
{
    public static class CommandHelper
    {
        public static bool IsNumberForProccesManager(Command previousCommand, string messageText)
        {
            return previousCommand.Type is CommandType.Transfer
                && previousCommand.Info is CommandInfo.ToKillList
                && messageText.IsNumber();
        }

        public static bool IsNumberForScreenshotManager(Command previousCommand, string messageText)
        {
            return previousCommand.Type is CommandType.Transfer
                && previousCommand.Info is CommandInfo.ToScreensList
                && messageText.IsNumber();
        }

    }
}
