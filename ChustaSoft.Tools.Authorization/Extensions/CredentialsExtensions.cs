using ChustaSoft.Tools.Authorization.Models;
using System.Security.Authentication;

namespace ChustaSoft.Tools.Authorization
{
    public static class CredentialsExtensions
    {

        public static LoginType GetLoginType(this Credentials credentials)
        {
            if (IsUsernameLogin(credentials))
                return LoginType.USER;
            else if (IsEmailLogin(credentials))
                return LoginType.MAIL;
            else if (IsPhoneLogin(credentials))
                return LoginType.PHONE;
            else
                throw new AuthenticationException("User must inform Username or Email and password for login");
        }

        public static TUser ToUser<TUser>(this Credentials credentials, string defaultCulture)
            where TUser : User, new()
        { 
            return new TUser
            {
                UserName = credentials.Username,
                Email = string.IsNullOrEmpty(credentials.Email) ? $"{credentials.Phone}{AuthorizationConstants.NO_EMAIL_SUFFIX_FORMAT}" : credentials.Email,
                PhoneNumber = credentials.Phone,
                PasswordHash = credentials.Password,
                Culture = string.IsNullOrEmpty(credentials.Culture) ? defaultCulture : credentials.Culture,
                IsActive = true
            };
        }


        private static bool IsUsernameLogin(Credentials credentials)
            => !string.IsNullOrEmpty(credentials.Username) && !string.IsNullOrEmpty(credentials.Password);

        private static bool IsEmailLogin(Credentials credentials)
            => !string.IsNullOrEmpty(credentials.Email) && !string.IsNullOrEmpty(credentials.Password);

        private static bool IsPhoneLogin(Credentials credentials)
            => !string.IsNullOrEmpty(credentials.Phone) && !string.IsNullOrEmpty(credentials.Password);

    }
}
