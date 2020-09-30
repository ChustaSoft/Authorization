using ChustaSoft.Tools.Authorization.Models;
using System.Security.Authentication;

namespace ChustaSoft.Tools.Authorization
{
    public static class UserValidationExtensions
    {

        public static LoginType GetLoginType(this UserValidation userValidation)
        {
            if (IsEmailLogin(userValidation))
                return LoginType.MAIL;
            else if (IsPhoneLogin(userValidation))
                return LoginType.PHONE;
            else
                throw new AuthenticationException("User must inform Email or Phone, and confirmation token for validating it");
        }


        private static bool IsEmailLogin(UserValidation userValidation)
            => !string.IsNullOrEmpty(userValidation.Email) && !string.IsNullOrEmpty(userValidation.ConfirmationToken);

        private static bool IsPhoneLogin(UserValidation userValidation)
            => !string.IsNullOrEmpty(userValidation.Phone) && !string.IsNullOrEmpty(userValidation.ConfirmationToken);

    }
}
