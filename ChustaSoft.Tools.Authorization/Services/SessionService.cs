using ChustaSoft.Common.Contracts;
using System.Security.Authentication;
using System.Threading.Tasks;

namespace ChustaSoft.Tools.Authorization
{
    public class SessionService<TUser> : ISessionService
         where TUser : User, new()
    {

        #region Fields

        private readonly IUserService<TUser> _userService;

        private readonly ICredentialsBusiness _credentialsBusiness;

        private readonly ITokenHelper<TUser> _tokenHelper;

        private readonly IMapper<TUser, Credentials> _userMapper;
        private readonly IMapper<TUser, TokenInfo, Session> _sessionMapper;

        #endregion


        #region Constructor

        public SessionService(
            IUserService<TUser> userService,
            ICredentialsBusiness credentialsBusiness, 
            ITokenHelper<TUser> tokenService, 
            IMapper<TUser, Credentials> userMapper, IMapper<TUser, TokenInfo, Session> sessionMapper)
        {
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
            var loginType = _credentialsBusiness.ValidateCredentials(credentials);
            var user = await _userService.LoginAsync(credentials, loginType);
            var tokenInfo = _tokenHelper.Generate(user);
            var session = _sessionMapper.MapFromSource(user, tokenInfo);

            return session;
        }

        public async Task<Session> RegisterAsync(Credentials credentials)
        {
            var user = _userMapper.MapToSource(credentials);
            var resultFlag = await _userService.CreateAsync(user, credentials.Password);

            if (resultFlag)
            {
                var tokenInfo = _tokenHelper.Generate(user);
                var session = _sessionMapper.MapFromSource(user, tokenInfo);

                return session;
            }
            else
                throw new AuthenticationException($"User {user.UserName} could not be created");
        }

        #endregion

    }



    #region Default Implementation

    public class SessionService : SessionService<User>
    {
        public SessionService(
               IUserService userService,
               ICredentialsBusiness credentialsBusiness,
               ITokenHelper tokenService,
               IMapper<User, Credentials> userMapper, IMapper<User, TokenInfo, Session> sessionMapper)
            : base(userService, credentialsBusiness, tokenService, userMapper, sessionMapper)
        { }
    }

    #endregion

}
