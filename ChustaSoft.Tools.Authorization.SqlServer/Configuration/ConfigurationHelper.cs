using ChustaSoft.Tools.Authorization.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ChustaSoft.Tools.Authorization
{
    public static class ConfigurationHelper
    {

        #region Public Extension methods

        public static void WithSqlServerProvider(this IdentityBuilder identityBuilder, string connectionString) 
        {
            PerformSqlServerRegistration<AuthorizationContext, User, Role>(identityBuilder, connectionString);
        }

        public static void WithSqlServerProvider<TAuthContext, TUser, TRole>(this IdentityBuilder identityBuilder, string connectionString)
            where TAuthContext : AuthorizationContextBase<TUser, TRole>
            where TUser : User, new()
            where TRole : Role, new()
        {
            PerformSqlServerRegistration<TAuthContext, TUser, TRole>(identityBuilder, connectionString);
        }

        public static IAuthorizationBuilder SetupDatabase(this IAuthorizationBuilder authorizationBuilder)
        {
            return authorizationBuilder.SetupDatabase<AuthorizationContext, User, Role>();
        }

        public static IAuthorizationBuilder SetupDatabase<TAuthContext, TUser, TRole>(this IAuthorizationBuilder authorizationBuilder)
            where TAuthContext : AuthorizationContextBase<TUser, TRole>
            where TUser : User, new()
            where TRole : Role, new()
        {
            var authorizationContext = authorizationBuilder.ServiceProvider.GetRequiredService<TAuthContext>();

            authorizationContext.Database.Migrate();

            return authorizationBuilder;
        }

        #endregion


        #region Private methods

        private static void PerformSqlServerRegistration<TAuthContext, TUser, TRole>(IdentityBuilder identityBuilder, string connectionString)
            where TAuthContext : AuthorizationContextBase<TUser, TRole>
            where TUser : User, new()
            where TRole : Role, new()
        {
            var assemblyName = Assembly.GetAssembly(typeof(TAuthContext)).FullName;

            identityBuilder.Services.AddDbContext<TAuthContext>(opt => opt.UseSqlServer(connectionString, x => x.MigrationsAssembly(assemblyName)));
            identityBuilder.AddEntityFrameworkStores<TAuthContext>();
        }

        #endregion

    }
}
