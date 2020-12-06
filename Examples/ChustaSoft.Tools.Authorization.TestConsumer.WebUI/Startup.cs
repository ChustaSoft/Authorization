using ChustaSoft.Tools.Authorization.AspNet;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace ChustaSoft.Tools.Authorization.TestConsumer.WebUI
{
    public class Startup
    {

        private const string CONNECTIONSTRING_NAME = "AuthorizationConnection";
        
        private readonly IConfiguration _configuration;

        
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        
        public void ConfigureServices(IServiceCollection services)
        {
            services.RegisterAuthorization(_configuration, "d5ab5e3f5799445fb9f68d1fcdda3b9f")
                .WithSqlServerProvider(_configuration.GetConnectionString(CONNECTIONSTRING_NAME));

            services.AddMvc()
                .AddAuthorizationControllers();

            services.AddControllersWithViews();
            
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            app.UseStaticFiles();



            app.ConfigureAuthorization(env, serviceProvider, "")
                .SetupDatabase()
                .DefaultUsers(ub =>
                {
                    ub.Add("SYSTEM", "Sys.1234");
                    ub.Add("ADMIN", "Admn.1234").WithRole("Admin");
                });
            ;

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
                app.UseSpaStaticFiles();
            }


        }

    }
}
