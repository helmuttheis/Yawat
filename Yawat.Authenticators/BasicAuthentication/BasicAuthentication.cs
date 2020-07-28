// ReSharper disable once CheckNamespace
namespace Yawat.Authenticator
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;

    using Yawat.Interfaces;

    public class BasicAuthenticator : IAuthenticator
    {
        private readonly AuthenticationHeaderValue authenticationHeaderValue;

        private readonly List<string> errors;

        public BasicAuthenticator(string userName, string password)
        {
            this.errors = new List<string>();

            this.authenticationHeaderValue = new System.Net.Http.Headers.AuthenticationHeaderValue(
                "Basic",
                Convert.ToBase64String(
                    Encoding.UTF8.GetBytes(
                        $"{userName}:{password}")));
        }

        public async Task<bool> Setup()
        {
            await Task.Delay(0);
            return true;
        }

        public List<string> GetErrors()
        {
            return this.errors;
        }

        public bool UpdateHttpClient(HttpClient client)
        {
            client.DefaultRequestHeaders.Authorization = this.authenticationHeaderValue;

            return true;
        }
    }
}
