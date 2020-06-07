using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;


namespace ChustaSoft.Tools.Authorization
{
    public class UserService<TUser> : IUserService<TUser>
         where TUser : User, new()
    {

        #region Fields

        private readonly UserManager<TUser> _userManager;

        #endregion


        #region Constructor

        public UserService(UserManager<TUser> userManager)
        {
            _userManager = userManager;
        }

        #endregion


        #region Public methods

        public async Task<TUser> GetAsync(Guid userId)
        {
            return await _userManager.FindByIdAsync(userId.ToString());
        }

        #endregion

    }

    public class UserService : UserService<User>, IUserService
    {
        public UserService(UserManager<User> userManager)
            : base(userManager)
        { }
    }
}
