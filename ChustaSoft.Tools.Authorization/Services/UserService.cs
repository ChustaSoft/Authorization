using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;


namespace ChustaSoft.Tools.Authorization
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
