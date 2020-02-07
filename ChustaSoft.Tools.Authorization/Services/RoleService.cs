using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;


namespace ChustaSoft.Tools.Authorization
{
    public class RoleService : IRoleService
    {

        #region Fields

        private readonly RoleManager<Role> _roleManager;

        #endregion


        #region Constructor

        public RoleService(RoleManager<Role> roleManager)
        {
            _roleManager = roleManager;
        }

        #endregion


        #region Public methods

        public Task<Role> Get(Guid roleId)
        {
            var role = _roleManager.FindByIdAsync(roleId.ToString());

            return role;
        }

        #endregion

    }
}
