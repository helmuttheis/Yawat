namespace Authenticated
{
    using System;
    using System.Threading.Tasks;
    using NUnit.Framework;

    using Yawat;
    using Yawat.ResponseData;

    [TestFixture]
    public class IsToDoRequests
    {
        private string baseRoute = string.Empty;

        private YawatOptions options;

        [SetUp]
        public async Task Setup()
        {
            this.baseRoute = YawatSettings.Get("IdentityServer.BaseRoute");
            var identityServerUrl = YawatSettings.Get("IdentityServer.IdentityServerUrl");
            var clientId = YawatSettings.Get("IdentityServer.ClientId");
            var clientSecret = YawatSettings.Get("IdentityServer.ClientSecret");
            var scope = YawatSettings.Get("IdentityServer.Scope");

            this.options = new YawatOptions(typeof(JsonResponseData))
            {
                Authenticator = new Yawat.Authenticator.IdentityServerAuthenticator(identityServerUrl, clientId, clientSecret, scope)
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

            Assert.AreNotEqual(json, string.Empty);
        }
    }
}
