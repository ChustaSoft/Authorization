using ChustaSoft.Common.Contracts;
using ChustaSoft.Tools.Authorization.Models;
using Microsoft.AspNetCore.Authentication;
using System.Security.Authentication;
using System.Threading.Tasks;

namespace ChustaSoft.Tools.Authorization
{
    public class SessionService<TUser> : ISessionService
         where TUser : User, new()
    {

        #region Fields

        private readonly ISecuritySettings _securitySettings;

        private readonly IUserService<TUser> _userService;

        private readonly ITokenHelper<TUser> _tokenHelper;

        private readonly IMapper<TUser, Credentials> _userMapper;
        private readonly IMapper<TUser, TokenInfo, Session> _sessionMapper;

        #endregion


        #region Constructor

        public SessionService(
                ISecuritySettings securitySettings,
                IUserService<TUser> userService,
                ITokenHelper<TUser> tokenService, 
                IMapper<TUser, Credentials> userMapper, IMapper<TUser, TokenInfo, Session> sessionMapper)
        {
            _securitySettings = securitySettings;

            _userService = userService;
            
            _tokenHelper = tokenService;

            _userMapper = userMapper;
            _sessionMapper = sessionMapper;
        }

        #endregion


        #region Public methods

        public async Task<Session> AuthenticateAsync(Credentials credentials)
        {
            var user = await LoginAsync(credentials);
            var session = GetUserSession(user);

            return session;
        }

        public async Task<Session> RegisterAsync(Credentials credentials)
        {
            var user = _userMapper.MapToSource(credentials);
            var resultFlag = await _userService.CreateAsync(user, credentials.Password, credentials.Parameters);

            if (resultFlag)
            {
                var tokenInfo = _tokenHelper.Generate(user, _securitySettings.PrivateKey);
                var session = _sessionMapper.MapFromSource(user, tokenInfo);

                return session;
            }
            else
                throw new AuthenticationException($"User {user.UserName} could not be created");
        }

        public async Task<Session> ValidateAsync(UserValidation userValidation)
        {
            var loginType = userValidation.GetLoginType();
            var user = await PerformValidation(userValidation, loginType);
            var session = GetUserSession(user);

            return session;
        }
       
        public AuthenticationProperties GetExternalProperties(string provider, string loginCallbackUrl)
        {
            return _userService.GetExternalProperties(provider, loginCallbackUrl);
        }

        #endregion


        #region Private methods

        private async Task<TUser> LoginAsync(Credentials credentials)
        {
            var loginType = credentials.GetLoginType();

            return loginType switch
            {
                LoginType.USER => await _userService.GetByUsername(credentials.Username, credentials.Password),
                LoginType.MAIL => await _userService.GetByEmail(credentials.Email, credentials.Password),
                LoginType.PHONE => await _userService.GetByPhone(credentials.Phone, credentials.Password),

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
            var session = _sessionMapper.MapFromSource(user, tokenInfo);

            return session;
        }

        #endregion

    }



    #region Default Implementation

    public class SessionService : SessionService<User>
    {
        public SessionService(
                ISecuritySettings securitySettings,
                IUserService userService,
                ITokenHelper tokenService,
                IMapper<User, Credentials> userMapper, IMapper<User, TokenInfo, Session> sessionMapper)
            : base(securitySettings, userService, tokenService, userMapper, sessionMapper)
        { }
    }

    #endregion

}
