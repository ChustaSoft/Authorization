using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;

namespace ChustaSoft.Tools.Authorization.TestOAuth.IDP
{
    public class Startup
    {

        private const string CONNECTIONSTRING_NAME = "AuthorizationConnection";


        public readonly IConfiguration _configuration;


        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options => options.EnableEndpointRouting = false);

            services.RegisterAuthorization("d5ab5e3f5799445fb9f68d1fcdda3b9f", x =>
                {
                    x.SetSiteName("AuthorizationApi");
                })
                .WithSqlServerProvider(_configuration.GetConnectionString(CONNECTIONSTRING_NAME));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.ConfigureOAuthProvider()
                .SetupDatabase(serviceProvider)
                .DefaultUsers(ub =>
                {
                    ub.Add("TestUser", "Test.Pa$w0rd").WithFullAccess().WithRole("Admin");
                })
                .DefaultClients(new List<Client>
                {
                    new Client
                    {
                        UpdateAccessTokenClaimsOnRefresh = true,
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
                })
                .DefaultIdentityResources(new List<IdentityResource>
                {
                    new IdentityResources.OpenId(),
                    new IdentityResources.Profile()
                });

            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
        }
    }
}
