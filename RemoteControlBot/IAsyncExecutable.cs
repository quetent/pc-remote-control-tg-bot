namespace RemoteControlBot
{
    public interface IAsyncExecutable
    {
        public Task ExecuteAsync(long commandSenderId, CancellationToken cancellationToken);
    }
}
