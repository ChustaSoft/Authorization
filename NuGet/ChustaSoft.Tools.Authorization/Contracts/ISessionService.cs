using ChustaSoft.Tools.Authorization.Models;
using System.Threading.Tasks;

namespace ChustaSoft.Tools.Authorization
{
    /// <summary>
    /// Session service
    /// </summary>
    public interface ISessionService
    {

        /// <summary>
        /// Register user
        /// </summary>
        /// <param name="credentials">User credentials</param>
        /// <returns>Session with token for the user, and confirmation tokens</returns>
        Task<ValidableSession> RegisterAsync(ValidableCredentials credentials);

        /// <summary>
        /// Authenticate user, independently of the mechanims
        /// </summary>
        /// <param name="credentials">User credentials</param>
        /// <returns>Session with token for the user</returns>
        Task<Session> AuthenticateAsync(Credentials credentials);

        /// <summary>
        /// Validate user email or phone
        /// </summary>
        /// <param name="userValidation">User email/phone and confirmation token</param>
        /// <returns>Session with token for the user</returns>
        Task<Session> ValidateAsync(UserValidation userValidation);

        /// <summary>
        /// Activate or deactivate a user
        /// </summary>
        /// <param name="userActivation">Username, password and activation flag</param>
        /// <returns>Result flag</returns>
        Task<bool> ActivateAsync(UserActivation userActivation);

        /// <summary>
        /// Authenticate the external user, creating it first if not exists
        /// </summary>
        /// <returns></returns>
        Task AuthenticateExternalAsync();

        /// <summary>
        /// Generated the token to reset user password
        /// </summary>
        /// <param name="credentials">User credentials</param>
        /// <returns></returns>
        Task<string> GenerateResetPasswordTokenAsync(ResetPasswordCredentials credentials);

        /// <summary>
        /// Reset user password
        /// </summary>
        /// <param name="user">The user to reset the password</param>
        /// <param name="token">Reset password token</param>
        /// <param name="newPassword">New password to set</param>
        Task ResetPasswordAsync(ResetPasswordCredentials credentials);
    }
}