using ChustaSoft.Common.Contracts;
using ChustaSoft.Common.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChustaSoft.Tools.Authorization.Configuration
{
    public class DefaultUsersLoader<TUser, TRole> : IDefaultUsersLoader
        where TUser : User, new()
        where TRole : Role
    {

        private readonly IMapper<TUser, Credentials> _credentialsMapper;
        private readonly IUserService<TUser> _userService;
        private readonly IRoleService<TRole> _roleService;


        public ICollection<ErrorMessage> Errors { get; private set; }


        public DefaultUsersLoader(IUserService<TUser> userService, IRoleService<TRole> roleService, IMapper<TUser, Credentials> credentialsMapper)
        {
            Errors = new List<ErrorMessage>();

            _userService = userService;
            _roleService = roleService;
            _credentialsMapper = credentialsMapper;
        }



        public async Task<bool> PersistAsync(ICredentialsBuilder userBuilder)
        {
            var usersData = userBuilder.Build();

            await CheckRolesAsync(usersData);

            foreach (var credentialsTupple in usersData)
            {
                if (!await _userService.ExistAsync(credentialsTupple.Credentials.Email))
                {
                    var user = _credentialsMapper.MapToSource(credentialsTupple.Credentials);
                    var flag = await _userService.CreateAsync(user, credentialsTupple.Credentials.Password, credentialsTupple.Credentials.Parameters);

                    if (!flag)
                        Errors.Add(new ErrorMessage(Common.Enums.ErrorType.Unknown, $"User: {user.UserName} could not be created"));


                    if (credentialsTupple.Roles.Any())
                    {
                        var roleAssignationFlag = await _userService.AssignRoleAsync(user, credentialsTupple.Roles);

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

        public DefaultUsersLoader(IUserService userService, IRoleService roleService, IMapper<User, Credentials> userMapper)
            : base(userService, roleService, userMapper)
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
