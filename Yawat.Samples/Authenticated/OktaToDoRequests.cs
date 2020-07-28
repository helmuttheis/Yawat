namespace Authenticated
{
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using NUnit.Framework;

    using Yawat;
    using Yawat.ResponseData;

    [TestFixture]
    public class OktaToDoRequests
    {
        private string baseRoute = string.Empty;

        private YawatOptions options;

        [SetUp]
        public async Task Setup()
        {
            this.baseRoute = YawatSettings.Get("Okta.BaseRoute");
            var clientId = YawatSettings.Get("Okta.ClientId");
            var clientSecret = YawatSettings.Get("Okta.ClientSecret");
            var scope = YawatSettings.Get("Okta.Scope");
            var tokenUrl = YawatSettings.Get("Okta.TokenUrl");

            this.options = new YawatOptions(typeof(JsonResponseData))
            {
                Authenticator = new Yawat.Authenticator.OktaAuthenticator( tokenUrl, clientId, clientSecret, scope)
            };

            var success = await this.options.Authenticator.Setup();
            if (!success)
            {
                Console.WriteLine("IsAuthenticator.Setup() was not successful.");
                foreach (var error in this.options.Authenticator.GetErrors())
                {
                    Console.WriteLine($"    {error}");
                }
            }
        }

        [Test]
        public async Task ShouldGetJsonFromGet()
        {
            var result = await SetupTeardown.HttpClientWithOptions.GetAsync($"{this.baseRoute}", this.options);

            var json = result.AsString();

            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);

            Assert.AreNotEqual(string.Empty, json);
        }
    }
}
