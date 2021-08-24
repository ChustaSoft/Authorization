using ChustaSoft.Tools.Authorization.Context;
using ChustaSoft.Tools.Authorization.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ChustaSoft.Tools.Authorization
{
    public static class ConfigurationHelper
    {

        public static void WithSqlServerProvider(this IOAuthProviderAuthorizationBuilder identityServerBuilder, string connectionString)
            => identityServerBuilder.WithSqlServerProvider<AuthIdentityContext, User, Role>(connectionString);

        public static IOAuthProviderAuthorizationBuilder WithSqlServerProvider<TAuthContext, TUser, TRole>(this IOAuthProviderAuthorizationBuilder identityServerBuilder, string connectionString)
            where TAuthContext : AuthorizationContextBase<TUser, TRole>
            where TUser : User, new()
            where TRole : Role, new()
        {
            SetupIdentityUserDb<TAuthContext, TUser, TRole>(identityServerBuilder, connectionString);
            SetupIdentityServerDb<TUser>(identityServerBuilder, connectionString);

            return identityServerBuilder;
        }

        public static IAuthorizationBuilder SetupDatabase(this IApplicationBuilder applicationBuilder)
            => applicationBuilder.SetupDatabase<AuthIdentityContext, User, Role>();

        public static IAuthorizationBuilder SetupDatabase<TAuthContext, TUser, TRole>(this IApplicationBuilder applicationBuilder)
            where TAuthContext : AuthorizationContextBase<TUser, TRole>
            where TUser : User, new()
            where TRole : Role, new()
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetRequiredService<AuthConfigurationContext>().Database.Migrate();
                serviceScope.ServiceProvider.GetRequiredService<AuthOperationContext>().Database.Migrate();
                serviceScope.ServiceProvider.GetRequiredService<TAuthContext>().Database.Migrate();
            }

            return applicationBuilder.ApplicationServices.GetRequiredService<IAuthorizationBuilder>();
        }


        private static void SetupIdentityUserDb<TAuthContext, TUser, TRole>(IOAuthProviderAuthorizationBuilder identityServerBuilder, string connectionString)
            where TAuthContext : AuthorizationContextBase<TUser, TRole>
            where TUser : User, new()
            where TRole : Role, new()
        {
            identityServerBuilder.IdentityBuilder.AddIdentityStore<TAuthContext, TUser, TRole>(connectionString);
        }

        private static void SetupIdentityServerDb<TUser>(IOAuthProviderAuthorizationBuilder identityServerBuilder, string connectionString) 
            where TUser : User, new()
        {
            const string MIGRATIONS_ASSEMBLY_NAME = "ChustaSoft.Tools.Authorization.OAuth.Provider.SqlServer";

            identityServerBuilder.ProviderBuilder
                .AddConfigurationStore<AuthConfigurationContext>(opt =>
                {
                    opt.ConfigureDbContext = builder => builder.UseSqlServer(connectionString, opt => opt.MigrationsAssembly(MIGRATIONS_ASSEMBLY_NAME));
                })
                .AddOperationalStore<AuthOperationContext>(opt =>
                {
                    opt.ConfigureDbContext = builder => builder.UseSqlServer(connectionString, opt => opt.MigrationsAssembly(MIGRATIONS_ASSEMBLY_NAME));
                })
                .AddAspNetIdentity<TUser>();
        }

    }
}
