using Microsoft.AspNetCore.Identity;
using System.Security.Authentication;

namespace ChustaSoft.Tools.Authorization
{
    internal static class SignInExtension
    {

        internal static bool Successful(this SignInResult result) => result == SignInResult.Success;


        internal static bool Failed(this SignInResult result) => result == SignInResult.Failed;


        internal static void ManageUnsucceededSignin(this SignInResult result)
        {
            if (result == SignInResult.LockedOut)
            {
                throw new AuthenticationException($"User is locked");
            }
            else if (result == SignInResult.NotAllowed)
            {
                throw new AuthenticationException($"User is not allowed to sign in");
            }
            else if (result == SignInResult.TwoFactorRequired)
            {
                throw new AuthenticationException($"Two factor authentication is required");
            }
            else
            {
                throw new AuthenticationException($"Unable to sign in user");
            }
        }
        


    }
}
