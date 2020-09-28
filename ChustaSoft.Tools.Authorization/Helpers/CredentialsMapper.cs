using ChustaSoft.Common.Contracts;
using ChustaSoft.Tools.Authorization.Models;

namespace ChustaSoft.Tools.Authorization
{
    public class CredentialsMapper<TUser> : IMapper<TUser, Credentials>
        where TUser : User, new()
    {

        private readonly AuthorizationSettings _authorizationSettings;


        public CredentialsMapper(AuthorizationSettings authorizationSettings)
        {
            _authorizationSettings = authorizationSettings;
        }


        public Credentials MapFromSource(TUser user)
            => new Credentials
                {
                    Username = user.UserName,
                    Email = user.Email,
                    Phone = user.PhoneNumber,
                    Culture = user.Culture
                };

        public TUser MapToSource(Credentials credentials)
            => new TUser
            {
                    UserName = credentials.Username,
                    Email = string.IsNullOrEmpty(credentials.Email) ?  $"{credentials.Phone}{AuthorizationConstants.NO_EMAIL_SUFFIX_FORMAT}" : credentials.Email,
                    PhoneNumber = credentials.Phone,
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