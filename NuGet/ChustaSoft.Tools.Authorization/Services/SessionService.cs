using ChustaSoft.Tools.Authorization.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Security.Authentication;
using System.Threading.Tasks;

namespace ChustaSoft.Tools.Authorization
{
    public class SessionService<TUser> : ServiceBase, ISessionService
         where TUser : User, new()
    {

        #region Fields

        private readonly ISecuritySettings _securitySettings;

        private readonly IUserService<TUser> _userService;

        private readonly ITokenHelper<TUser> _tokenHelper;

        #endregion


        #region Constructor

        public SessionService(
                AuthorizationSettings authorizationSettings,
                ISecuritySettings securitySettings,
                IUserService<TUser> userService,
                ITokenHelper<TUser> tokenService)
            : base(authorizationSettings)
        {
            _securitySettings = securitySettings;

            _userService = userService;
            
            _tokenHelper = tokenService;
        }

        #endregion


        #region Public methods

        public async Task<ValidableSession> RegisterAsync(ValidableCredentials credentials)
        {
            var user = credentials.ToUser<TUser>();
            var resultFlag = await _userService.CreateAsync(user, credentials.Password, credentials.Parameters);

            if (resultFlag)
            {
                var tokenInfo = _tokenHelper.Generate(user, _securitySettings.PrivateKey);
                var session = new ValidableSession(user, tokenInfo, credentials.Parameters);

                return session;
            }
            else
                throw new AuthenticationException($"User {user.UserName} could not be created");
        }

        public async Task<Session> AuthenticateAsync(Credentials credentials)
        {
            var user = await LoginAsync(credentials);
            var session = GetUserSession(user);

            return session;
        }

        public async Task<Session> ValidateAsync(UserValidation userValidation)
        {
            var loginType = userValidation.GetLoginType();
            var user = await PerformValidation(userValidation, loginType);
            var session = GetUserSession(user);

            return session;
        }

        public async Task<bool> ActivateAsync(UserActivation userActivation)
        {
            var user = await _userService.GetAsync(userActivation.Username, userActivation.Password);

            user.IsActive = userActivation.Activate;

            return await _userService.UpdateAsync(user);
        }

        public AuthenticationProperties BuildAuthenticationProperties(string provider, string loginCallbackUrl)
        {
            return _userService.BuildAuthenticationProperties(provider, loginCallbackUrl);
        }

        public async Task AuthenticateExternalAsync()
        {
            var loginInfo = await _userService.GetExternalLoginInfoAsync();

            var result = await _userService.ExternalSignInAsync(loginInfo.LoginProvider, loginInfo.ProviderKey, isPersistent: false);

            if (result == SignInResult.Success)
            {
                return;
            }
            else if (result == SignInResult.LockedOut)
            {
                throw new AuthenticationException($"User is locked");
            }
            else
            {
                await _userService.CreateExternalAsync(loginInfo);
                await _userService.ExternalSignInAsync(loginInfo.LoginProvider, loginInfo.ProviderKey, isPersistent: false);
            }
        }

        #endregion


        #region Private methods

        private async Task<TUser> LoginAsync(Credentials credentials)
        {
            var loginType = credentials.GetLoginType();

            return loginType switch
            {
                LoginType.USER => await _userService.SignByUsername(credentials.Username, credentials.Password),
                LoginType.MAIL => await _userService.SignByEmail(credentials.Email, credentials.Password),
                LoginType.PHONE => await _userService.SignByPhone(credentials.Phone, credentials.Password),

                _ => throw new AuthenticationException("User could not by logged in into the system"),
            };
        }

        private async Task<TUser> PerformValidation(UserValidation userValidation, LoginType loginType)
        {
            return loginType switch
            {
                LoginType.MAIL => await _userService.ConfirmEmail(userValidation.Email, userValidation.ConfirmationToken),
                LoginType.PHONE => await _userService.ConfirmPhone(userValidation.Phone, userValidation.ConfirmationToken),

                _ => throw new AuthenticationException("User could not by validated into the system"),
            };
        }

        private Session GetUserSession(TUser user)
        {
            var tokenInfo = _tokenHelper.Generate(user, _securitySettings.PrivateKey);
            var session = new Session(user, tokenInfo);

            return session;
        }

        #endregion

    }



    #region Default Implementation

    public class SessionService : SessionService<User>
    {
        public SessionService(
                AuthorizationSettings authorizationSettings,
                ISecuritySettings securitySettings,
                IUserService userService,
                ITokenHelper tokenService)
            : base(authorizationSettings, securitySettings, userService, tokenService)
        { }
    }

    #endregion

}
