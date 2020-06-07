using ChustaSoft.Common.Contracts;


namespace ChustaSoft.Tools.Authorization
{
    public class CredentialsMapper<TUser> : IMapper<TUser, Credentials>
        where TUser : User, new()
    {

        private AuthorizationSettings _authorizationSettings;


        public CredentialsMapper(AuthorizationSettings authorizationSettings)
        {
            _authorizationSettings = authorizationSettings;
        }


        public Credentials MapFromSource(TUser user)
            => new Credentials
                {
                    Username = user.UserName,
                    Email = user.Email,
                    Culture = user.Culture
                };

        public TUser MapToSource(Credentials credentials)
            => new TUser
            {
                    UserName = credentials.Username,
                    Email = credentials.Email,
                    PasswordHash = credentials.Password,
                    Culture = string.IsNullOrEmpty(credentials.Culture) ? _authorizationSettings.DefaultCulture : credentials.Culture
            };

    }


    public class CredentialsMapper : CredentialsMapper<User> 
    {

        public CredentialsMapper(AuthorizationSettings authorizationSettings)
            : base(authorizationSettings)
        { }

    }

}