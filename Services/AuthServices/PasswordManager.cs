using BC = BCrypt.Net.BCrypt;

namespace AdeAuth.Services.AuthServices
{
    public class PasswordManager : IPasswordManager
    {
        public string HashPassword(string plainPassword)
        {
            return BC.HashPassword(plainPassword);
        }

        public bool VerifyPassword(string plainPassword, string hashPassword)
        {
            return BC.Verify(plainPassword, hashPassword);
        }
    }
}
