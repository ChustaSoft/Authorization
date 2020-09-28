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

        private readonly AuthorizationSettings _authorizationSettings;

        private readonly IUserService<TUser> _userService;
        private readonly IRoleService<TRole> _roleService;


        public ICollection<ErrorMessage> Errors { get; private set; }


        public DefaultUsersLoader(AuthorizationSettings authorizationSettings, IUserService<TUser> userService, IRoleService<TRole> roleService)
        {
            Errors = new List<ErrorMessage>();

            _authorizationSettings = authorizationSettings;

            _userService = userService;
            _roleService = roleService;
        }



        public async Task<bool> PersistAsync(ICredentialsBuilder userBuilder)
        {
            var usersData = userBuilder.Build();

            await CheckRolesAsync(usersData);

            foreach (var credentialsTupple in usersData)
            {
                if (!await _userService.ExistAsync(credentialsTupple.Credentials.Email))
                {
                    var user = credentialsTupple.Credentials.ToUser<TUser>(_authorizationSettings.DefaultCulture);
                    var flag = await _userService.CreateAsync(user, credentialsTupple.Credentials.Password, credentialsTupple.Credentials.Parameters);

                    if (!flag)
                        Errors.Add(new ErrorMessage(Common.Enums.ErrorType.Unknown, $"User: {user.UserName} could not be created"));


                    if (credentialsTupple.Roles.Any())
                    {
                        var roleAssignationFlag = await _userService.AssignRolesAsync(user, credentialsTupple.Roles);

                        if (!roleAssignationFlag)
                            Errors.Add(new ErrorMessage(Common.Enums.ErrorType.Invalid, $"Roles could not be assigned to User: {user.UserName}"));
                    }
                }
            }

            return !Errors.Any();
        }


        private async Task CheckRolesAsync(IEnumerable<(Credentials Credentials, IEnumerable<string> Roles)> usersData)
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

        public DefaultUsersLoader(AuthorizationSettings authorizationSettings, IUserService userService, IRoleService roleService)
            : base(authorizationSettings, userService, roleService)
        { }

    }

    #endregion



    #region Contract

    public interface IDefaultUsersLoader
    {

        ICollection<ErrorMessage> Errors { get; }

        Task<bool> PersistAsync(ICredentialsBuilder userBuilder);

    }

    #endregion

}
