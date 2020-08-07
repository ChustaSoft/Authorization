using Microsoft.Extensions.DependencyInjection;
using System;

namespace ChustaSoft.Tools.Authorization
{

    public class AuthorizationBuilder<TUser, TRole> : IAuthorizationBuilder
        where TUser : User, new()
        where TRole : Role
    {

        public IServiceProvider ServiceProvider { get; set; }

        protected IDefaultUsersLoader _defaultUsersLoader;


        public AuthorizationBuilder(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;

            _defaultUsersLoader = serviceProvider.GetRequiredService<IDefaultUsersLoader>();
        }


        public IAuthorizationBuilder DefaultUsers(Action<ICredentialsBuilder> userBuilderAction)
        {
            var credentialsBuilder = new CredentialsBuilder();

            userBuilderAction.Invoke(credentialsBuilder);

            var resultFalg = _defaultUsersLoader.PersistAsync(credentialsBuilder).Result;

            if (!resultFalg)
                throw new AuthConfigurationException("Something went wrong loading default users to the system", _defaultUsersLoader.Errors);

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

        IAuthorizationBuilder DefaultUsers(Action<ICredentialsBuilder> userBuilderAction);
    }

    #endregion

}
