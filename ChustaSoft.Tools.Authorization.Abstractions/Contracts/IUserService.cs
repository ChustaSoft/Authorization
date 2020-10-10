using ChustaSoft.Tools.Authorization.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChustaSoft.Tools.Authorization
{
    public interface IUserService<TUser> : IUserRoleService<TUser>, IUserClaimService<TUser>
         where TUser : User, new()
    {

        Task<TUser> GetAsync(Guid userId);

        Task<TUser> GetAsync(string username, string password);

        Task<TUser> SignByUsername(string username, string password);

        Task<TUser> SignByEmail(string email, string password);

        Task<TUser> SignByPhone(string phone, string password);

        Task<TUser> ConfirmEmail(string email, string token);

        Task<TUser> ConfirmPhone(string phone, string token);

        Task<bool> CreateAsync(TUser user, string password, IDictionary<string, string> parameters);

        Task<bool> UpdateAsync(TUser user);

        Task<bool> ExistAsync(string userEmail);

    }


    public interface IUserRoleService<TUser>
         where TUser : User, new()
    {
        Task<bool> AssignRoleAsync(Guid userId, string roleName);

        Task<bool> AssignRoleAsync(TUser user, string roleName);

        Task<bool> AssignRolesAsync(TUser user, IEnumerable<string> roleNames);
    }


    public interface IUserClaimService<TUser>
         where TUser : User, new()
    {
        Task<bool> AssignClaimAsync(Guid userId, string claimName);

        Task<bool> AssignClaimAsync(TUser user, string claimName);

        Task<bool> AssignClaimsAsync(TUser user, IEnumerable<string> claimNames);
    }



    #region Default Contracts

    public interface IUserService : IUserService<User> { }

    public interface IUserRoleService : IUserRoleService<User> { }

    public interface IUserClaimService : IUserClaimService<User> { }


    #endregion

}
