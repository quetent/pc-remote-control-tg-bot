namespace RemoteControlBot
{
    public static class HttpClientUtilities
    {
        public static async Task WaitForInternetConnectionAsync(string url, int timeoutMs, int requestsDelay)
        {
            while (!await CheckForInternetConnectionAsync(url, timeoutMs))
                Thread.Sleep(requestsDelay);
        }

        public static async Task<bool> CheckForInternetConnectionAsync(string url, int timeoutMs)
        {
            try
            {
                using var client = new HttpClient
                {
                    Timeout = TimeSpan.FromSeconds(timeoutMs)
                };

                var response = await client.GetAsync(url);

                return true;
            }
            catch (HttpRequestException)
            {
                return false;
            }
        }
    }
}
