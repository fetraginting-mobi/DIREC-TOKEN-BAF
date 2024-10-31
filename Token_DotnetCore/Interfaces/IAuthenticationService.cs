namespace Token_Ifinsys.Interfaces
{
    public interface IAuthenticationService
    {
        User Login(string userName, string password);
    }

    public class User
    {
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        // other properties
    }
}
