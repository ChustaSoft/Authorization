using ChustaSoft.Tools.Authorization.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace ChustaSoft.Tools.Authorization
{

    public class OAuthProviderAuthorizationBuilder<TUser, TRole> : IOAuthProviderAuthorizationBuilder
        where TUser : User, new()
        where TRole : Role
    {
        
        public IIdentityServerBuilder ProviderBuilder { private set; get; }
        public IdentityBuilder IdentityBuilder { private set; get; }
        
        
        public OAuthProviderAuthorizationBuilder(IIdentityServerBuilder providerBuilder, IdentityBuilder identityBuilder) 
        {
            ProviderBuilder = providerBuilder;
            IdentityBuilder = identityBuilder;
        }

    }



    #region Default Implementation

    public class OAuthProviderAuthorizationBuilder : OAuthProviderAuthorizationBuilder<User, Role>, IOAuthProviderAuthorizationBuilder
    {
        public OAuthProviderAuthorizationBuilder(IIdentityServerBuilder providerBuilder, IdentityBuilder identityBuilder)
            : base(providerBuilder, identityBuilder)
        { }
    }


    #endregion



    #region Contract

    public interface IOAuthProviderAuthorizationBuilder
    {

        public IIdentityServerBuilder ProviderBuilder { get; }
        public IdentityBuilder IdentityBuilder { get; }

    }

    #endregion

}
