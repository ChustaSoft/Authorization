using System;
using System.Threading.Tasks;
using ChustaSoft.Tools.Authorization.Models;
using Microsoft.AspNetCore.Identity;


namespace ChustaSoft.Tools.Authorization.Services
{
    public class UserService : IUserService
    {

        #region Fields

        private readonly UserManager<User> _userManager;

        #endregion


        #region Constructor

        public UserService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        #endregion


        #region Public methods

        public async Task<User> GetAsync(Guid userId)
        {
            return await _userManager.FindByIdAsync(userId.ToString());
        }

        #endregion

    }
}
