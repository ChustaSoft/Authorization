using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
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

        public async Task<TUser> GetByUsername(string username, string password)
        {
            var userSignIn = await _signInManager.PasswordSignInAsync(username, password, isPersistent: false, lockoutOnFailure: false);

            if (userSignIn.Succeeded)
                return await _userManager.FindByNameAsync(username);
            else
                throw new AuthenticationException("User not allowed to login in the system");
        }

        public async Task<TUser> GetByEmail(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user != null)
            {
                var userSignIn = await _signInManager.PasswordSignInAsync(user.UserName, password, isPersistent: false, lockoutOnFailure: false);

                if (userSignIn.Succeeded)
                    return user;
            }

            throw new AuthenticationException("User not allowed to login in the system");
        }

        public async Task<bool> CreateAsync(TUser user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);

            return result.Succeeded;
        }

        public async Task<bool> AssignRoleAsync(TUser user, IEnumerable<string> roleNames)
        {
            var result = await _userManager.AddToRolesAsync(user, roleNames);

            return result.Succeeded;
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
