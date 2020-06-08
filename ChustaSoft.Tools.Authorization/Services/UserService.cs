using Microsoft.AspNetCore.Identity;
using System;
using System.Security.Authentication;
using System.Threading.Tasks;

namespace ChustaSoft.Tools.Authorization
{
    public class UserService<TUser> : IUserService<TUser>
         where TUser : User, new()
    {

        #region Fields

        private readonly SignInManager<TUser> _signInManager;
        private readonly UserManager<TUser> _userManager;

        #endregion


        #region Constructor

        public UserService(SignInManager<TUser> signInManager, UserManager<TUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        #endregion


        #region Public methods

        public async Task<TUser> GetAsync(Guid userId)
        {
            return await _userManager.FindByIdAsync(userId.ToString());
        }

        public async Task<TUser> LoginAsync(Credentials credentials, LoginType loginType) 
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

        public async Task<bool> CreateAsync(TUser user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);

            return result.Succeeded;
        }

        #endregion


        #region Private methods

        private async Task<TUser> LoginByUsername(Credentials credentials)
        {
            var userSignIn = await _signInManager.PasswordSignInAsync(credentials.Username, credentials.Password, isPersistent: false, lockoutOnFailure: false);

            if (userSignIn.Succeeded)
                return await _userManager.FindByNameAsync(credentials.Username);
            else
                throw new AuthenticationException("User not allowed to login in the system");
        }

        private async Task<TUser> LoginByEmail(Credentials credentials)
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



    #region Default Implementation

    public class UserService : UserService<User>, IUserService
    {
        public UserService(SignInManager<User> signInManager, UserManager<User> userManager)
            : base(signInManager, userManager)
        { }
    }

    #endregion

}
