using ChustaSoft.Tools.Authorization.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

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

        internal static TUser ToUser<TUser>(this ExternalLoginInfo loginInfo)
           where TUser : User, new()
        {
            var credentials = new Credentials
            {
                Email = loginInfo.Principal.FindFirstValue(ClaimTypes.Email),
                Username = NormalizeUsername(loginInfo)
            };

            TUser user = credentials.ToUser<TUser>().WithFullAccess();

            return user;
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

        internal static bool HasValidEmail<TUser>(this TUser user)
            where TUser : User, new()
        { 
            return !string.IsNullOrWhiteSpace(user.Email) && !user.Email.EndsWith(AuthorizationConstants.NO_EMAIL_SUFFIX_FORMAT);
        }

        internal static bool HasValidPhone<TUser>(this TUser user)
            where TUser : User, new()
        { 
            return !string.IsNullOrWhiteSpace(user.PhoneNumber);
        }


        private static string NormalizeUsername(ExternalLoginInfo loginInfo)
        {
            string emailUsername = loginInfo.Principal.FindFirstValue(ClaimTypes.Email);
            emailUsername = !string.IsNullOrEmpty(emailUsername) && emailUsername.Contains("@") ? emailUsername.Split("@")[0] : string.Empty;

            string username = loginInfo.Principal.FindFirstValue(ClaimTypes.Name)?.Replace(" ", "");

            string normalizedUsername = string.Empty;

            if (!string.IsNullOrEmpty(username))            
                normalizedUsername = username;
            
            if (!string.IsNullOrEmpty(emailUsername))            
                normalizedUsername += $"_{emailUsername}";            

            if (normalizedUsername.StartsWith("_"))            
                normalizedUsername = normalizedUsername.Substring(1);            

            return normalizedUsername;
        }

    }
}
