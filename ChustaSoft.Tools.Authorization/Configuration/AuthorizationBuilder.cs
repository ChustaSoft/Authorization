using ChustaSoft.Tools.Authorization.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ChustaSoft.Tools.Authorization.Configuration
{

    public class AuthorizationBuilder<TUser, TRole> : IAuthorizationBuilder
        where TUser : User, new()
        where TRole : Role
    {

        public IServiceProvider ServiceProvider { get; set; }

        protected IUserBuilder _userBuilder;


        public AuthorizationBuilder(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;

            _userBuilder = serviceProvider.GetRequiredService<IUserBuilder>();
        }


        public IAuthorizationBuilder DefaultUsers(Action<IUserBuilder> userBuilderAction)
        {
            userBuilderAction.Invoke(_userBuilder);

            var resultFalg = _userBuilder.PersistUsersAsync().Result;

            if (!resultFalg)
                throw new AuthConfigurationException("Something went wrong loading default users to the system", _userBuilder.Errors);

            return this;
        }

    }



    #region Default Implementation

    public class AuthorizationBuilder : AuthorizationBuilder<User, Role>
    {
        public AuthorizationBuilder(IServiceProvider serviceProvider)
            : base(serviceProvider)
        { }
    }


    #endregion



    #region Contract

    public interface IAuthorizationBuilder
    {
        IServiceProvider ServiceProvider { get; set; }

        IAuthorizationBuilder DefaultUsers(Action<IUserBuilder> userBuilderAction);
    }

    #endregion

}
