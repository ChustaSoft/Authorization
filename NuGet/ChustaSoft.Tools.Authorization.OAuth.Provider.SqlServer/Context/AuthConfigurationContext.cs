using ChustaSoft.Tools.Authorization.Constants;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;

namespace ChustaSoft.Tools.Authorization.Context
{
    public class AuthConfigurationContext : ConfigurationDbContext<AuthConfigurationContext>
    {

        public AuthConfigurationContext(DbContextOptions<AuthConfigurationContext> options, ConfigurationStoreOptions storeOptions) 
            : base(options, storeOptions)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema(AuthDbConstants.SCHEMA_NAME);
        }
    }
}