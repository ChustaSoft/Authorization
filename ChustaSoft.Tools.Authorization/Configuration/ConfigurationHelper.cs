using ChustaSoft.Common.Contracts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;

namespace ChustaSoft.Tools.Authorization
{
    public static class ConfigurationHelper
    {

        #region Constants

        private const string AUTH_SETINGS_SECTION = "AuthorizationSettings";

        #endregion


        #region Extension methods

        public static void RegisterAuthorizationCore<TAuthContext, TUser, TRole>(this IServiceCollection services, IConfiguration configuration, string connectionString)
            where TAuthContext : AuthorizationContextBase<TUser, TRole>
            where TUser : User, new()
            where TRole : Role, new()
        {
            RegisterDatabase<TAuthContext, TUser, TRole>(services, connectionString);
            RegisterServices<TUser, TRole>(services);
            RegisterIdentityConfigurations<TAuthContext, TUser, TRole>(services, configuration);
        }

        #endregion


        #region Private methods

        private static void RegisterDatabase<TAuthContext, TUser, TRole>(IServiceCollection services, string connectionString)
            where TAuthContext : AuthorizationContextBase<TUser, TRole>
            where TUser : User, new()
            where TRole : Role, new()
        {
            var assemblyName = Assembly.GetAssembly(typeof(TAuthContext)).FullName;

            services.AddDbContext<TAuthContext>(opt => opt.UseSqlServer(connectionString, x => x.MigrationsAssembly(assemblyName)));
        }

        private static void RegisterIdentityConfigurations<TAuthContext, TUser, TRole>(IServiceCollection services, IConfiguration configuration)
            where TAuthContext : AuthorizationContextBase<TUser, TRole>
            where TUser : User, new()
            where TRole : Role, new()
        {
            var authSettings = GetFromSettingsOrDefault(configuration);

            services.AddSingleton(authSettings);

            services.AddIdentity<TUser, TRole>(opt =>
            {
                opt.Password.RequireDigit = authSettings.StrongSecurityPassword;
                opt.Password.RequireNonAlphanumeric = authSettings.StrongSecurityPassword;
                opt.Password.RequireLowercase = authSettings.StrongSecurityPassword;
                opt.Password.RequireUppercase = authSettings.StrongSecurityPassword;
                opt.Password.RequiredLength = authSettings.MinPasswordLength;

                opt.User.RequireUniqueEmail = true;
            })
                .AddEntityFrameworkStores<TAuthContext>()
                .AddDefaultTokenProviders();

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
                        ValidAudience = authSettings.SiteName,
                        ValidIssuer = authSettings.SiteName,
                        IssuerSigningKey = SecurityKeyHelper.GetSecurityKey(configuration)
                    };
                });
        }

        private static AuthorizationSettings GetFromSettingsOrDefault(IConfiguration configuration)
        {
            var authSettings = configuration.GetSection(AUTH_SETINGS_SECTION).Get<AuthorizationSettings>();

            if (authSettings == null)
                authSettings = AuthorizationSettings.GetDefault();

            return authSettings;
        }

        private static void RegisterServices<TUser, TRole>(IServiceCollection services)
            where TUser : User, new()
            where TRole : Role, new()
        {
            services.AddTransient<ICredentialsBusiness, CredentialsBusiness>();

            SetupUserTypedServices<TUser>(services);
            SetupRoleTypedServices<TRole>(services);
        }

        private static void SetupUserTypedServices<TUser>(IServiceCollection services) 
            where TUser : User, new()
        {
            if (typeof(TUser) == typeof(User))
            {
                services.AddTransient<IUserService, UserService>();
                services.AddTransient<ISessionService, SessionService>();

                services.AddTransient<ITokenHelper, TokenHelper>();

                services.AddTransient<IMapper<User, Credentials>, CredentialsMapper>();
                services.AddTransient<IMapper<User, TokenInfo, Session>, SessionMapper>();
            }
            else
            {
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

        #endregion

    }
}
