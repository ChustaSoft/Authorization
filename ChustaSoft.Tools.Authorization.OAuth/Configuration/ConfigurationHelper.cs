
using IdentityServer4.Models;
using IdentityServer4.Test;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace ChustaSoft.Tools.Authorization
{
    public static class ConfigurationHelper
    {

        public static void WithOAuthProvider(this IdentityBuilder identityBuilder) => WithOAuthProvider(identityBuilder.Services);

        public static void WithOAuthProvider(this IServiceCollection services)
        {
            services
                .AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddTestUsers(GetUsers())
                .AddInMemoryIdentityResources(GetResources())
                .AddInMemoryClients(GetClients());
        }


        public static void UseOAuthProvider(this IApplicationBuilder app) 
        {
            app.UseIdentityServer();
        }



        private static List<TestUser> GetUsers() 
        {
            return new List<TestUser> 
            { 
                new TestUser
                { 
                    SubjectId = Guid.NewGuid().ToString(),
                    Username = "User1",
                    Password = "Test.1234",
                    Claims = new List<Claim>
                    { 
                        new Claim("permission", "test"),
                        new Claim("role", "test-role")
                    }
                },
                new TestUser
                {
                    SubjectId = Guid.NewGuid().ToString(),
                    Username = "User2",
                    Password = "Test.1234",
                    Claims = new List<Claim>
                    {
                        new Claim("permission", "test"),
                        new Claim("role", "test-role")
                    }
                }
            };
        }

        private static IEnumerable<IdentityResource> GetResources() 
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
        }

        private static IEnumerable<Client> GetClients() 
        {
            return new List<Client> { };
        }

    }
}
