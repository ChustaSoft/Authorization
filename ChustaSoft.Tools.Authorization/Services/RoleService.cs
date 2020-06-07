using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;


namespace ChustaSoft.Tools.Authorization
{
    public class RoleService<TRole> : IRoleService<TRole>
         where TRole : Role
    {

        #region Fields

        private readonly RoleManager<TRole> _roleManager;

        #endregion


        #region Constructor

        public RoleService(RoleManager<TRole> roleManager)
        {
            _roleManager = roleManager;
        }

        #endregion


        #region Public methods

        public Task<TRole> Get(Guid roleId)
        {
            var role = _roleManager.FindByIdAsync(roleId.ToString());

            return role;
        }

        #endregion

    }


    public class RoleService : RoleService<Role>, IRoleService
    {
        public RoleService(RoleManager<Role> roleManager)
            :base(roleManager)
        { }

    }
}
