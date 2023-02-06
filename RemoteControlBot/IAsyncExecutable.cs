namespace RemoteControlBot
{
    interface IAsyncExecutable
    {
        public Task ExecuteAsync(CancellationToken cancellationToken);
    }
}
