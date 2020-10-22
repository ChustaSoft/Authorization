using ChustaSoft.Common.Utilities;
using ChustaSoft.Tools.Authorization.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChustaSoft.Tools.Authorization
{
    public class DefaultUsersLoader<TUser, TRole> : IDefaultUsersLoader
        where TUser : User, new()
        where TRole : Role
    {

        private readonly IUserService<TUser> _userService;
        private readonly IRoleService<TRole> _roleService;


        public ICollection<ErrorMessage> Errors { get; private set; }


        public DefaultUsersLoader(IUserService<TUser> userService, IRoleService<TRole> roleService)
        {
            Errors = new List<ErrorMessage>();

            _userService = userService;
            _roleService = roleService;
        }



        public async Task<bool> PersistAsync(IAutomaticCredentialsBuilder credentialsBuilder)
        {
            var usersData = credentialsBuilder.Build();

            await CheckRolesAsync(usersData);

            foreach (var credentialsTupple in usersData)
            {
                if (!await _userService.ExistAsync(credentialsTupple.Credentials.Email))
                {
                    var user = credentialsTupple.Credentials.ToUser<TUser>();
                    var flag = await _userService.CreateAsync(user, credentialsTupple.Credentials.Password, credentialsTupple.Credentials.Parameters);

                    await TryAllowFullAccess(credentialsTupple, user);
                    CheckUserCreation(user, flag);
                    await AssignRoles(credentialsTupple, user);
                }
            }

            return !Errors.Any();
        }


        private async Task AssignRoles((AutomaticCredentials Credentials, IEnumerable<string> Roles) credentialsTupple, TUser user)
        {
            if (credentialsTupple.Roles.Any())
            {
                var roleAssignationFlag = await _userService.AssignRolesAsync(user, credentialsTupple.Roles);

                if (!roleAssignationFlag)
                    Errors.Add(new ErrorMessage(Common.Enums.ErrorType.Invalid, $"Roles could not be assigned to User: {user.UserName}"));
            }
        }

        private void CheckUserCreation(TUser user, bool flag)
        {
            if (!flag)
                Errors.Add(new ErrorMessage(Common.Enums.ErrorType.Unknown, $"User: {user.UserName} could not be created"));
        }

        private async Task TryAllowFullAccess((AutomaticCredentials Credentials, IEnumerable<string> Roles) credentialsTupple, TUser user)
        {
            if (credentialsTupple.Credentials.FullAccess)
                await _userService.UpdateAsync(user.WithFullAccess());
        }

        private async Task CheckRolesAsync(IEnumerable<(AutomaticCredentials Credentials, IEnumerable<string> Roles)> usersData)
        {
            var allRoles = usersData.SelectMany(x => x.Roles).ToList();
            if (allRoles.Any())
                foreach (var role in allRoles)
                    if (!await _roleService.ExistAsync(role))
                        await _roleService.SaveAsync(role);
        }

    }



    #region Default Implementation

    public class DefaultUsersLoader : DefaultUsersLoader<User, Role>
    {

        public DefaultUsersLoader(IUserService userService, IRoleService roleService)
            : base(userService, roleService)
        { }

    }

    #endregion



    #region Contract

    public interface IDefaultUsersLoader
    {

        ICollection<ErrorMessage> Errors { get; }

        Task<bool> PersistAsync(IAutomaticCredentialsBuilder userBuilder);

    }

    #endregion

}
