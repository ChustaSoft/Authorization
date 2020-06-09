using ChustaSoft.Tools.Authorization.AspNet;
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
        private const string DATABASE_USER = "DbUser";
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
                .IntegrateChustaSoftAuthorization();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, AuthorizationContext authContext)
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

            app.ConfigureAuthorization(env, authContext);
        }

        #endregion


        #region Private methods

        private string BuildConnectionString()
        {
            var builder = new SqlConnectionStringBuilder(_configuration.GetConnectionString(CONNECTIONSTRING_NAME));

            //Just in case of having credentials on environment variables
            //builder.UserID = _configuration[DATABASE_USER];
            //builder.DataSource = _configuration[DATABASE_SERVER];
            //builder.Password = _configuration[SECRET_DATABASE_PASSWORD];

            return builder.ConnectionString;
        }

        #endregion

    }
}
