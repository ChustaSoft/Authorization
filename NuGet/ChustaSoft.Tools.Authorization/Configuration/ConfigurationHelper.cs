using ChustaSoft.Tools.Authorization.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChustaSoft.Tools.Authorization
{
    public static class ConfigurationHelper
    {

        public static IdentityBuilder RegisterAuthorizationServices(this IServiceCollection services, string privateKey, AuthorizationSettings authSettings)
            => services.RegisterAuthorizationServices<User, Role>(privateKey, authSettings);

        public static IdentityBuilder RegisterAuthorizationServices<TUser, TRole>(this IServiceCollection services, string privateKey, AuthorizationSettings authSettings)
            where TUser : User, new()
            where TRole : Role, new()
        {
            services.AddSingleton(authSettings);
            services.AddTransient<ISecuritySettings>(x => new SecuritySettings(privateKey));

            SetupTypedServices<TUser, TRole>(services);
            SetupUserTypedServices<TUser>(services);
            SetupRoleTypedServices<TRole>(services);
            SetupExternalProviders(services, authSettings);

            var identityBuilder = GetConfiguredIdentityBuilder<TUser, TRole>(services, authSettings);

            return identityBuilder;
        }

        public static IdentityBuilder WithUserCreatedAction<TUser, TUserCreatedImpl>(this IdentityBuilder identityBuilder)
            where TUserCreatedImpl : class, IUserCreated
            where TUser : User, new()
        {
            identityBuilder.Services.AddTransient<IUserCreated, TUserCreatedImpl>();
            
            identityBuilder.OverrideCurrentUserServiceInContainer<TUser>();

            return identityBuilder;
        }

        public static IdentityBuilder WithUserCreatedActions<TUser, TUserCreatedImpl1, TUserCreatedImpl2>(this IdentityBuilder identityBuilder)
            where TUserCreatedImpl1 : class, IUserCreated
            where TUserCreatedImpl2 : class, IUserCreated
            where TUser : User, new()
        {
            identityBuilder.Services.AddTransient<IUserCreated, TUserCreatedImpl1>();
            identityBuilder.Services.AddTransient<IUserCreated, TUserCreatedImpl2>();

            identityBuilder.OverrideCurrentUserServiceInContainer<TUser>();

            return identityBuilder;
        }


        private static void OverrideCurrentUserServiceInContainer<TUser>(this IdentityBuilder identityBuilder) 
            where TUser : User, new()
        {
            ServiceDescriptor serviceDescriptor = null;
            if (typeof(TUser) == typeof(User))
                serviceDescriptor = new ServiceDescriptor(typeof(IUserService),
                    x => new UserService(x.GetRequiredService<AuthorizationSettings>(), x.GetRequiredService<SignInManager<User>>(), x.GetRequiredService<UserManager<User>>(), GetIUserCreatedEventHandlers(x)),
                    ServiceLifetime.Transient
                );
            else
                serviceDescriptor = new ServiceDescriptor(typeof(IUserService<TUser>),
                    x => new UserService<TUser>(x.GetRequiredService<AuthorizationSettings>(), x.GetRequiredService<SignInManager<TUser>>(), x.GetRequiredService<UserManager<TUser>>(), GetIUserCreatedEventHandlers(x)),
                    ServiceLifetime.Transient
                );

            identityBuilder.Services.Replace(serviceDescriptor);
        }

        private static IEnumerable<EventHandler<UserEventArgs>> GetIUserCreatedEventHandlers(IServiceProvider serviceProvider)
        {
            foreach (var item in serviceProvider.GetServices<IUserCreated>())
                yield return item.DoAfter;
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
                                
                services.AddTransient<IProviderService, ProviderService>();
            }
            else
            {
                services.AddTransient<IUserClaimService<TUser>, UserService<TUser>>();
                services.AddTransient<IUserRoleService<TUser>, UserService<TUser>>();
                services.AddTransient<IUserService<TUser>, UserService<TUser>>();
                
                services.AddTransient<IProviderService, ProviderService<TUser>>();
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

        private static void SetupExternalProviders(IServiceCollection services, AuthorizationSettings authSettings)
        {
            foreach (var providerName in authSettings.ExternalProviders.Keys)
            {
                var providerConfig = authSettings.ExternalProviders[providerName];

                if (!string.IsNullOrEmpty(providerConfig.ClientId) && !string.IsNullOrEmpty(providerConfig.ClientSecret))
                {
                    switch (providerName)
                    {
                        case ExternalAuthenticationProviders.Google:
                            services.AddAuthentication().AddGoogle(opt =>
                            {
                                opt.ClientId = providerConfig.ClientId;
                                opt.ClientSecret = providerConfig.ClientSecret;
                            });
                            break;
                        case ExternalAuthenticationProviders.Microsoft:
                            services.AddAuthentication().AddMicrosoftAccount(opt =>
                            {
                                opt.ClientId = providerConfig.ClientId;
                                opt.ClientSecret = providerConfig.ClientSecret;
                            });
                            break;
                    }
                }
            }
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

                    opt.SignIn.RequireConfirmedAccount = authSettings.ConfirmationRequired;

                    opt.Password.RequiredLength = authSettings.MinPasswordLength;

                    opt.Lockout.AllowedForNewUsers = true;
                    opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(authSettings.MinutesToUnlock);
                    opt.Lockout.MaxFailedAccessAttempts = authSettings.MaxAttemptsToLock;

                    opt.User.RequireUniqueEmail = true;
                })
                .AddDefaultTokenProviders();
        }

    }
}
