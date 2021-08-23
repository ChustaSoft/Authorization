using ChustaSoft.Tools.Authorization.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ChustaSoft.Tools.Authorization
{
    public static class ConfigurationHelper
    {

        private const string AUTH_SETINGS_SECTION = "Authorization";
        
        
        public static IdentityBuilder RegisterAuthorization(this IServiceCollection services, IConfiguration configuration, string privateKey, string authSectionName = AUTH_SETINGS_SECTION)
        {
            var authSettings = GetSettingsFromConfiguration(configuration, authSectionName);

            services.SetupJwtServices<User>();

            return services.RegisterAuthorizationServices<User, Role>(privateKey, authSettings);
        }

        public static IdentityBuilder RegisterAuthorization<TUser, TRole>(this IServiceCollection services, IConfiguration configuration, string privateKey, string authSectionName = AUTH_SETINGS_SECTION)
            where TUser : User, new()
            where TRole : Role, new()
        {
            var authSettings = GetSettingsFromConfiguration(configuration, authSectionName);

            services.SetupJwtServices<TUser>();

            return services.RegisterAuthorizationServices<TUser, TRole>(privateKey, authSettings);
        }

        public static IdentityBuilder RegisterAuthorization(this IServiceCollection services, string privateKey, Action<IAuthorizationSettingsBuilder> settingsBuildingAction)
        {
            var authSettings = GetSettingsFromBuilder(settingsBuildingAction);

            services.SetupJwtServices<User>();

            return services.RegisterAuthorizationServices<User, Role>(privateKey, authSettings);
        }

        public static IdentityBuilder RegisterAuthorization<TUser, TRole>(this IServiceCollection services, string privateKey, Action<IAuthorizationSettingsBuilder> settingsBuildingAction)
            where TUser : User, new()
            where TRole : Role, new()
        {
            var authSettings = GetSettingsFromBuilder(settingsBuildingAction);

            services.SetupJwtServices<TUser>();

            return services.RegisterAuthorizationServices<TUser, TRole>(privateKey, authSettings);
        }


        private static AuthorizationSettings GetSettingsFromBuilder(Action<IAuthorizationSettingsBuilder> settingsBuildingAction)
        {
            var settingsBuilder = new AuthorizationSettingsBuilder();

            settingsBuildingAction.Invoke(settingsBuilder);

            var authSettings = settingsBuilder.Build();
            return authSettings;
        }

        private static AuthorizationSettings GetSettingsFromConfiguration(IConfiguration configuration, string authSectionName)
        {
            var authSettings = configuration.GetSection(authSectionName).Get<AuthorizationSettings>();

            if (authSettings == null)
                authSettings = new AuthorizationSettings();

            return authSettings;
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
