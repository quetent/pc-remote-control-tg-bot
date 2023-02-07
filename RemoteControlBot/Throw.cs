namespace RemoteControlBot
{
    public static class Throw
    {
        public static void IfIncorrectCommandType(Command command, CommandType expectedCommandType)
        {
            if (command.Type != expectedCommandType)
                throw new ArgumentException(command.ToString(), command.Type.ToString());
        }

        public static void IfIncorrectCommandInfo(Command command, CommandInfo expectedCommandInfo)
        {
            if (command.Info != expectedCommandInfo)
                throw new ArgumentException(command.ToString(), command.Info.ToString());
        }

        public static T ShouldBeNotReachable<T>()
        {
            throw new InvalidOperationException();
        }

        public static void NotImplemented(string message)
        {
            NotImplemented<bool>(message);
        }

        public static T NotImplemented<T>(string message)
        {
            throw new NotImplementedException($"Not implemented: {message}");
        }
    }
}
