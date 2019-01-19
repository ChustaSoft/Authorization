using ChustaSoft.Tools.Authorization.Enums;
using ChustaSoft.Tools.Authorization.Models;
using System.Security.Authentication;


namespace ChustaSoft.Tools.Authorization.Services
{
    public class CredentialsBusiness : ICredentialsService
    {

        #region Public methods

        public LoginType ValidateCredentials(Credentials credentials)
        {
            if (PerformLoginByMail(credentials))
                return LoginType.MAIL;
            else if (PerformLoginByCode(credentials))
                return LoginType.CODE;
            else
                throw new AuthenticationException("User must inform Username or Email and password for login");
        }

        #endregion


        #region Private methods

        private static bool PerformLoginByMail(Credentials credentials) => !string.IsNullOrEmpty(credentials.Email) && !string.IsNullOrEmpty(credentials.Password);

        private static bool PerformLoginByCode(Credentials credentials) => !string.IsNullOrEmpty(credentials.Username) && !string.IsNullOrEmpty(credentials.Password);

        #endregion

    }
}
