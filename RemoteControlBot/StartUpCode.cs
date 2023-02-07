namespace RemoteControlBot
{
    public enum StartUpCode
    {
        Null,
        Crashed,
        RestartRequested,
        ConnectionLost
    }

    public class StartUpCodeUtilities
    {
        public static StartUpCode ParseStartUpCode(string[] startUpArgs)
        {
            StartUpCode startUpCode = StartUpCode.Null;

            foreach (var arg in startUpArgs)
            {
                var argData = arg.Split("=");

                if (argData.Length == 2 && argData[0] == "StartUpCode")
                {
                    startUpCode = argData[1] switch
                    {
                        "Null" => StartUpCode.Null,
                        "Crashed" => StartUpCode.Crashed,
                        "RestartRequested" => StartUpCode.RestartRequested,
                        "ConnectionLost" => StartUpCode.ConnectionLost,
                        _ => Throw.NotImplemented<StartUpCode>($"{typeof(StartUpCodeUtilities)} -> {argData[1]}")
                    };

                    break;
                }
            }

            return startUpCode;
        }
    }
}
