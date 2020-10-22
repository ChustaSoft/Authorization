using ChustaSoft.Common.Contracts;
using ChustaSoft.Common.Utilities;
using System.Collections.Generic;

namespace ChustaSoft.Tools.Authorization
{
    public class AuthorizationSettingsBuilder : IAuthorizationSettingsBuilder
    {

        private AuthorizationSettings AuthorizationSettings { get; set; }


        public ICollection<ErrorMessage> Errors { get; set; }


        public AuthorizationSettingsBuilder()
        {
            AuthorizationSettings = new AuthorizationSettings();
        }


        public AuthorizationSettingsBuilder SetSiteName(string siteName)
        {
            AuthorizationSettings.SiteName = siteName;

            return this;
        }

        public AuthorizationSettingsBuilder SetPasswordSecurity(bool flag, int minLength)
        {
            AuthorizationSettings.StrongSecurityPassword = flag;
            AuthorizationSettings.MinPasswordLength = minLength;

            return this;
        }

        public AuthorizationSettingsBuilder SetConfirmationRequired(bool flag)
        {
            AuthorizationSettings.ConfirmationRequired = flag;
            
            return this;
        }

        public AuthorizationSettingsBuilder SetAccountLock(int maxAttemps, int minutesToUnlock)
        {
            AuthorizationSettings.MaxAttemptsToLock = maxAttemps;
            AuthorizationSettings.MinutesToUnlock = minutesToUnlock;

            return this;
        }

        public AuthorizationSettingsBuilder SetDefaultCulture(string defaultCulture)
        {
            AuthorizationSettings.DefaultCulture = defaultCulture;

            return this;
        }

        public AuthorizationSettingsBuilder SetMinutesToExpire(int minutesToExpire)
        {
            AuthorizationSettings.MinutesToExpire = minutesToExpire;

            return this;
        }

        public AuthorizationSettingsBuilder AddExternalProvider(ExternalAuthenticationProviders providerName, ExternalAuthenticationProviderSettings externalAuthenticationSettings)
        {
            AuthorizationSettings.ExternalProviders.Add(providerName, externalAuthenticationSettings);

            return this;
        }

        public AuthorizationSettingsBuilder AddExternalProvider(ExternalAuthenticationProviders providerName, string clientId, string clientSecret)
        {
            AddExternalProvider(providerName, new ExternalAuthenticationProviderSettings { ClientId = clientId, ClientSecret = clientSecret });

            return this;
        }

        public AuthorizationSettings Build() => AuthorizationSettings;

    }

    public interface IAuthorizationSettingsBuilder : IBuilder<AuthorizationSettings>
    {
        AuthorizationSettingsBuilder SetAccountLock(int maxAttemps, int minutesToUnlock);
        AuthorizationSettingsBuilder SetDefaultCulture(string defaultCulture);
        AuthorizationSettingsBuilder SetMinutesToExpire(int minutesToExpire);
        AuthorizationSettingsBuilder SetPasswordSecurity(bool flag, int minLength);
        AuthorizationSettingsBuilder SetConfirmationRequired(bool flag);
        AuthorizationSettingsBuilder SetSiteName(string siteName);
        AuthorizationSettingsBuilder AddExternalProvider(ExternalAuthenticationProviders providerName, ExternalAuthenticationProviderSettings externalAuthenticationSettings);
        AuthorizationSettingsBuilder AddExternalProvider(ExternalAuthenticationProviders providerName, string clientId, string clientSecret);
    }

}
