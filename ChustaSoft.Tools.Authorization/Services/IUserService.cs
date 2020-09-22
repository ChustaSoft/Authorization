using ChustaSoft.Tools.Authorization.Models;
using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChustaSoft.Tools.Authorization
{
    public interface IUserService<TUser> : IUserRoleService<TUser>, IUserClaimService<TUser>
         where TUser : User, new()
    {

        Task<TUser> GetAsync(Guid userId);

        Task<TUser> GetByUsername(string username, string password);

        Task<TUser> GetByEmail(string email, string password);

        Task<bool> CreateAsync(TUser user, string password, IDictionary<string, string> parameters);

        Task<bool> ExistAsync(string userEmail);

        AuthenticationProperties GetExternalProperties(string provider, string loginCallbackUrl);

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
