using ChustaSoft.Tools.Authorization.Configuration;
using ChustaSoft.Tools.Authorization.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Data.SqlClient;


namespace ChustaSoft.Tools.Authorization
{
    public class Startup
    {

        #region Constants

        private const string CONNECTIONSTRING_NAME = "AuthorizationConnection";
        private const string DATABASE_SERVER = "DbServer";
        private const string SECRET_DATABASE_PASSWORD = "DbUserPassword";

        #endregion


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
            services.RegisterAuthorization(_configuration, BuildConnectionString());

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


        #region Private methods

        private string BuildConnectionString()
        {
            var builder = new SqlConnectionStringBuilder(_configuration.GetConnectionString(CONNECTIONSTRING_NAME));

            builder.DataSource = _configuration[DATABASE_SERVER];
            builder.Password = _configuration[SECRET_DATABASE_PASSWORD];

            return builder.ConnectionString;
        }

        #endregion

    }
}
