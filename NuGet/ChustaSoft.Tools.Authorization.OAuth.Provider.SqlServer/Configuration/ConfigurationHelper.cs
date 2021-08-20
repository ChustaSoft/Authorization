using ChustaSoft.Tools.Authorization.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ChustaSoft.Tools.Authorization
{
    public static class ConfigurationHelper
    {

        public static void WithSqlServerProvider(this IIdentityServerBuilder identityServerBuilder, string connectionString) 
        {
            const string MIGRATIONS_ASSEMBLY_NAME = "ChustaSoft.Tools.Authorization.OAuth.Provider.SqlServer";

            identityServerBuilder
                .AddConfigurationStore<AuthConfigurationContext>(opt => 
                {
                    opt.ConfigureDbContext = builder => builder.UseSqlServer(connectionString, opt => opt.MigrationsAssembly(MIGRATIONS_ASSEMBLY_NAME));
                })
                .AddOperationalStore<AuthOperationContext>(opt =>
                {
                    opt.ConfigureDbContext = builder => builder.UseSqlServer(connectionString, opt => opt.MigrationsAssembly(MIGRATIONS_ASSEMBLY_NAME));
                });
        }

        public static IApplicationBuilder SetupDatabase(this IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetRequiredService<AuthConfigurationContext>().Database.Migrate();
                serviceScope.ServiceProvider.GetRequiredService<AuthOperationContext>().Database.Migrate();
            }

            return applicationBuilder;
        }

    }
}
