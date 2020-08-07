﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ChustaSoft.Tools.Authorization
{
    public class UserService<TUser> : IUserService<TUser>
         where TUser : User, new()
    {

        #region Fields

        private readonly SignInManager<TUser> _signInManager;
        private readonly UserManager<TUser> _userManager;

        private readonly IAfterUserCreationAction _afterUserCreationAction;

        #endregion


        #region Constructor

        public UserService(SignInManager<TUser> signInManager, UserManager<TUser> userManager, IAfterUserCreationAction afterUserCreationAction)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _afterUserCreationAction = afterUserCreationAction;
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

        public async Task<bool> CreateAsync(TUser user, string password, IDictionary<string, string> parameters)
        {
            var result = await _userManager.CreateAsync(user, password);
            var customResult = await _afterUserCreationAction.DoAfter(user.Id, parameters);

            return result.Succeeded && customResult;
        }

        public async Task<bool> ExistAsync(string userEmail)
        {
            var result = await _userManager.FindByEmailAsync(userEmail);

            return result != null;
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

        #endregion

    }



    #region Default Implementation

    public class UserService : UserService<User>, IUserService, IUserRoleService, IUserClaimService
    {
        public UserService(SignInManager<User> signInManager, UserManager<User> userManager, IAfterUserCreationAction afterUserCreationAction)
            : base(signInManager, userManager, afterUserCreationAction)
        { }
    }

    #endregion

}
