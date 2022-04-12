namespace ChustaSoft.Tools.Authorization
{
    public class OAuthorizationSettingsBuilder : AuthorizationSettingsBuilder<OAuthorizationSettings>
    {

        public OAuthorizationSettingsBuilder()
            : base()
        { }


        public OAuthorizationSettingsBuilder SetThumbPrint(string thumbPrint)
        {
            AuthorizationSettings.ThumbPrint = thumbPrint;

            return this;
        }

        public new OAuthorizationSettingsBuilder SetSiteName(string siteName)
            => (OAuthorizationSettingsBuilder)base.SetSiteName(siteName);

        public new OAuthorizationSettingsBuilder SetPasswordSecurity(bool flag, int minLength)
            => (OAuthorizationSettingsBuilder)base.SetPasswordSecurity(flag, minLength);

        public new OAuthorizationSettingsBuilder SetConfirmationRequired(bool flag)
            => (OAuthorizationSettingsBuilder)base.SetConfirmationRequired(flag);

        public new OAuthorizationSettingsBuilder SetAccountLock(int maxAttemps, int minutesToUnlock)
            => (OAuthorizationSettingsBuilder)base.SetAccountLock(maxAttemps, minutesToUnlock);

        public new OAuthorizationSettingsBuilder SetDefaultCulture(string defaultCulture)
            => (OAuthorizationSettingsBuilder)base.SetDefaultCulture(defaultCulture);

        public new OAuthorizationSettingsBuilder SetMinutesToExpire(int minutesToExpire)
            => (OAuthorizationSettingsBuilder)base.SetMinutesToExpire(minutesToExpire);

        public new OAuthorizationSettingsBuilder SetDefaultRole(string defaultRole)
            => (OAuthorizationSettingsBuilder)base.SetDefaultRole(defaultRole);

        public new OAuthorizationSettingsBuilder AddExternalProvider(ExternalAuthenticationProviders providerName, ExternalAuthenticationSettings externalAuthenticationProviderSettings)
            => (OAuthorizationSettingsBuilder)base.AddExternalProvider(providerName, externalAuthenticationProviderSettings);

        public new OAuthorizationSettingsBuilder AddExternalProvider(ExternalAuthenticationProviders providerName, string clientId, string clientSecret)
            => (OAuthorizationSettingsBuilder)base.AddExternalProvider(providerName, clientId, clientSecret);

    }

}
