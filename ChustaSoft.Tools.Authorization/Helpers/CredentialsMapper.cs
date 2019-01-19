using ChustaSoft.Common.Helpers;
using ChustaSoft.Tools.Authorization.Models;


namespace ChustaSoft.Tools.Authorization.Helpers
{
    public class CredentialsMapper : IMapper<User, Credentials>
    {

        public Credentials MapFromSource(User user)
            => new Credentials
                {
                    Username = user.UserName,
                    Email = user.Email
                };

        public User MapToSource(Credentials credentials)
            => new User
                {
                    UserName = credentials.Username,
                    Email = credentials.Email,
                    PasswordHash = credentials.Password,
                };

    }
}