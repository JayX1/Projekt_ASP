namespace ASP_projekt.Interfaces
{
    public interface ICustomAuthService
    {
        bool ValidateUser(string username, string password);
    }
}
