using ChustaSoft.Common.Contracts;
using ChustaSoft.Tools.Authorization.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using System;

namespace ChustaSoft.Tools.Authorization
{
    public static class ConfigurationHelper
    {

        #region Constants

        private const string AUTH_SETINGS_SECTION = "AuthorizationSettings";

        #endregion


        #region Public Extension methods

        public static IdentityBuilder RegisterAuthorization<TUser, TRole>(this IServiceCollection services, IConfiguration configuration)
            where TUser : User, new()
            where TRole : Role, new()
        {
            var authSettings = GetFromSettingsOrDefault(configuration);

            services.AddSingleton(authSettings);
            services.AddTransient<ICredentialsBusiness, CredentialsBusiness>();

            SetDefaultCustomActions(services);

            SetupJwtAuthentication(services, configuration, authSettings);
            SetupTypedServices<TUser, TRole>(services);
            SetupUserTypedServices<TUser>(services);
            SetupRoleTypedServices<TRole>(services);

            var identityBuilder = GetConfiguredIdentityBuilder<TUser, TRole>(services, authSettings);

            return identityBuilder;
        }

        public static IdentityBuilder WithCustomUserAction<TCustomUserAction>(this IdentityBuilder identityBuilder)
            where TCustomUserAction : IAfterUserCreationAction
        {
            var descriptor = new ServiceDescriptor(typeof(IAfterUserCreationAction), typeof(TCustomUserAction), ServiceLifetime.Transient);

            identityBuilder.Services.Replace(descriptor);

            return identityBuilder;
        }

        #endregion


        #region Private methods

        private static AuthorizationSettings GetFromSettingsOrDefault(IConfiguration configuration)
        {
            var authSettings = configuration.GetSection(AUTH_SETINGS_SECTION).Get<AuthorizationSettings>();

            if (authSettings == null)
                authSettings = new AuthorizationSettings();

            return authSettings;
        }

        private static void SetupJwtAuthentication(IServiceCollection services, IConfiguration configuration, AuthorizationSettings authSettings)
        {
            services.AddAuthentication(opt =>
                {
                    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(opt =>
                {
                    opt.SaveToken = true;
                    opt.RequireHttpsMetadata = true;
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidAudience = authSettings.SiteName,
                        ValidIssuer = authSettings.SiteName,
                        IssuerSigningKey = SecurityKeyHelper.GetSecurityKey(configuration)
                    };
                });
        }

        private static IdentityBuilder GetConfiguredIdentityBuilder<TUser, TRole>(IServiceCollection services, AuthorizationSettings authSettings)
            where TUser : User, new()
            where TRole : Role, new()
        {
            return services.AddIdentity<TUser, TRole>(opt =>
                {
                    opt.Password.RequireDigit = authSettings.StrongSecurityPassword;
                    opt.Password.RequireNonAlphanumeric = authSettings.StrongSecurityPassword;
                    opt.Password.RequireLowercase = authSettings.StrongSecurityPassword;
                    opt.Password.RequireUppercase = authSettings.StrongSecurityPassword;
                    opt.Password.RequiredLength = authSettings.MinPasswordLength;

                    opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(authSettings.MinutesToUnlock);
                    opt.Lockout.MaxFailedAccessAttempts = authSettings.MaxAttemptsToLock;
                    opt.Lockout.AllowedForNewUsers = true;

                    opt.User.RequireUniqueEmail = true;
                })
                .AddDefaultTokenProviders();
        }

        private static void SetupTypedServices<TUser, TRole>(IServiceCollection services)
           where TUser : User, new()
           where TRole : Role, new()
        {
            if (typeof(TUser) == typeof(User) && typeof(TRole) == typeof(Role))
            {
                services.AddTransient<IAuthorizationBuilder, AuthorizationBuilder>();
                services.AddTransient<IDefaultUsersLoader, DefaultUsersLoader>();
            }
            else
            {
                services.AddTransient<IAuthorizationBuilder, AuthorizationBuilder<TUser, TRole>>();
                services.AddTransient<IDefaultUsersLoader, DefaultUsersLoader<TUser, TRole>>();
            }
        }

        private static void SetupUserTypedServices<TUser>(IServiceCollection services)
            where TUser : User, new()
        {
            if (typeof(TUser) == typeof(User))
            {
                services.AddTransient<IUserClaimService, UserService>();
                services.AddTransient<IUserRoleService, UserService>();
                services.AddTransient<IUserService, UserService>();
                services.AddTransient<ISessionService, SessionService>();

                services.AddTransient<ITokenHelper, TokenHelper>();

                services.AddTransient<IMapper<User, Credentials>, CredentialsMapper>();
                services.AddTransient<IMapper<User, TokenInfo, Session>, SessionMapper>();
            }
            else
            {
                services.AddTransient<IUserClaimService<TUser>, UserService<TUser>>();
                services.AddTransient<IUserRoleService<TUser>, UserService<TUser>>();
                services.AddTransient<IUserService<TUser>, UserService<TUser>>();
                services.AddTransient<ISessionService, SessionService<TUser>>();

                services.AddTransient<ITokenHelper<TUser>, TokenHelper<TUser>>();

                services.AddTransient<IMapper<TUser, Credentials>, CredentialsMapper<TUser>>();
                services.AddTransient<IMapper<TUser, TokenInfo, Session>, SessionMapper<TUser>>();
            }
        }

        private static void SetupRoleTypedServices<TRole>(IServiceCollection services)
            where TRole : Role, new()
        {
            if (typeof(TRole) == typeof(Role))
            {
                services.AddTransient<IRoleService, RoleService>();
            }
            else
            {
                services.AddTransient<IRoleService<TRole>, RoleService<TRole>>();
            }
        }

        private static void SetDefaultCustomActions(IServiceCollection services)
        {
            services.AddTransient<IAfterUserCreationAction, DefaultCustomUserActionsImplementation>();
        }

        #endregion

    }
}
