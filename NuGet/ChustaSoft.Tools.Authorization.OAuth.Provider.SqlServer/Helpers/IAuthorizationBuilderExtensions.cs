using ChustaSoft.Tools.Authorization.Context;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;

namespace ChustaSoft.Tools.Authorization
{
    public static class IAuthorizationBuilderExtensions
    {

        public static IAuthorizationBuilder DefaultClients(this IAuthorizationBuilder authorizationBuilder, IEnumerable<Client> clients)
        {
            var dbContext = GetAuthConfigurationContext(authorizationBuilder);

            foreach (var client in clients.Select(x => x.ToEntity()))
                if (!dbContext.Clients.Any(x => x.ClientId == client.ClientId))
                    dbContext.Clients.Add(client);

            dbContext.SaveChanges();

            return authorizationBuilder;
        }


        public static IAuthorizationBuilder DefaultIdentityResources(this IAuthorizationBuilder authorizationBuilder, IEnumerable<IdentityResource> identityResources) 
        {
            var dbContext = GetAuthConfigurationContext(authorizationBuilder);

            foreach (var idResource in identityResources.Select(x => x.ToEntity()))
                if (!dbContext.IdentityResources.Any(x => x.DisplayName == idResource.DisplayName))
                    dbContext.IdentityResources.Add(idResource);

            dbContext.SaveChanges();

            return authorizationBuilder;
        }



        private static AuthConfigurationContext GetAuthConfigurationContext(IAuthorizationBuilder authorizationBuilder)
        {
            return authorizationBuilder.ServiceProvider.GetRequiredService<AuthConfigurationContext>();
        }

    }
}
