using AdeAuth.Models;

namespace AdeAuth.Services
{
    public static class DeployConfiguration
    {
        public static List<User> AddNewUsers()
        {
            return new List<User>
            {
                new("Adeola","Aderibigbe","Email"),
                new("Wuraola","Aderibigbe","Email2")
            };
        }
    }
}
