using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace ChustaSoft.Tools.Authorization.AspNet
{
    public static class ConfigurationHelper
    {

        #region Constants

        private const string ASP_ASSEMBLY_NAME = "ChustaSoft.Tools.Authorization.AspNet";

        #endregion


        #region Public Extension methods

        /// <summary>
        /// Configuration extension method for main security settings
        /// </summary>
        /// <param name="app">ApplicationBuilder</param>
        /// <param name="env">WebHostEnvironment</param>
        /// <param name="serviceProvider">ServiceProvider for obtaining injected dependencies inside DI container</param>
        /// <param name="corsPolicy">CORS policy name configured</param>
        /// <returns>IAuthorizationBuilder for additional configurations</returns>
        public static IAuthorizationBuilder ConfigureAuthorization(this IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider, string corsPolicy)
        {
            if (!env.EnvironmentName.Equals("dev"))
            {
                app.UseHsts();
                app.UseHttpsRedirection();
            }

            app.UseRouting();
            app.UseCors(corsPolicy);

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            return serviceProvider.GetRequiredService<IAuthorizationBuilder>();
        }

        [Obsolete("This method is no longer needed, just using services.AddControllers() should be enough, will be removed in future releases")]
        public static IMvcBuilder AddAuthorizationControllers(this IMvcBuilder mvcBuilder)
        {
            var assembly = Assembly.Load(ASP_ASSEMBLY_NAME);

            mvcBuilder.AddApplicationPart(assembly).AddControllersAsServices();

            return mvcBuilder;
        }

        #endregion

    }
}
