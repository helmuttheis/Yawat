namespace Yawat.Interfaces
{
    using System.Collections.Generic;

    using System.Net.Http;
    using System.Threading.Tasks;

    public interface IAuthenticator
    {
        Task<bool> Setup();

        bool UpdateHttpClient(HttpClient client);

        List<string> GetErrors();
    }
}
