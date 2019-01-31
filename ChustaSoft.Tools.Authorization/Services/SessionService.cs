using ChustaSoft.Common.Helpers;
using ChustaSoft.Tools.Authorization.Enums;
using ChustaSoft.Tools.Authorization.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Authentication;
using System.Threading.Tasks;


namespace ChustaSoft.Tools.Authorization.Services
{
    public class SessionService : ISessionService
    {

        #region Fields

        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        private readonly ICredentialsService _credentialsBusiness;
        private readonly ITokenService _tokenService;

        private readonly IMapper<User, Credentials> _userMapper;
        private readonly IMapper<User, TokenInfo, Session> _sessionMapper;

        #endregion


        #region Constructor

        public SessionService(UserManager<User> userManager, SignInManager<User> signInManager, ICredentialsService credentialsBusiness, ITokenService tokenService, 
            IMapper<User, Credentials> userMapper, IMapper<User, TokenInfo, Session> sessionMapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;

            _credentialsBusiness = credentialsBusiness;
            _tokenService = tokenService;

            _userMapper = userMapper;
            _sessionMapper = sessionMapper;
        }

        #endregion


        #region Public methods

        public async Task<Session> AuthenticateAsync(Credentials credentials)
        {
            var loginType = _credentialsBusiness.ValidateCredentials(credentials);
            var user = await TryLoginUser(credentials, loginType);
            var tokenInfo = _tokenService.Generate(user);
            var session = _sessionMapper.MapFromSource(user, tokenInfo);

            return session;
        }

        public async Task<Session> RegisterAsync(Credentials credentials)
        {
            var user = _userMapper.MapToSource(credentials);
            var result = await _userManager.CreateAsync(user, credentials.Password);

            if (result.Succeeded)
            {
                var tokenInfo = _tokenService.Generate(user);
                var session = _sessionMapper.MapFromSource(user, tokenInfo);

                return session;
            }
            else
                throw new AuthenticationException($"User {user.UserName} could not be created");
        }

        #endregion


        #region Private methods

        private async Task<User> TryLoginUser(Credentials credentials, LoginType loginType)
        {
            switch (loginType)
            {
                case LoginType.USER:
                    return await LoginByUsername(credentials);

                case LoginType.MAIL:
                    return await LoginByEmail(credentials);

                default:
                    throw new AuthenticationException("User could not by logged in into the system");
            }
        }

        private async Task<User> LoginByUsername(Credentials credentials)
        {
            var userSignIn = await _signInManager.PasswordSignInAsync(credentials.Username, credentials.Password, isPersistent: false, lockoutOnFailure: false);

            if (userSignIn.Succeeded)
                return await _userManager.FindByNameAsync(credentials.Username);
            else
                throw new AuthenticationException();
        }

        private async Task<User> LoginByEmail(Credentials credentials)
        {
            var user = await _userManager.FindByEmailAsync(credentials.Email);

            if (user != null)
            {
                var userSignIn = await _signInManager.PasswordSignInAsync(user.UserName, credentials.Password, isPersistent: false, lockoutOnFailure: false);

                if (userSignIn.Succeeded)
                    return user;
            }

            throw new AuthenticationException();
        }

        #endregion

    }
}
