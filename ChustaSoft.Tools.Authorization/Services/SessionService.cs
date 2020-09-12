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

        private readonly ICredentialsBusiness _credentialsBusiness;

        private readonly ITokenHelper<TUser> _tokenHelper;

        private readonly IMapper<TUser, Credentials> _userMapper;
        private readonly IMapper<TUser, TokenInfo, Session> _sessionMapper;

        #endregion


        #region Constructor

        public SessionService(
                ISecuritySettings securitySettings,
                IUserService<TUser> userService,
                ICredentialsBusiness credentialsBusiness, 
                ITokenHelper<TUser> tokenService, 
                IMapper<TUser, Credentials> userMapper, IMapper<TUser, TokenInfo, Session> sessionMapper)
        {
            _securitySettings = securitySettings;

            _userService = userService;
            _credentialsBusiness = credentialsBusiness;

            _tokenHelper = tokenService;

            _userMapper = userMapper;
            _sessionMapper = sessionMapper;
        }

        #endregion


        #region Public methods

        public async Task<Session> AuthenticateAsync(Credentials credentials)
        {
            var user = await LoginAsync(credentials);
            var tokenInfo = _tokenHelper.Generate(user, _securitySettings.PrivateKey);
            var session = _sessionMapper.MapFromSource(user, tokenInfo);

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

        public AuthenticationProperties BuildAuthenticationProperties(string provider, string loginCallbackUrl)
        {
            return _userService.BuildAuthenticationProperties(provider, loginCallbackUrl);
        }

        #endregion


        #region Private methods

        private async Task<TUser> LoginAsync(Credentials credentials)
        {
            var loginType = _credentialsBusiness.ValidateCredentials(credentials);

            switch (loginType)
            {
                case LoginType.USER:
                    return await _userService.GetByUsername(credentials.Username, credentials.Password);

                case LoginType.MAIL:
                    return await _userService.GetByEmail(credentials.Email, credentials.Password);

                default:
                    throw new AuthenticationException("User could not by logged in into the system");
            }
        }

        #endregion

    }



    #region Default Implementation

    public class SessionService : SessionService<User>
    {
        public SessionService(
                ISecuritySettings securitySettings,
                IUserService userService,
                ICredentialsBusiness credentialsBusiness,
                ITokenHelper tokenService,
                IMapper<User, Credentials> userMapper, IMapper<User, TokenInfo, Session> sessionMapper)
            : base(securitySettings, userService, credentialsBusiness, tokenService, userMapper, sessionMapper)
        { }
    }

    #endregion

}
