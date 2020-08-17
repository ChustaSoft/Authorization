using ChustaSoft.Common.Contracts;
using ChustaSoft.Common.Utilities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChustaSoft.Tools.Authorization
{
    public enum ExternalAuthenticationProviders
    {
        Google,
        Microsoft
    }

    public class AuthorizationSettingsBuilder : IAuthorizationSettingsBuilder
    {

        private AuthorizationSettings _authorizationSettings;


        public ICollection<ErrorMessage> Errors { get; set; }


        public AuthorizationSettingsBuilder()
        {
            _authorizationSettings = new AuthorizationSettings();
        }


        public AuthorizationSettingsBuilder SetSiteName(string siteName)
        {
            _authorizationSettings.SiteName = siteName;

            return this;
        }

        public AuthorizationSettingsBuilder SetPasswordSecurity(bool flag, int minLength)
        {
            _authorizationSettings.StrongSecurityPassword = flag;
            _authorizationSettings.MinPasswordLength = minLength;

            return this;
        }

        public AuthorizationSettingsBuilder SetAccountLock(int maxAttemps, int minutesToUnlock)
        {
            _authorizationSettings.MaxAttemptsToLock = maxAttemps;
            _authorizationSettings.MinutesToUnlock = minutesToUnlock;

            return this;
        }

        public AuthorizationSettingsBuilder SetDefaultCulture(string defaultCulture)
        {
            _authorizationSettings.DefaultCulture = defaultCulture;

            return this;
        }

        public AuthorizationSettingsBuilder SetMinutesToExpire(int minutesToExpire)
        {
            _authorizationSettings.MinutesToExpire = minutesToExpire;

            return this;
        }

        public AuthorizationSettingsBuilder SetExternalAuthorization(IConfiguration configuration)
        {
            var externalProviders = Enum.GetValues(typeof(ExternalAuthenticationProviders));
            
            _authorizationSettings.ExternalSettings = new List<(ExternalAuthenticationProviders Provider, (string ClientId, string ClientSecret))>();

            foreach (ExternalAuthenticationProviders externalProvider in externalProviders)
            {
                string externalProviderName = Enum.GetName(typeof(ExternalAuthenticationProviders), externalProvider);

                if (!string.IsNullOrEmpty(configuration[$"ExternalAuthentication:{externalProviderName}:ClientId"]))
                {
                    _authorizationSettings.ExternalSettings.Add((externalProvider, (configuration[$"ExternalAuthentication:{externalProviderName}:ClientId"], configuration[$"ExternalAuthentication:{externalProviderName}:ClientSecret"])));
                }
            }

            return this;
        }

        public AuthorizationSettings Build() => _authorizationSettings;

    }

    public interface IAuthorizationSettingsBuilder : IBuilder<AuthorizationSettings>
    {
        AuthorizationSettingsBuilder SetAccountLock(int maxAttemps, int minutesToUnlock);
        AuthorizationSettingsBuilder SetDefaultCulture(string defaultCulture);
        AuthorizationSettingsBuilder SetMinutesToExpire(int minutesToExpire);
        AuthorizationSettingsBuilder SetPasswordSecurity(bool flag, int minLength);
        AuthorizationSettingsBuilder SetSiteName(string siteName);
        AuthorizationSettingsBuilder SetExternalAuthorization(IConfiguration configuration);

    }

}
