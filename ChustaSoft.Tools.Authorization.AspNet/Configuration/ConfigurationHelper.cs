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
            services.RegisterAuthorizationCore<AuthorizationContext, User, Role>(configuration, connectionString);
        }

        public static void RegisterAuthorization<TAuthContext, TUser, TRole>(this IServiceCollection services, IConfiguration configuration, string connectionString)
            where TAuthContext : AuthorizationContextBase<TUser, TRole>
            where TUser : User
            where TRole : Role
        {
            services.RegisterAuthorizationCore<TAuthContext, TUser, TRole>(configuration, connectionString);
        }

        public static void ConfigureAuthorization(this IApplicationBuilder app, IWebHostEnvironment env, AuthorizationContext authContext)
        {
            PerformConfiguration<AuthorizationContext, User, Role>(app, env, authContext);
        }

        public static void ConfigureAuthorization<TAuthContext, TUser, TRole>(this IApplicationBuilder app, IWebHostEnvironment env, TAuthContext authContext)
            where TAuthContext : AuthorizationContextBase<TUser, TRole>
            where TUser : User
            where TRole : Role
        {
            PerformConfiguration<TAuthContext, TUser, TRole>(app, env, authContext);
        }

        public static IMvcBuilder IntegrateChustaSoftAuthorization(this IMvcBuilder mvcBuilder)
        {
            var assembly = Assembly.Load(ASP_ASSEMBLY_NAME);

            mvcBuilder.AddApplicationPart(assembly).AddControllersAsServices();

            return mvcBuilder;
        }

        #endregion


        #region Private methods

        private static void PerformConfiguration<TAuthContext, TUser, TRole>(IApplicationBuilder app, IWebHostEnvironment env, TAuthContext authContext)
            where TAuthContext : AuthorizationContextBase<TUser, TRole>
            where TUser : User
            where TRole : Role
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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            authContext.Database.Migrate();
        }

        #endregion

    }
}
