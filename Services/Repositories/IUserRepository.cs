using AdeAuth.Models;

namespace AdeAuth.Services.Repositories
{
    public interface IUserRepository
    {
        void AddUser(User user);
        User? GetUser(Guid id);
        User? GetUserByEmail(string email, AuthenticationType authenticationType);
        User? GetUserByEmail(string email);
    }
}
