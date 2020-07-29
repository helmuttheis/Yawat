namespace Yawat.Cli
{
    using Yawat.ResponseData;

    public class Config
    {
        public const string BaseRoute = "/api/TodoItems";

        public Config()
        {
            this.Options = new Yawat.YawatOptions(typeof(JsonResponseData))
            {
                Host = YawatSettings.Get("Host", "localhost"),
                Protocol = YawatSettings.Get("Protocol", "https"),
                Port = YawatSettings.Get("Port", "44328")
            };

            this.HttpClientWithOptions = new YawatHttpClient(this.Options);
        }

        private Yawat.YawatOptions Options { get;  }

        public Yawat.YawatHttpClient HttpClientWithOptions { get; }
    }
}
