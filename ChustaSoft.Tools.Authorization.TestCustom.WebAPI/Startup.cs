using ChustaSoft.Tools.Authorization.AspNet;
using ChustaSoft.Tools.Authorization.TestCustom.WebAPI;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Data.SqlClient;


namespace ChustaSoft.Tools.Authorization
{
    public class Startup
    {

        #region Constants

        private const string CONNECTIONSTRING_NAME = "AuthorizationConnection";

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
            services.RegisterAuthorization<AuthCustomContext, CustomUser, CustomRole>(_configuration, BuildConnectionString());

            services.AddMvc()
                .IntegrateChustaSoftAuthorization();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, AuthCustomContext authContext)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            if (env.EnvironmentName.Equals("dev"))
            {
                app.UseDeveloperExceptionPage();
                builder.AddUserSecrets<Startup>();
            }

            app.ConfigureAuthorization<AuthCustomContext, CustomUser, CustomRole>(env, authContext);
        }

        #endregion


        #region Private methods

        private string BuildConnectionString()
        {
            var builder = new SqlConnectionStringBuilder(_configuration.GetConnectionString(CONNECTIONSTRING_NAME));

            return builder.ConnectionString;
        }

        #endregion

    }
}
