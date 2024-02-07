using AdeAuth.Models;
using AdeAuth.Models.Dtos;

namespace AdeAuth.Services.AuthServices
{
    public interface IAuthService
    {
        string LoginUser(LoginDto loginDto);
        Guid SignUpUser(SignUpDto signUpDto, AccountType accountType = AccountType.User, AuthenticationType authenticationType = AuthenticationType.Default);
        string Encrypt(string email, string id, string accountType);
        string[] DecryptData(string data);
        User IsUserExist(string email);
    }
}
