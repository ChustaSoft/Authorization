using ChustaSoft.Tools.Authorization.AspNet;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;

namespace ChustaSoft.Tools.Authorization.TestBasic.WebAPI
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
            services.RegisterAuthorization("d5ab5e3f5799445fb9f68d1fcdda3b9f", x =>
                {
                    x.SetSiteName("AuthorizationApi");
                })
                .WithSqlServerProvider(_configuration.GetConnectionString(CONNECTIONSTRING_NAME));

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ChustaSoft.Tools.Authorization.TestBasic.WebAPI", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ChustaSoft.Tools.Authorization.TestBasic.WebAPI v1"));
            }

            app.UseRouting();

            app.ConfigureAuthorization(env, serviceProvider, "TestPolicy")
                .SetupDatabase();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
