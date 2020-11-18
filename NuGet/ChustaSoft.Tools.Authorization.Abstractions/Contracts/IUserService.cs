﻿using ChustaSoft.Tools.Authorization.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChustaSoft.Tools.Authorization
{

    /// <summary>
    /// Service for managing users
    /// </summary>
    /// <typeparam name="TUser">Constrained on supertype internal User</typeparam>
    public interface IUserService<TUser> : IUserRoleService<TUser>, IUserClaimService<TUser>
         where TUser : User, new()
    {

        /// <summary>
        /// Get User by Id
        /// </summary>
        /// <param name="userId">User PK</param>
        /// <returns>User retrived</returns>
        Task<TUser> GetAsync(Guid userId);

        /// <summary>
        /// Get User by username and password
        /// </summary>
        /// <param name="username">Unique username</param>
        /// <param name="password">Password for uesr</param>
        /// <returns>User retrived if succeed</returns>
        Task<TUser> GetAsync(string username, string password);

        /// <summary>
        /// Sign user by username. Active user is taken into account
        /// </summary>
        /// <param name="username">Unique username</param>
        /// <param name="password">Password for user</param>
        /// <returns>User retrived if succeed</returns>
        Task<TUser> SignByUsername(string username, string password);

        /// <summary>
        /// Sign user by email. Active user is taken into account
        /// </summary>
        /// <param name="email">Unique email</param>
        /// <param name="password">Password for user</param>
        /// <returns>User retrived if succeed</returns>
        Task<TUser> SignByEmail(string email, string password);

        /// <summary>
        /// Sign user by phone. Active user is taken into account
        /// </summary>
        /// <param name="phone">Unique phone</param>
        /// <param name="password">Password for user</param>
        /// <returns>User retrived if succeed</returns>
        Task<TUser> SignByPhone(string phone, string password);

        /// <summary>
        /// Confirm email for user
        /// </summary>
        /// <param name="email">User email</param>
        /// <param name="token">Generated confirmation email token</param>
        /// <returns>User retrived if succeed</returns>
        Task<TUser> ConfirmEmail(string email, string token);

        /// <summary>
        /// Confirm phone for user
        /// </summary>
        /// <param name="phone">User phone</param>
        /// <param name="token">Generated confirmation phone token</param>
        /// <returns>User retrived if succeed</returns>
        Task<TUser> ConfirmPhone(string phone, string token);

        /// <summary>
        /// Create user, parameters are allowed that can be re-thrown to Client API.
        /// For cofirmation email and token, those parameters will be also both sent back to UI and Client API in IUserCreated if implemented
        /// </summary>
        /// <param name="user">User object to be created</param>
        /// <param name="password">Password for user</param>
        /// <param name="parameters">Parameters dictionary, that will be re-thrown to Clien API.</param>
        /// <returns>Result flag</returns>
        Task<bool> CreateAsync(TUser user, string password, IDictionary<string, string> parameters);

        /// <summary>
        /// Update user action
        /// </summary>
        /// <param name="user">User to be updated</param>
        /// <returns>Result flag</returns>
        Task<bool> UpdateAsync(TUser user);

        /// <summary>
        /// Check if a user already exists by email
        /// </summary>
        /// <param name="userEmail">email to be checked</param>
        /// <returns>True if already exists, false otherwise</returns>
        Task<bool> ExistAsync(string userEmail);

        /// <summary>
        /// Creates an external user in db, assigning a role if required, and storing the userLogin information in database
        /// </summary>
        /// <param name="defaultRole">Role to assign</param>
        /// <returns></returns>
        Task CreateExternalAsync(string defaultRole);       

        /// <summary>
        /// Perform the login of an external user
        /// </summary>
        /// <param name="isPersistent">Flag indicating whether the sign-in cookie should persist after the browser is closed</param>
        /// <returns></returns>
        Task<SignInResult> ExternalSignInAsync(bool isPersistent);


    }


    /// <summary>
    /// Service for managing role assignations to user
    /// </summary>
    /// <typeparam name="TUser">Constrained on supertype internal User</typeparam>
    public interface IUserRoleService<TUser>
         where TUser : User, new()
    {

        /// <summary>
        /// Assign role to a user by the UserId
        /// </summary>
        /// <param name="userId">User PK</param>
        /// <param name="roleName">Role name to be assigned</param>
        /// <returns>Result flag</returns>
        Task<bool> AssignRoleAsync(Guid userId, string roleName);

        /// <summary>
        /// Assign role to a user
        /// </summary>
        /// <param name="user">User to add the role</param>
        /// <param name="roleName">Role name to be assigned</param>
        /// <returns>Result flag</returns>
        Task<bool> AssignRoleAsync(TUser user, string roleName);

        /// <summary>
        /// Assign multiple roles to a single user
        /// </summary>
        /// <param name="user">User to add the role</param>
        /// <param name="roleNames">Roles to be assigned</param>
        /// <returns>Result flag</returns>
        Task<bool> AssignRolesAsync(TUser user, IEnumerable<string> roleNames);

    }



    public interface IUserClaimService<TUser>
         where TUser : User, new()
    {

        /// <summary>
        /// Assign claim to a user by UserId
        /// </summary>
        /// <param name="userId">User PK</param>
        /// <param name="claimName">Claim to be assign</param>
        /// <returns>Result flag</returns>
        Task<bool> AssignClaimAsync(Guid userId, string claimName);

        /// <summary>
        /// Assign claim to a user
        /// </summary>
        /// <param name="user">User to assign claim</param>
        /// <param name="claimName">Claim to be assign</param>
        /// <returns>Result flag</returns>
        Task<bool> AssignClaimAsync(TUser user, string claimName);

        /// <summary>
        /// Assign multiple claims to a user
        /// </summary>
        /// <param name="user">User to assign claims</param>
        /// <param name="claimNames">Claims to be assign</param>
        /// <returns>Result flag</returns>
        Task<bool> AssignClaimsAsync(TUser user, IEnumerable<string> claimNames);

    }



    #region Default Contracts

    /// <summary>
    /// Contract by default if base Role is used
    /// </summary>
    public interface IUserService : IUserService<User> { }

    /// <summary>
    /// Contract by default for roles if base Role is used
    /// </summary>
    public interface IUserRoleService : IUserRoleService<User> { }

    /// <summary>
    /// Contract by default for claims if base Role is used
    /// </summary>
    public interface IUserClaimService : IUserClaimService<User> { }


    #endregion

}
