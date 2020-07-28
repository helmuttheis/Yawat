// ReSharper disable once CheckNamespace
namespace Yawat.Authenticator
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;

    using IdentityModel.Client;

    using Yawat.Interfaces;

    public class IdentityServerAuthenticator : IAuthenticator
    {
        private readonly string identityServerUrl;
        private readonly string clientId;
        private readonly string clientSecret;
        private readonly string scope;

        private readonly List<string> errors;

        private TokenResponse tokenResponse;

        public IdentityServerAuthenticator(string identityServerUrl, string clientId, string clientSecret, string scope)
        {
            this.identityServerUrl = identityServerUrl;
            this.clientId = clientId;
            this.clientSecret = clientSecret;
            this.scope = scope;

            this.errors = new List<string>();
        }

        public async Task<bool> Setup()
        {
            // discover endpoints from metadata
            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync(this.identityServerUrl);
            if (disco.IsError)
            {
                this.errors.Add($"GetDiscoveryDocumentAsync({this.identityServerUrl}) was not successful");
                return false;
            }

            // request token
            this.tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,

                ClientId = this.clientId,
                ClientSecret = this.clientSecret,
                Scope = this.scope
            });
            if (this.tokenResponse.IsError)
            {
                this.errors.Add($"RequestClientCredentialsTokenAsync({this.identityServerUrl}) was not successful, Error={this.tokenResponse.Error}");
            }

            return !this.tokenResponse.IsError;
        }

        public List<string> GetErrors()
        {
            return this.errors;
        }

        public bool UpdateHttpClient(HttpClient client)
        {
            client.SetBearerToken(this.tokenResponse.AccessToken);
            return true;
        }
    }
}
