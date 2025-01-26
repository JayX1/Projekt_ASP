using Microsoft.Extensions.Configuration;
using ASP_projekt.Interfaces;
using ASP_projekt.Models;

namespace ASP_projekt.Services
{
    public class CustomAuthService : ICustomAuthService
    {
        private readonly IConfiguration _configuration;
        public CustomAuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public bool ValidateUser(string username, string password)
        {
            var users = _configuration.GetSection("Users").Get<List<User>>();
            return users.Any(user => user.Username == username && user.Password == password);
        }
    }
}
