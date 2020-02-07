using ChustaSoft.Common.Contracts;


namespace ChustaSoft.Tools.Authorization
{
    public class CredentialsMapper : IMapper<User, Credentials>
    {

        private AuthorizationSettings _authorizationSettings;


        public CredentialsMapper(AuthorizationSettings authorizationSettings)
        {
            _authorizationSettings = authorizationSettings;
        }


        public Credentials MapFromSource(User user)
            => new Credentials
                {
                    Username = user.UserName,
                    Email = user.Email,
                    Culture = user.Culture
                };

        public User MapToSource(Credentials credentials)
            => new User
                {
                    UserName = credentials.Username,
                    Email = credentials.Email,
                    PasswordHash = credentials.Password,
                    Culture = string.IsNullOrEmpty(credentials.Culture) ? _authorizationSettings.DefaultCulture : credentials.Culture
            };

    }
}