using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ChustaSoft.Tools.Authorization.AspNet
{
    public static class ConfigurationHelper
    {

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

            app.UseCors(corsPolicy);

            app.UseAuthentication();
            app.UseAuthorization();

            return serviceProvider.GetRequiredService<IAuthorizationBuilder>();
        }

    }
}
