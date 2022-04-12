using ChustaSoft.Tools.Authorization.AspNet;
using ChustaSoft.Tools.Authorization.TestCustom.WebAPI;
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
            /*
             * Example with Builder:
             */
            //services.RegisterAuthorization<CustomUser, CustomRole>("d5ab5e3f5799445fb9f68d1fcdda3b9f", x =>
            //    {
            //        x.SetSiteName("AuthorizationApi");
            //        x.AddExternalProvider(ExternalAuthenticationProviders.Google, "[ClientIdParam]", "[ClientSecretParam]");
            //    })
            //    .WithUserCreatedAction<CustomUser, CustomUserAction>()
            //    .WithSqlServerProvider<AuthCustomContext, CustomUser, CustomRole>(BuildConnectionString());


            /*
            * Example with appSettings:
            */

            services.RegisterAuthorization<CustomUser, CustomRole>(_configuration, "d5ab5e3f5799445fb9f68d1fcdda3b9f")
                //.WithUserCreatedAction<CustomUser, CustomUserAction>() // For single action
                .WithUserCreatedActions<CustomUser, CustomUserActionA, CustomUserActionB>() //For multiple actions
                .WithSqlServerProvider<AuthCustomContext, CustomUser, CustomRole>(BuildConnectionString());

            services.AddControllers();

            services.AddCors(o => o.AddPolicy("IdentityPolicy", builder =>
            {
                builder.WithOrigins("http://localhost:4200")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            }));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            app.UseStaticFiles();
            app.UseRouting();

            app.ConfigureAuthorization(env, serviceProvider, "IdentityPolicy")
                .SetupDatabase<AuthCustomContext, CustomUser, CustomRole>()
                .DefaultUsers(ub =>
                {
                    ub.Add("SYSTEM", "Sys.1234").WithFullAccess();
                    ub.Add("ADMIN", "Admn.1234").WithRole("Admin").WithFullAccess();
                });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
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
