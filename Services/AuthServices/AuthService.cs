using AdeAuth.Models;
using AdeAuth.Models.Dtos;
using AdeAuth.Services.Repositories;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using System.Text;

namespace AdeAuth.Services.AuthServices
{
    public class AuthService : IAuthService
    {
        public AuthService(IUserRepository userRepository, IPasswordManager passwordManager)
        {
           _userRepository = userRepository;
           _passwordManager = passwordManager;
        }
        public string LoginUser(LoginDto loginDto, AuthenticationType authenticationType = AuthenticationType.Default)
        {
            var currentUser = _userRepository.GetUserByEmail(loginDto.Email,authenticationType);
            if(currentUser == null)
            {
                return "Invalid email/password";
            }

            var isVerified = _passwordManager.VerifyPassword(loginDto.Password, currentUser.PasswordHash);
            if(isVerified)
            {
                return Encrypt(loginDto.Email, currentUser.Id.ToString() ,currentUser.AccountType.ToString());
            }

            return "Invalid email/password";
        }

        public Guid SignUpUser(SignUpDto signUpDto, AccountType accountType, AuthenticationType authenticationType = AuthenticationType.Default)
        {
            var currentUser = _userRepository.GetUserByEmail(signUpDto.Email,authenticationType);
            if (currentUser == null)
            {
                var newUser = new User(signUpDto.FirstName, signUpDto.LastName, signUpDto.Email, accountType, authenticationType);
                if (!string.IsNullOrEmpty(signUpDto.Password))
                {
                    newUser.SetPasswordHash(_passwordManager.HashPassword(signUpDto.Password));
                }
                _userRepository.AddUser(newUser);
                return newUser.Id;
            }
            return new Guid();
        }

        public User? IsUserExist(string email, AuthenticationType authenticationType = AuthenticationType.Default)
        {
            return _userRepository.GetUserByEmail(email,authenticationType);
        }

        public string[] DecryptData(string data)
        {
            var decodedData = Convert.FromBase64String(data);
            var result = Encoding.UTF8.GetString(decodedData);
            return result.Split('-');
        }

        public string Encrypt(string email,string id,string accountType)
        {
            var encodedData = Encoding.UTF8.GetBytes($"{email}-${id}-${accountType}");
            return Convert.ToBase64String(encodedData);
        }

        

        private readonly IUserRepository _userRepository;

        private readonly IPasswordManager _passwordManager;
    }
}
