using ChustaSoft.Tools.Authorization.AspNet;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
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
            services.RegisterAuthorization(_configuration, "d5ab5e3f5799445fb9f68d1fcdda3b9f")
                .WithSqlServerProvider(BuildConnectionString());

            services.AddMvc()
                .AddAuthorizationControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
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

            app.ConfigureAuthorization(env, serviceProvider)
                .SetupDatabase()
                .DefaultUsers(ub => 
                    {
                        ub.Add("SYSTEM", "Sys.1234");
                        ub.Add("ADMIN", "Admn.1234").WithRole("Admin");
                    });
                ;
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
