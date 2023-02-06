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

        internal static void NotImplemented(Command command)
        {
            NotImplemented<bool>(command.ToString());
        }

        internal static T NotImplemented<T>(Command command)
        {
            return NotImplemented<T>($"Command not implemented {command}");
        }

        internal static T NotImplemented<T>(string message)
        {
            throw new NotImplementedException(message);
        }
    }
}
