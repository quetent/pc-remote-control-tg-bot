namespace RemoteControlBot
{
    public static class App
    {
        public static void Restart(StartUpCode startUpCode)
        {
            ProcessManager.StartProcess(Environment.ProcessPath!, $"StartUpCode={startUpCode}", false);

            Exit();
        }

        public static void Exit()
        {
            Environment.Exit(0);
        }
    }
}
