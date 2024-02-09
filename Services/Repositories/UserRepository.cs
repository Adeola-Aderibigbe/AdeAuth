using AdeAuth.Models;

namespace AdeAuth.Services.Repositories
{
    public class UserRepository : IUserRepository
    {
        public UserRepository(IList<User> users)
        {
            _users = users;
        }
        public void AddUser(User user)
        {
             user.Id = Guid.NewGuid();
            _users.Add(user);
        }

        public User? GetUser(Guid id)
        {
            return _users.FirstOrDefault(s => s.Id == id);
        }

        public User? GetUserByEmail(string email,AuthenticationType authenticationType)
        {
            return _users.FirstOrDefault(s => s.Email == email && s.AuthenticationType == authenticationType );
        }

        public User? GetUserByEmail(string email)
        {
            return _users.FirstOrDefault(s => s.Email == email);
        }

        public readonly IList<User> _users;
    }
}
