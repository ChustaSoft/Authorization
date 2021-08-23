using ChustaSoft.Tools.Authorization.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ChustaSoft.Tools.Authorization
{
    public static class ConfigurationHelper
    {

        public static IdentityBuilder RegisterAuthorization(this IServiceCollection services, IConfiguration configuration, string privateKey, string authSectionName = AuthorizationConstants.AUTH_SETINGS_SECTION)
        {
            var authSettings = AuthorizationSettings.GetFromFile(configuration, authSectionName);

            services.SetupJwtServices<User>();

            return services.RegisterAuthorizationServices<User, Role>(privateKey, authSettings);
        }

        public static IdentityBuilder RegisterAuthorization<TUser, TRole>(this IServiceCollection services, IConfiguration configuration, string privateKey, string authSectionName = AuthorizationConstants.AUTH_SETINGS_SECTION)
            where TUser : User, new()
            where TRole : Role, new()
        {
            var authSettings = AuthorizationSettings.GetFromFile(configuration, authSectionName);

            services.SetupJwtServices<TUser>();

            return services.RegisterAuthorizationServices<TUser, TRole>(privateKey, authSettings);
        }

        public static IdentityBuilder RegisterAuthorization(this IServiceCollection services, string privateKey, Action<AuthorizationSettingsBuilder<AuthorizationSettings>> settingsBuildingAction)
        {
            var authSettings = AuthorizationSettings.GetFromBuilder(settingsBuildingAction);

            services.SetupJwtServices<User>();

            return services.RegisterAuthorizationServices<User, Role>(privateKey, authSettings);
        }

        public static IdentityBuilder RegisterAuthorization<TUser, TRole>(this IServiceCollection services, string privateKey, Action<AuthorizationSettingsBuilder<AuthorizationSettings>> settingsBuildingAction)
            where TUser : User, new()
            where TRole : Role, new()
        {
            var authSettings = AuthorizationSettings.GetFromBuilder(settingsBuildingAction);

            services.SetupJwtServices<TUser>();

            return services.RegisterAuthorizationServices<TUser, TRole>(privateKey, authSettings);
        }
       

        private static void SetupJwtServices<TUser>(this IServiceCollection services)
            where TUser : User, new()
        {
            if (typeof(TUser) == typeof(User))
            {
                services.AddTransient<ISessionService, SessionService>();
                
                services.AddTransient<ITokenHelper, TokenHelper>();
            }
            else
            {
                services.AddTransient<ISessionService, SessionService<TUser>>();
                
                services.AddTransient<ITokenHelper<TUser>, TokenHelper<TUser>>();
            }
        }

    }
}
