using ChustaSoft.Common.Utilities;
using System.Collections.Generic;

namespace ChustaSoft.Tools.Authorization
{
    public class AuthorizationSettingsBuilder<TSettings>
        where TSettings : AuthorizationSettings, new()
    {

        protected TSettings AuthorizationSettings { get; set; }


        public ICollection<ErrorMessage> Errors { get; set; }


        public AuthorizationSettingsBuilder()
        {
            AuthorizationSettings = new TSettings();
        }


        public AuthorizationSettingsBuilder<TSettings> SetSiteName(string siteName)
        {
            AuthorizationSettings.SiteName = siteName;

            return this;
        }

        public AuthorizationSettingsBuilder<TSettings> SetPasswordSecurity(bool flag, int minLength)
        {
            AuthorizationSettings.StrongSecurityPassword = flag;
            AuthorizationSettings.MinPasswordLength = minLength;

            return this;
        }

        public AuthorizationSettingsBuilder<TSettings> SetConfirmationRequired(bool flag)
        {
            AuthorizationSettings.ConfirmationRequired = flag;
            
            return this;
        }

        public AuthorizationSettingsBuilder<TSettings> SetAccountLock(int maxAttemps, int minutesToUnlock)
        {
            AuthorizationSettings.MaxAttemptsToLock = maxAttemps;
            AuthorizationSettings.MinutesToUnlock = minutesToUnlock;

            return this;
        }

        public AuthorizationSettingsBuilder<TSettings> SetDefaultCulture(string defaultCulture)
        {
            AuthorizationSettings.DefaultCulture = defaultCulture;

            return this;
        }

        public AuthorizationSettingsBuilder<TSettings> SetMinutesToExpire(int minutesToExpire)
        {
            AuthorizationSettings.MinutesToExpire = minutesToExpire;

            return this;
        }

        public AuthorizationSettingsBuilder<TSettings> SetDefaultRole(string defaultRole)
        {
            AuthorizationSettings.DefaultRole = defaultRole;

            return this;
        }

        public AuthorizationSettingsBuilder<TSettings> AddExternalProvider(ExternalAuthenticationProviders providerName, ExternalAuthenticationSettings externalAuthenticationProviderSettings)
        {
            AuthorizationSettings.ExternalProviders.Add(providerName, externalAuthenticationProviderSettings);

            return this;
        }

        public AuthorizationSettingsBuilder<TSettings> AddExternalProvider(ExternalAuthenticationProviders providerName, string clientId, string clientSecret)
        {
            AddExternalProvider(providerName, new ExternalAuthenticationSettings { ClientId = clientId, ClientSecret = clientSecret });

            return this;
        }

        public TSettings Build() => AuthorizationSettings;

    }

}
