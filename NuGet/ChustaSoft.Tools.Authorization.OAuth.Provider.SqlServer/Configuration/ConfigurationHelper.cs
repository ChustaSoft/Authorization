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

            identityServerBuilder.AddConfigurationStore<AuthConfigurationContext>(opt => 
                {
                    opt.ConfigureDbContext = builder => builder.UseSqlServer(connectionString, opt => opt.MigrationsAssembly("ChustaSoft.Tools.Authorization.OAuth.Provider.SqlServer"));
                });
        }

        public static IApplicationBuilder SetupDatabase(this IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.ApplicationServices.MigrateDatabase<AuthConfigurationContext>();

            return applicationBuilder;
        }

    }
}
