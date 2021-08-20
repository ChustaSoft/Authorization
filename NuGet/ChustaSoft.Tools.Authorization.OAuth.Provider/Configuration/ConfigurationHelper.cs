using IdentityServer4.Test;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;

namespace ChustaSoft.Tools.Authorization
{
    public static class ConfigurationHelper
    {

        public static IIdentityServerBuilder WithOAuthProvider(this IdentityBuilder identityBuilder) 
            => WithOAuthProvider(identityBuilder.Services);

        public static IIdentityServerBuilder WithOAuthProvider(this IServiceCollection services, string thumbPrint = null)
        {
            return services
                .AddIdentityServer()
                .AddTestUsers(GetUsers())
                .AddDeveloperSigningCredential();
        }

        public static IApplicationBuilder UseOAuthProvider(this IApplicationBuilder app)
        {
            return app.UseIdentityServer();
        }


        private static X509Certificate2 LoadCertificate(string thumbPrint) 
        {
            using (var store = new X509Store(StoreName.My, StoreLocation.LocalMachine)) 
            {
                store.Open(OpenFlags.ReadOnly);

                var certCollection = store.Certificates.Find(X509FindType.FindByThumbprint, thumbPrint, true);

                if (certCollection.Count == 0)
                    throw new Exception("The specified certificate wasn't found.");

                return certCollection[0];
            }
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

      

    }
}
