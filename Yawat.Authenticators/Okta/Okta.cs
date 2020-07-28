// ReSharper disable once CheckNamespace
namespace Yawat.Authenticator
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using Yawat.Interfaces;

    public class OktaAuthenticator : IAuthenticator
    {
        private readonly List<string> errList = new List<string>();

        private readonly ITokenService tokenService;

        public OktaAuthenticator(
            string tokenUrl,
            string clientId,
            string clientSecret,
            string scope)
        {
            var oktaSettings = new OktaSettings
                {TokenUrl = tokenUrl, ClientId = clientId, ClientSecret = clientSecret, Scope = scope};

            this.tokenService = new OktaTokenService(oktaSettings);
        }

        public List<string> GetErrors()
        {
            return this.errList;
        }

        public async Task<bool> Setup()
        {
            await Task.Delay(100);
            return true;
        }

        public bool UpdateHttpClient(HttpClient client)
        {
            Task.Run(async () =>
            {
                var token = await this.tokenService.GetToken();

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }).Wait();

            return true;
        }
    }
}