using ChustaSoft.Tools.Authorization.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Security.Cryptography.X509Certificates;

namespace ChustaSoft.Tools.Authorization
{
    public static class ConfigurationHelper
    {

        public static IOAuthProviderAuthorizationBuilder RegisterAuthorization(this IServiceCollection services, IConfiguration configuration, string privateKey, string authSectionName = AuthorizationConstants.AUTH_SETINGS_SECTION)
        {
            var authSettings = AuthorizationSettings.GetFromFile<OAuthorizationSettings>(configuration, authSectionName);
            var identityServerBuilder = services.SetupOAuthProvider(authSettings);
            var identityBuilder = services.RegisterAuthorizationServices<User, Role>(privateKey, authSettings);

            return new OAuthProviderAuthorizationBuilder(identityServerBuilder, identityBuilder);
        }

        public static IOAuthProviderAuthorizationBuilder RegisterAuthorization<TUser, TRole>(this IServiceCollection services, IConfiguration configuration, string privateKey, string authSectionName = AuthorizationConstants.AUTH_SETINGS_SECTION)
            where TUser : User, new()
            where TRole : Role, new()
        {
            var authSettings = AuthorizationSettings.GetFromFile<OAuthorizationSettings>(configuration, authSectionName);
            var identityServerBuilder = services.SetupOAuthProvider(authSettings);
            var identityBuilder = services.RegisterAuthorizationServices<TUser, TRole>(privateKey, authSettings);

            return new OAuthProviderAuthorizationBuilder(identityServerBuilder, identityBuilder);
        }

        public static IOAuthProviderAuthorizationBuilder RegisterAuthorization(this IServiceCollection services, string privateKey, Action<OAuthorizationSettingsBuilder> settingsBuildingAction)
        {
            var authSettings = AuthorizationSettings.GetFromBuilder<OAuthorizationSettings, OAuthorizationSettingsBuilder>(settingsBuildingAction);
            var identityServerBuilder = services.SetupOAuthProvider(authSettings);
            var identityBuilder = services.RegisterAuthorizationServices<User, Role>(privateKey, authSettings);

            return new OAuthProviderAuthorizationBuilder(identityServerBuilder, identityBuilder);
        }

        public static IOAuthProviderAuthorizationBuilder RegisterAuthorization<TUser, TRole>(this IServiceCollection services, string privateKey, Action<OAuthorizationSettingsBuilder> settingsBuildingAction)
            where TUser : User, new()
            where TRole : Role, new()
        {
            var authSettings = AuthorizationSettings.GetFromBuilder<OAuthorizationSettings, OAuthorizationSettingsBuilder>(settingsBuildingAction);
            var identityServerBuilder = services.SetupOAuthProvider(authSettings);
            var identityBuilder = services.RegisterAuthorizationServices<TUser, TRole>(privateKey, authSettings);

            return new OAuthProviderAuthorizationBuilder(identityServerBuilder, identityBuilder);
        }

        public static IApplicationBuilder UseOAuthProvider(this IApplicationBuilder app)
        {
            app.UseIdentityServer();
            app.UseAuthorization();

            return app;
        }


        private static IIdentityServerBuilder SetupOAuthProvider(this IServiceCollection services, OAuthorizationSettings authSettings)
        {
            var builder = services.AddIdentityServer();

            if(string.IsNullOrWhiteSpace(authSettings.ThumbPrint))
                builder.AddDeveloperSigningCredential();
            else
                builder.AddSigningCredential(LoadCertificate(authSettings.ThumbPrint));

            return builder;
        }

        private static X509Certificate2 LoadCertificate(string thumbPrint) 
        {
            using (var store = new X509Store(StoreName.My, StoreLocation.LocalMachine)) 
            {
                store.Open(OpenFlags.ReadOnly);

                var certCollection = store.Certificates.Find(X509FindType.FindByThumbprint, thumbPrint, true);

                if (certCollection.Count == 0)
                    throw new Exception("The specified certificate wasn't found.");

                return certCollection[0];
            }
        }

    }
}
