// ReSharper disable once CheckNamespace
namespace Yawat.Authenticator

{
    using System.Threading.Tasks;

    public interface ITokenService
    {
        Task<string> GetToken();
    }
}
