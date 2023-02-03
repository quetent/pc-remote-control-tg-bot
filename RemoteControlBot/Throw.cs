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

        internal static void CommandNotImplemented(Command command)
        {
            CommandNotImplemented<bool>(command.ToString());
        }

        internal static T CommandNotImplemented<T>(Command command)
        {
            return CommandNotImplemented<T>(command.ToString());
        }

        internal static T CommandNotImplemented<T>(string commandText)
        {
            throw new NotImplementedException($"Command not implemented {commandText}");
        }
    }
}
