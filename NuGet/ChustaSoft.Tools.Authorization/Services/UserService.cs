using ChustaSoft.Tools.Authorization.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ChustaSoft.Tools.Authorization
{
    public class UserService<TUser> : ServiceBase, IUserService<TUser>, IReviewModelService<TUser>
         where TUser : User, new()
    {

        #region Fields

        private readonly SignInManager<TUser> _signInManager;
        private readonly UserManager<TUser> _userManager;


        #endregion

        #region Constants

        private const string CREATE_ACTION = "create";
        private const string LOGIN_ACTION = "login";
        private const string USER_NOT_ALLOWED_TO_LOGIN = "User not allowed to login in the system";

        #endregion


        #region Events

        public event EventHandler<UserEventArgs> UserCreatedEventHandler;

        #endregion


        #region Constructor

        public UserService(AuthorizationSettings authorizationSettings, SignInManager<TUser> signInManager, UserManager<TUser> userManager)
            : this(authorizationSettings, signInManager, userManager, null)
        { }

        public UserService(
                AuthorizationSettings authorizationSettings, 
                SignInManager<TUser> signInManager, UserManager<TUser> userManager, 
                EventHandler<UserEventArgs> userCreatedEventHandler)
            : base(authorizationSettings)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            
            if(userCreatedEventHandler != null)
                UserCreatedEventHandler += userCreatedEventHandler;
        }

        #endregion


        #region Public methods

        public async Task<TUser> GetAsync(Guid userId)
        {
            return await _userManager.FindByIdAsync(userId.ToString());
        }

        public async Task<TUser> GetAsync(string username, string password)
        {
            var userSignIn = await _signInManager.PasswordSignInAsync(username, password, isPersistent: false, lockoutOnFailure: true);

            if (userSignIn.Succeeded)            
                return await _userManager.FindByNameAsync(username);            
            else            
                return null;            
        }

        public async Task<TUser> SignByUsername(string username, string password)
        {
            var user = await GetAsync(username, password);

            if (user != null && user.IsActive)
                return user;

            throw new AuthenticationException(USER_NOT_ALLOWED_TO_LOGIN);
        }

        public async Task<TUser> SignByEmail(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user != null && user.HasValidEmail() && user.IsActive)
            {
                var userSignIn = await _signInManager.PasswordSignInAsync(user.UserName, password, isPersistent: false, lockoutOnFailure: true);

                if (userSignIn.Succeeded)
                    return user;
            }

            throw new AuthenticationException(USER_NOT_ALLOWED_TO_LOGIN);
        }

        public async Task<TUser> SignByPhone(string phone, string password)
        {
            var user = _userManager.Users.FirstOrDefault(x => x.PhoneNumber == phone && (!_authorizationSettings.ConfirmationRequired || x.PhoneNumberConfirmed));

            if (user != null && user.IsActive)
            {
                var userSignIn = await _signInManager.PasswordSignInAsync(user.UserName, password, isPersistent: false, lockoutOnFailure: true);

                if (userSignIn.Succeeded)
                    return user;
            }

            throw new AuthenticationException("User not allowed to login in the system");
        }

        public async Task<TUser> ConfirmEmail(string email, string token)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user != null)
            {
                var confirmationResult = await _userManager.ConfirmEmailAsync(user, token);

                if (confirmationResult.Succeeded)
                    return user;
            }

            throw new AuthenticationException("Invalid email or token for confirmation");
        }

        public async Task<TUser> ConfirmPhone(string phone, string token)
        {
            var user = _userManager.Users.FirstOrDefault(x => x.PhoneNumber == phone);

            if (user != null)
            {
                var confirmationResult = await _userManager.ChangePhoneNumberAsync(user, user.PhoneNumber, token);
                
                await TryPerformFakeEmailActions(user);

                if (confirmationResult.Succeeded)
                    return user;
            }

            throw new AuthenticationException("Invalid phone or token for confirmation");
        }

        public async Task<bool> CreateAsync(TUser user, string password, IDictionary<string, string> parameters)
        {
            Review(user);
            var result = await _userManager.CreateAsync(user, password);

            await TryAddConfirmationTokens(user, parameters, result);
            TryRaiseUserCreatedEvent(user, parameters, result);

            return result.Succeeded;
        }

        public async Task<bool> UpdateAsync(TUser user)
        {
            var result = await _userManager.UpdateAsync(user);

            return result.Succeeded;
        }

        public async Task<bool> ExistAsync(string userEmail)
        {
            var result = await _userManager.FindByEmailAsync(userEmail);

            return result != null;
        }

        public async Task<bool> AssignRoleAsync(Guid userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            return await AssignRoleAsync(user, roleName);
        }

        public async Task<bool> AssignRoleAsync(TUser user, string roleName)
        {
            var result = await _userManager.AddToRoleAsync(user, roleName);

            return result.Succeeded;
        }

        public async Task<bool> AssignRolesAsync(TUser user, IEnumerable<string> roleNames)
        {
            var result = await _userManager.AddToRolesAsync(user, roleNames);

            return result.Succeeded;
        }

        public async Task<bool> AssignClaimAsync(Guid userId, string claimName)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            return await AssignClaimAsync(user, claimName);
        }

        public async Task<bool> AssignClaimAsync(TUser user, string claimName)
        {
            var result = await _userManager.AddClaimAsync(user, new Claim(AuthorizationConstants.CLAIM_PERMISSION_KEY, claimName));

            return result.Succeeded;
        }

        public async Task<bool> AssignClaimsAsync(TUser user, IEnumerable<string> claimNames)
        {
            var claims = claimNames.Select(x => new Claim(AuthorizationConstants.CLAIM_PERMISSION_KEY, x));
            var result = await _userManager.AddClaimsAsync(user, claims);

            return result.Succeeded;
        }

        public void Review(TUser user) 
        {
            if (string.IsNullOrEmpty(user.Email) && !string.IsNullOrEmpty(user.PhoneNumber))
                user.Email = $"{user.PhoneNumber}{AuthorizationConstants.NO_EMAIL_SUFFIX_FORMAT}";
            if (string.IsNullOrEmpty(user.Culture))
                user.Culture = _authorizationSettings.DefaultCulture;
        }

        public async Task<SignInResult> ExternalSignInAsync(bool isPersistent)
        {
            var loginInfo = await _signInManager.GetExternalLoginInfoAsync();
            return await _signInManager.ExternalLoginSignInAsync(loginInfo.LoginProvider, loginInfo.ProviderKey, isPersistent);
        }

        public async Task CreateExternalAsync(string defaultRole)
        {
            var loginInfo = await _signInManager.GetExternalLoginInfoAsync();

            Credentials credentials = new Credentials{ 
                Email = loginInfo.Principal.FindFirstValue(ClaimTypes.Email),
                Username = NormalizeUsername(loginInfo)
            };

            TUser user = credentials.ToUser<TUser>().WithFullAccess();
            
            var result = await _userManager.CreateAsync(user);

            manageIdentityResult(result, CREATE_ACTION);

            if (!string.IsNullOrEmpty(defaultRole))
            {
                await AssignRoleAsync(user, defaultRole);
            }

            result = await _userManager.AddLoginAsync(user, loginInfo);

            manageIdentityResult(result, LOGIN_ACTION);
        }

        #endregion


        #region Private methods

        private async Task TryAddConfirmationTokens(TUser user, IDictionary<string, string> parameters, IdentityResult result)
        {
            if (result.Succeeded && _authorizationSettings.ConfirmationRequired)
            {
                if (user.HasValidEmail())
                {
                    var confirmEmailToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                    parameters.Add("EmailConfirmationToken", confirmEmailToken);
                }
                if (user.HasValidPhone())
                {
                    var confirmPhoneToken = await _userManager.GenerateChangePhoneNumberTokenAsync(user, user.PhoneNumber);

                    parameters.Add("PhoneConfirmationToken", confirmPhoneToken);
                }
            }
        }

        private void TryRaiseUserCreatedEvent(TUser user, IDictionary<string, string> parameters, IdentityResult result)
        {
            if (result.Succeeded)
                UserCreatedEventHandler?.Invoke(this, new UserEventArgs(user.Id, parameters));
        }

        private async Task TryPerformFakeEmailActions(TUser user)
        {
            if (!user.HasValidEmail())
            {
                var fakeEmailToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                await _userManager.ConfirmEmailAsync(user, fakeEmailToken);
            }
        }

        #endregion

        #region Private methods

        private void manageIdentityResult(IdentityResult result, string actionName)
        {
            if (result != IdentityResult.Success)
            {
                string errorMessage = result.Errors != null && result.Errors.Count() > 0 ? result.Errors.First().Description : string.Empty;
                throw new AuthenticationException($"Unable to {actionName} user. {errorMessage}");
            }
        }

        private string NormalizeUsername(ExternalLoginInfo loginInfo)
        {
            string emailUsername = loginInfo.Principal.FindFirstValue(ClaimTypes.Email);
            emailUsername = !string.IsNullOrEmpty(emailUsername) && emailUsername.Contains("@") ? emailUsername.Split("@")[0] : string.Empty;

            string username = loginInfo.Principal.FindFirstValue(ClaimTypes.Name);

            string normalizedUsername = string.Empty;

            if (!string.IsNullOrEmpty(username))
            {
                normalizedUsername = username;
            }
            if (!string.IsNullOrEmpty(emailUsername))
            {
                normalizedUsername += $"_{emailUsername}";
            }

            if (normalizedUsername.StartsWith("_"))
            {
                normalizedUsername = normalizedUsername.Substring(1);
            }

            return normalizedUsername;
        }

        #endregion

    }



    #region Default Implementation

    public class UserService : UserService<User>, IUserService, IUserRoleService, IUserClaimService
    {
        public UserService(AuthorizationSettings authorizationSettings, SignInManager<User> signInManager, UserManager<User> userManager)
            : base(authorizationSettings, signInManager, userManager)
        { }

        public UserService(AuthorizationSettings authorizationSettings, SignInManager<User> signInManager, UserManager<User> userManager, EventHandler<UserEventArgs> func)
           : base(authorizationSettings, signInManager, userManager, func)
        { }
    }

    #endregion

}
