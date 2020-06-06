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
            where TUser : User
            where TRole : Role
        {
            RegisterDatabase<TAuthContext, TUser, TRole>(services, connectionString);
            RegisterServices(services);
            RegisterIdentityConfigurations<TAuthContext, TUser, TRole>(services, configuration);
        }

        #endregion


        #region Private methods

        private static void RegisterDatabase<TAuthContext, TUser, TRole>(IServiceCollection services, string connectionString)
            where TAuthContext : AuthorizationContextBase<TUser, TRole>
            where TUser : User
            where TRole : Role
        {
            var assemblyName = Assembly.GetAssembly(typeof(TAuthContext)).FullName;

            services.AddDbContext<TAuthContext>(opt => opt.UseSqlServer(connectionString, x => x.MigrationsAssembly(assemblyName)));
        }

        private static void RegisterIdentityConfigurations<TAuthContext, TUser, TRole>(IServiceCollection services, IConfiguration configuration)
            where TAuthContext : AuthorizationContextBase<TUser, TRole>
            where TUser : User
            where TRole : Role
        {
            var authSettings = GetFromSettingsOrDefault(configuration);

            services.AddSingleton(authSettings);

            services.AddIdentity<User, Role>(opt =>
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

        private static void RegisterServices(IServiceCollection services)
        {
            services.AddTransient<ICredentialsBusiness, CredentialsBusiness>();

            services.AddTransient<ISessionService, SessionService>();
            services.AddTransient<IUserService, UserService>();

            services.AddTransient<ITokenHelper, TokenHelper>();

            services.AddTransient<IMapper<User, Credentials>, CredentialsMapper>();
            services.AddTransient<IMapper<User, TokenInfo, Session>, SessionMapper>();
        }

        #endregion

    }
}
