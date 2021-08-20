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

            services.WithOAuthProvider().WithSqlServerProvider(_configuration.GetConnectionString(CONNECTIONSTRING_NAME));
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