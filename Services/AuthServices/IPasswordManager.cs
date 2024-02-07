namespace AdeAuth.Services.AuthServices
{
    public interface IPasswordManager
    {
        string HashPassword(string plainPassword);

        bool VerifyPassword(string plainPassword, string hashPassword);
    }
}
