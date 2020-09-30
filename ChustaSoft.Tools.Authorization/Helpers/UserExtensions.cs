using ChustaSoft.Tools.Authorization.Models;

namespace ChustaSoft.Tools.Authorization
{
    internal static class UserExtensions
    {

        internal static TUser ToUser<TUser>(this Credentials credentials, string defaultCulture)
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


        internal static TUser WithFullAccess<TUser>(this TUser user)
            where TUser : User, new()
        {
            user.LockoutEnabled = false;
            user.IsActive = true;
            user.EmailConfirmed = true;
            user.PhoneNumberConfirmed = true;

            return user;
        }

    }
}
