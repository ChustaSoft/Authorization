using ChustaSoft.Tools.Authorization.Models;
using System.Collections.Generic;
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
                var roles = await _userService.GetRolesAsync(user);
                var tokenInfo = _tokenHelper.Generate(user, roles, _securitySettings.PrivateKey);
                var session = new ValidableSession(user, tokenInfo, credentials.Parameters);

                return session;
            }
            else
                throw new AuthenticationException($"User {user.UserName} could not be created");
        }

        public async Task<Session> AuthenticateAsync(Credentials credentials)
        {
            var user = await LoginAsync(credentials);
            var roles = await _userService.GetRolesAsync(user);

            var session = GetUserSession(user, roles);

            return session;
        }

        public async Task<Session> ValidateAsync(UserValidation userValidation)
        {
            var loginType = userValidation.GetLoginType();
            var user = await PerformValidation(userValidation, loginType);
            var roles = await _userService.GetRolesAsync(user);
            var session = GetUserSession(user, roles);

            return session;
        }

        public async Task<bool> ActivateAsync(UserActivation userActivation)
        {
            var user = await _userService.GetAsync(userActivation.Username, userActivation.Password);

            user.IsActive = userActivation.Activate;

            return await _userService.UpdateAsync(user);
        }

        public async Task AuthenticateExternalAsync()
        {
            var result = await _userService.ExternalSignInAsync(isPersistent: false);

            if (result.Successful())
            {
                return;
            }            
            else if (result.Failed())
            {
                await _userService.CreateExternalAsync();
                result = await _userService.ExternalSignInAsync(isPersistent: false);

                if (!result.Successful())
                {
                    result.ManageUnsucceededSignin();
                }
            }
            else
            {
                result.ManageUnsucceededSignin();
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

        private Session GetUserSession(TUser user, IEnumerable<string> roles)
        {
            var tokenInfo = _tokenHelper.Generate(user, roles, _securitySettings.PrivateKey);
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
