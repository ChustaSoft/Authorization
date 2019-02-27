using ChustaSoft.Common.Helpers;
using ChustaSoft.Tools.Authorization.Context;
using ChustaSoft.Tools.Authorization.Helpers;
using ChustaSoft.Tools.Authorization.Models;
using ChustaSoft.Tools.Authorization.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Data.SqlClient;
using System.Reflection;


namespace ChustaSoft.Tools.Authorization.Configuration
{
    public static class ConfigurationHelper
    {

        #region Constants

        private const string SECRET_DATABASE_PASSWORD = "DbUserPassword";
        private const string AUTH_SETINGS_SECTION = "AuthorizationSettings";
        private const string CORE_ASSEMBLY_NAME = "ChustaSoft.Tools.Authorization";
        private const string ASP_ASSEMBLY_NAME = "ChustaSoft.Tools.Authorization.AspNet";

        #endregion


        #region Extension methods

        public static void RegisterAuthorization(this IServiceCollection services, IConfiguration configuration, string connectionName)
        {
            var authSettings = GetAuthorizationSettings(services, configuration);

            RegisterDatabase(services, configuration, connectionName);
            RegisterServices(services);
            RegisterIdentityConfigurations(services, configuration, authSettings);
        }

        public static void ConfigureAuthorization(this IApplicationBuilder app, IHostingEnvironment env, AuthorizationContext authContext)
        {
            if (!env.IsDevelopment())
                app.UseHsts();

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseMvc();

            authContext.Database.Migrate();
        }

        public static IMvcBuilder IntegrateChustaSoftAuthorization(this IMvcBuilder mvcBuilder)
        {
            var assembly = Assembly.Load(ASP_ASSEMBLY_NAME);

            mvcBuilder.AddApplicationPart(assembly).AddControllersAsServices();

            return mvcBuilder;
        }

        #endregion


        #region Private methods

        private static void RegisterIdentityConfigurations(IServiceCollection services, IConfiguration configuration, AuthorizationSettings authSettings)
        {
            services.AddIdentity<User, Role>(opt =>
                {
                    opt.Password.RequireDigit = authSettings.StrongSecurityPassword;
                    opt.Password.RequireNonAlphanumeric = authSettings.StrongSecurityPassword;
                    opt.Password.RequireLowercase = authSettings.StrongSecurityPassword;
                    opt.Password.RequireUppercase = authSettings.StrongSecurityPassword;
                    opt.Password.RequiredLength = authSettings.MinPasswordLength;

                    opt.User.RequireUniqueEmail = true;
                })
                .AddEntityFrameworkStores<AuthorizationContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(opt => {
                    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(opt => {
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

        private static void RegisterServices(IServiceCollection services)
        {
            services.AddTransient<ICredentialsService, CredentialsBusiness>();
            services.AddTransient<ISessionService, SessionService>();
            services.AddTransient<ITokenService, TokenService>();
            services.AddTransient<IUserService, UserService>();

            services.AddTransient<IMapper<User, Credentials>, CredentialsMapper>();
            services.AddTransient<IMapper<User, TokenInfo, Session>, SessionMapper>();
        }

        private static AuthorizationSettings GetAuthorizationSettings(IServiceCollection services, IConfiguration configuration)
        {
            var authSettings = GetFromSettingsOrDefault(configuration);

            services.AddSingleton(authSettings);

            return authSettings;
        }

        private static AuthorizationSettings GetFromSettingsOrDefault(IConfiguration configuration)
        {
            var authSettings = configuration.GetSection(AUTH_SETINGS_SECTION).Get<AuthorizationSettings>();

            if (authSettings == null)
                authSettings = AuthorizationSettings.GetDefault();

            return authSettings;
        }

        private static void RegisterDatabase(IServiceCollection services, IConfiguration configuration, string connectionName)
        {
            var connectionString = BuildConnectionString(configuration, connectionName);

            services.AddDbContext<AuthorizationContext>(opt => opt.UseSqlServer(connectionString, x => x.MigrationsAssembly(CORE_ASSEMBLY_NAME)));
        }

        private static string BuildConnectionString(IConfiguration configuration, string connectionName)
        {
            var builder = new SqlConnectionStringBuilder(configuration.GetConnectionString(connectionName));

            builder.Password = configuration[SECRET_DATABASE_PASSWORD];

            return builder.ConnectionString;
        }

        #endregion

    }
}
