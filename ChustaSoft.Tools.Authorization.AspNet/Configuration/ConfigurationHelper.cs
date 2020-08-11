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

        public static IAuthorizationBuilder ConfigureAuthorization(this IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (!env.EnvironmentName.Equals("dev"))
            {
                app.UseHsts();
                app.UseHttpsRedirection();
            }
            
            app.UseCors();
            app.UseAuthentication();
            app.UseAuthorization();

            return serviceProvider.GetRequiredService<IAuthorizationBuilder>();
        }

        public static IMvcBuilder AddAuthorizationControllers(this IMvcBuilder mvcBuilder)
        {
            var assembly = Assembly.Load(ASP_ASSEMBLY_NAME);

            mvcBuilder.AddApplicationPart(assembly).AddControllersAsServices();

            return mvcBuilder;
        }

        #endregion

    }
}
