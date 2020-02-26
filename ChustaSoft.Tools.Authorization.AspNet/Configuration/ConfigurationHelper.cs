using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;


namespace ChustaSoft.Tools.Authorization.AspNet
{
    public static class ConfigurationHelper
    {

        #region Constants

        private const string ASP_ASSEMBLY_NAME = "ChustaSoft.Tools.Authorization.AspNet";

        #endregion


        #region Extension methods

        public static void RegisterAuthorization(this IServiceCollection services, IConfiguration configuration, string connectionString)
        {
            services.RegisterAuthorizationCore(configuration, connectionString);
        }

        public static void ConfigureAuthorization(this IApplicationBuilder app, IWebHostEnvironment env, AuthorizationContext authContext)
        {
            if (!env.EnvironmentName.Equals("dev")) 
            {
                app.UseHsts();
                app.UseHttpsRedirection();
            }

            app.UseRouting();
            app.UseCors();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });

            authContext.Database.Migrate();
        }

        public static IMvcBuilder IntegrateChustaSoftAuthorization(this IMvcBuilder mvcBuilder)
        {
            var assembly = Assembly.Load(ASP_ASSEMBLY_NAME);

            mvcBuilder.AddApplicationPart(assembly).AddControllersAsServices();

            return mvcBuilder;
        }

        #endregion

    }
}
