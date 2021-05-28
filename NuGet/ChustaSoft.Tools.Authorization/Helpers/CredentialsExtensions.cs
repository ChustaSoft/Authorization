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

        public static LoginType GetLoginType(this ResetPasswordCredentials credentials)
        {
            if (IsUsernameLogin(credentials))
                return LoginType.USER;
            else if (IsEmailLogin(credentials))
                return LoginType.MAIL;
            else if (IsPhoneLogin(credentials))
                return LoginType.PHONE;
            else
                throw new AuthenticationException("User must inform Username, Email or Phone for ResetPassword");
        }

        private static bool IsUsernameLogin(Credentials credentials)
            => !string.IsNullOrEmpty(credentials.Username) && !string.IsNullOrEmpty(credentials.Password);

        private static bool IsEmailLogin(Credentials credentials)
            => !string.IsNullOrEmpty(credentials.Email) && !string.IsNullOrEmpty(credentials.Password);

        private static bool IsPhoneLogin(Credentials credentials)
            => !string.IsNullOrEmpty(credentials.Phone) && !string.IsNullOrEmpty(credentials.Password);

        private static bool IsUsernameLogin(ResetPasswordCredentials credentials)
            => !string.IsNullOrEmpty(credentials.Username);

        private static bool IsEmailLogin(ResetPasswordCredentials credentials)
            => !string.IsNullOrEmpty(credentials.Email);

        private static bool IsPhoneLogin(ResetPasswordCredentials credentials)
            => !string.IsNullOrEmpty(credentials.Phone);

    }
}
