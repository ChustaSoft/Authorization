using ChustaSoft.Tools.Authorization.Configuration;
using ChustaSoft.Tools.Authorization.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ChustaSoft.Tools.Authorization
{
    public class Startup
    {

        #region Fields

        private readonly IConfiguration _configuration;

        #endregion

        
        #region Constructor

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        #endregion


        #region Public methods

        public void ConfigureServices(IServiceCollection services)
        {
            services.RegisterAuthorization(_configuration, "AuthorizationConnection");

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .IntegrateChustaSoftAuthorization();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, AuthorizationContext authContext)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                builder.AddUserSecrets<Startup>();
            }

            app.ConfigureAuthorization(env, authContext);

            app.UseMvc();
        }

        #endregion

    }
}
