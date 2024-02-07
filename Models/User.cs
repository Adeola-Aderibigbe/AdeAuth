using System.ComponentModel.DataAnnotations;
using System.Security.Principal;

namespace AdeAuth.Models
{
    public class User
    {
        protected User() { }
        public User(string firstName, string lastName,string email)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
        }
        public User(string firstName, string lastName, string email, AccountType accountType)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            AccountType = accountType;
        }

        public User(string firstName, string lastName, string email, AccountType accountType,AuthenticationType authenticationType)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            AccountType = accountType;
            AuthenticationType = authenticationType;
        }


        public void SetPasswordHash(string passwordHash)
        {
            PasswordHash = passwordHash;
        }


        [Key]
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public AccountType AccountType { get; set; } = AccountType.User;
        public AuthenticationType AuthenticationType { get; set; } = AuthenticationType.Default;
        
    }
}
