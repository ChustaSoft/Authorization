using ChustaSoft.Tools.Authorization.Models;

namespace ChustaSoft.Tools.Authorization
{
    internal static class UserExtensions
    {

        internal static TUser ToUser<TUser>(this Credentials credentials)
            where TUser : User, new()
        {
            return new TUser
            {
                UserName = credentials.Username,
                Email = credentials.Email,
                PhoneNumber = credentials.Phone,
                PasswordHash = credentials.Password,
                Culture = credentials.Culture,
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
