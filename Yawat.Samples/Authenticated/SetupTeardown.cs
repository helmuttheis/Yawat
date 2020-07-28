namespace Authenticated
{
    using NUnit.Framework;

    using Yawat;
    using Yawat.ResponseData;

    [SetUpFixture]
    public class SetupTeardown
    {
        public static Yawat.YawatOptions Options { get; private set; }

        public static Yawat.YawatHttpClient HttpClientWithOptions { get; private set; }

        [OneTimeSetUp]
        public void RunBeforeAnyTests()
        {
            Options = new Yawat.YawatOptions(typeof(JsonResponseData))
            {
                Host = YawatSettings.Get("Host", "localhost"),
                Protocol = YawatSettings.Get("Protocol", "https"),
                Port = YawatSettings.Get("Port",  "44328")
            };

            HttpClientWithOptions = new YawatHttpClient(Options);
        }

        [OneTimeTearDown]
        public void RunAfterAnyTests()
        {
            // ...
        }
    }
}
