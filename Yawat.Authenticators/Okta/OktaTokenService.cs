// ReSharper disable once CheckNamespace
namespace Yawat.Authenticator
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using Newtonsoft.Json;

    public class OktaTokenService : ITokenService
    {
        private OktaToken token = new OktaToken();
        private readonly OktaSettings oktaSettings;

        public OktaTokenService(OktaSettings oktaSettings)
        {
            this.oktaSettings = oktaSettings;
        }

        public async Task<string> GetToken()
        {
            if (!this.token.IsValidAndNotExpiring)
            {
                this.token = await this.GetNewAccessToken();
            }

            return this.token.AccessToken;
        }

        private async Task<OktaToken> GetNewAccessToken()
        {
            OktaToken newToken;
            var client = new HttpClient();
            var clientId = this.oktaSettings.ClientId;
            var clientSecret = this.oktaSettings.ClientSecret;
            var clientCredentials = System.Text.Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}");
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Basic", System.Convert.ToBase64String(clientCredentials));

            var postMessage = new Dictionary<string, string>
            {
                { "grant_type", "client_credentials" },
                { "scope", this.oktaSettings.Scope }
            };
            var request = new HttpRequestMessage(HttpMethod.Post, this.oktaSettings.TokenUrl)
            {
                Content = new FormUrlEncodedContent(postMessage)
            };
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            var response = await client.SendAsync(request);
            string jsonString;
            try
            {
                jsonString = await response.Content.ReadAsStringAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            if (response.IsSuccessStatusCode)
            {
                newToken = JsonConvert.DeserializeObject<OktaToken>(jsonString);
                newToken.ExpiresAt = DateTime.UtcNow.AddSeconds(newToken.ExpiresIn);
            }
            else
            {
                throw new ApplicationException("Unable to retrieve access token from Okta: " + jsonString);
            }

            return newToken;
        }

        private class OktaToken
        {
            [JsonProperty(PropertyName = "access_token")]
            public string AccessToken { get; set; }

            [JsonProperty(PropertyName = "expires_in")]
            public int ExpiresIn { get; set; }

            public DateTime ExpiresAt { get; set; }

            public string Scope { get; set; }

            [JsonProperty(PropertyName = "token_type")]
            public string TokenType { get; set; }

            public bool IsValidAndNotExpiring => !string.IsNullOrEmpty(this.AccessToken) && this.ExpiresAt > DateTime.UtcNow.AddSeconds(30);
        }
    }
}
