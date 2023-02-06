namespace RemoteControlBot
{
    internal static class Throw
    {
        internal static void IfIncorrectCommandType(Command command, CommandType expectedCommandType)
        {
            if (command.Type != expectedCommandType)
                throw new ArgumentException(command.ToString(), command.Type.ToString());
        }

        internal static T ShouldBeNotReachable<T>()
        {
            throw new InvalidOperationException();
        }

        internal static void NotImplemented(string message)
        {
            NotImplemented<bool>(message);
        }

        internal static T NotImplemented<T>(string message)
        {
            throw new NotImplementedException($"Not implemented: {message}");
        }
    }
}
