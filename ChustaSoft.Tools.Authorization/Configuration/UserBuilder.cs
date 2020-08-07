using ChustaSoft.Common.Contracts;
using ChustaSoft.Common.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChustaSoft.Tools.Authorization.Configuration
{
    public class UserBuilder<TUser, TRole> : IUserBuilder
        where TUser : User, new()
        where TRole : Role
    {

        private readonly ICollection<(Credentials Credentials, IEnumerable<string> Roles)> _usersCredentials;

        private readonly IMapper<TUser, Credentials> _credentialsMapper;
        private readonly IUserService<TUser> _userService;
        private readonly IRoleService<TRole> _roleService;

        private Credentials _credentials;
        private ICollection<string> _rolesAssigned;

        public ICollection<ErrorMessage> Errors { get; set; }


        public UserBuilder(IUserService<TUser> userService, IRoleService<TRole> roleService, IMapper<TUser, Credentials> credentialsMapper)
        {
            _usersCredentials = new List<(Credentials, IEnumerable<string>)>();

            _userService = userService;
            _roleService = roleService;
            _credentialsMapper = credentialsMapper;
        }


        public IUserBuilder AddCredentials(string userName, string userPassword)
        {
            CheckIfExistingCredentials();

            _credentials = new Credentials { Username = userName, Password = userPassword, Email = $"{userName}@noreply.com" };
            _rolesAssigned = _rolesAssigned = new List<string>();

            return this;
        }

        public IUserBuilder WithEmail(string email)
        {
            _credentials.Email = email;

            return this;
        }

        public IUserBuilder WithRole(string roleName)
        {
            _rolesAssigned.Add(roleName);

            return this;
        }

        public async Task<bool> PersistUsersAsync()
        {
            //TODO: Manage errors inside the collection (flag and roleAssignationFlag)

            CheckIfExistingCredentials();
            await CheckRolesAsync();

            foreach (var credentialsTupple in _usersCredentials)
            {
                if (!await _userService.ExistAsync(credentialsTupple.Credentials.Email)) 
                {
                    var user = _credentialsMapper.MapToSource(credentialsTupple.Credentials);
                    var flag = await _userService.CreateAsync(user, credentialsTupple.Credentials.Password, credentialsTupple.Credentials.Parameters);

                    if (credentialsTupple.Roles.Any())
                    {
                        var roleAssignationFlag = await _userService.AssignRoleAsync(user, credentialsTupple.Roles);
                    }
                }
            }

            return true;
        }


        private void CheckIfExistingCredentials()
        {
            if (_credentials != null)
                _usersCredentials.Add((_credentials, _rolesAssigned));
        }

        private async Task CheckRolesAsync()
        {
            var allRoles = _usersCredentials.SelectMany(x => x.Roles).ToList();
            if (allRoles.Any())
                foreach (var role in allRoles)
                    if (!await _roleService.ExistAsync(role))
                        await _roleService.SaveAsync(role);
        }

    }



    #region Default Implementation

    public class UserBuilder : UserBuilder<User, Role>
    {
        public UserBuilder(IUserService userService, IRoleService roleService, IMapper<User, Credentials> userMapper)
            : base(userService, roleService, userMapper)
        { }
    }

    #endregion



    #region Contract

    public interface IUserBuilder
    {

        IUserBuilder AddCredentials(string userName, string userPassword);

        IUserBuilder WithRole(string roleName);

        Task<bool> PersistUsersAsync();

    }

    #endregion

}
