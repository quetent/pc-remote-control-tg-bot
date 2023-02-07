namespace RemoteControlBot
{
    public interface IAsyncExecutable
    {
        public Task ExecuteAsync(CancellationToken cancellationToken);
    }
}
