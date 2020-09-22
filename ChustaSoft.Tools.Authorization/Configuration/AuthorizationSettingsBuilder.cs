using ChustaSoft.Common.Contracts;
using ChustaSoft.Common.Utilities;
using System.Collections.Generic;

namespace ChustaSoft.Tools.Authorization
{
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

        public AuthorizationSettingsBuilder AddExternalProvider(ExternalAuthenticationProviders providerName, ExternalAuthenticationProviderSettings externalAuthenticationSettings)
        {
            _authorizationSettings.ExternalProviders.Add(providerName, externalAuthenticationSettings);

            return this;
        }

        public AuthorizationSettingsBuilder AddExternalProvider(ExternalAuthenticationProviders providerName, string clientId, string clientSecret)
        {
            AddExternalProvider(providerName, new ExternalAuthenticationProviderSettings { ClientId = clientId, ClientSecret = clientSecret });

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
        AuthorizationSettingsBuilder AddExternalProvider(ExternalAuthenticationProviders providerName, ExternalAuthenticationProviderSettings externalAuthenticationSettings);
        AuthorizationSettingsBuilder AddExternalProvider(ExternalAuthenticationProviders providerName, string clientId, string clientSecret);
    }

}
