using ChustaSoft.Tools.Authorization.Context;
using ChustaSoft.Tools.Authorization.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

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

        public static IOAuthProviderAuthorizationBuilder WithUserCreatedAction<TUser, TUserCreatedImpl>(this IOAuthProviderAuthorizationBuilder identityBuilder)
            where TUser : User, new()
            where TUserCreatedImpl : class, IUserCreated
        {
            identityBuilder.IdentityBuilder.WithUserCreatedAction<TUser, TUserCreatedImpl>();

            return identityBuilder;
        }

        public static IOAuthProviderAuthorizationBuilder WithUserCreatedActions<TUser, TUserCreatedImpl1, TUserCreatedImpl2>(this IOAuthProviderAuthorizationBuilder identityBuilder)
           where TUser : User, new()
           where TUserCreatedImpl1 : class, IUserCreated
           where TUserCreatedImpl2 : class, IUserCreated
        {
            identityBuilder.IdentityBuilder.WithUserCreatedActions<TUser, TUserCreatedImpl1, TUserCreatedImpl2>();

            return identityBuilder;
        }

        public static IAuthorizationBuilder SetupDatabase(this IApplicationBuilder applicationBuilder, IServiceProvider serviceProvider)
            => applicationBuilder.SetupDatabase<AuthIdentityContext, User, Role>(serviceProvider);

        public static IAuthorizationBuilder SetupDatabase<TAuthContext, TUser, TRole>(this IApplicationBuilder applicationBuilder, IServiceProvider serviceProvider)
            where TAuthContext : AuthorizationContextBase<TUser, TRole>
            where TUser : User, new()
            where TRole : Role, new()
        {
            serviceProvider.GetRequiredService<AuthConfigurationContext>().Database.Migrate();
            serviceProvider.GetRequiredService<AuthOperationContext>().Database.Migrate();
            serviceProvider.GetRequiredService<TAuthContext>().Database.Migrate();

            return serviceProvider.GetRequiredService<IAuthorizationBuilder>();
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
