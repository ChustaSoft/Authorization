using ChustaSoft.Tools.Authorization.Models;
using System;
using System.Threading.Tasks;


namespace ChustaSoft.Tools.Authorization
{

    /// <summary>
    /// Service for managing roles
    /// </summary>
    /// <typeparam name="TRole">Constrained on supertype internal Role</typeparam>
    public interface IRoleService<TRole>
         where TRole : Role
    {

        /// <summary>
        /// Get rol by Id
        /// </summary>
        /// <param name="roleId">The Role PK</param>
        /// <returns>Role if found</returns>
        Task<TRole> Get(Guid roleId);

        /// <summary>
        /// Check if the role exists in the DB
        /// </summary>
        /// <param name="roleName">Role name to check</param>
        /// <returns>True if exists, false otherwise</returns>
        Task<bool> ExistAsync(string roleName);

        /// <summary>
        /// Save role functionality
        /// </summary>
        /// <param name="roleName">Role name to persist</param>
        /// <returns>True if succeed, false otherwise</returns>
        Task<bool> SaveAsync(string roleName);

    }


    /// <summary>
    /// Contract by default if base Role is used
    /// </summary>
    public interface IRoleService : IRoleService<Role> { }

}
