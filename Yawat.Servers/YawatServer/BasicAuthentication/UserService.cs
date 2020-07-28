namespace YawatServer.BasicAuthentication
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IUserService
    {
        Task<User> Authenticate(string username, string password);

        Task<IEnumerable<User>> GetAll();
    }

    public class UserService : IUserService
    {
        // users hardcoded for simplicity, store in a db with hashed passwords in production applications
        private readonly List<User> users = new List<User>
        {
            new User { Id = "testuser@dev.null", FirstName = "Test", LastName = "User", Username = "test", Password = "test" }
        };

        public async Task<User> Authenticate(string username, string password)
        {
            var user = await Task.Run(() => this.users.SingleOrDefault(x => x.Username == username && x.Password == password));

            // return null if user not found

            // authentication successful so return user details without password
            return user?.WithoutPassword();
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await Task.Run(() => this.users.WithoutPasswords());
        }
    }
}