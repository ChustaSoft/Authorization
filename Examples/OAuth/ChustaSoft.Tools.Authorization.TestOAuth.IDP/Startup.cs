using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseOAuthProvider().SetupDatabase();
            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
        }
    }
}
