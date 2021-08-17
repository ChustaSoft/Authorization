using IdentityServer4;
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
                .AddInMemoryApiResources(GetApiResources())
                .AddInMemoryApiScopes(GetApiScopes())
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


        private static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("client-test-web_api", "Test Client REST WebAPI", new[] { "role" })
                    {
                        Scopes = { "client-test-web_api"},
                        ApiSecrets = { new Secret("secret_api".Sha256()) }
                    }
            };
        }

        private static IEnumerable<ApiScope> GetApiScopes()
        {
            return new List<ApiScope>
            {
                new ApiScope("roles"),
                new ApiScope("client-test-web_api", "Test Client REST WebAPI")
            };
        }

        private static IEnumerable<Client> GetClients() 
        {
            return new List<Client> 
            {
                new Client
                { 
                    ClientName = "Client Test",
                    ClientId = "client-test-web_ui",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RedirectUris = new List<string>()
                    {
                        "https://localhost:44392/signin-oidc"
                    },
                    PostLogoutRedirectUris = new List<string>()
                    {
                        "https://localhost:44392/signout-callback-oidc"
                    },
                    AllowedScopes = { 
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "roles",
                        "client-test-web_api" 
                    },                    
                    ClientSecrets = { new Secret("secret".Sha256()) }
                }
            };
        }

    }
}
