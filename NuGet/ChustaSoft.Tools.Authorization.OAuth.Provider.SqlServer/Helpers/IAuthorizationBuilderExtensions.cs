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
            var dbContext = authorizationBuilder.ServiceProvider.GetRequiredService<AuthConfigurationContext>();

            foreach (var client in clients.Select(x => x.ToEntity()))            
                if (!dbContext.Clients.Any(x => x.ClientId == client.ClientId))                
                    dbContext.Clients.Add(client);

            dbContext.SaveChanges();

            return authorizationBuilder;
        }
    }
}
