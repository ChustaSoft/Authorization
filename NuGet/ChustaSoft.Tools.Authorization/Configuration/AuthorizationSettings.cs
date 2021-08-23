using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace ChustaSoft.Tools.Authorization
{
    public class AuthorizationSettings
    {

        public string SiteName { get; set; }
        public int MinutesToExpire { get; set; }
        public int MinutesToUnlock { get; set; }
        public int MaxAttemptsToLock { get; set; }
        public int MinPasswordLength { get; set; }
        public bool StrongSecurityPassword { get; set; }
        public string DefaultCulture { get; set; }
        public bool ConfirmationRequired { get; set; }
        public string DefaultRole { get; set; }
        public IDictionary<ExternalAuthenticationProviders, ExternalAuthenticationSettings> ExternalProviders { get; set; }


        public AuthorizationSettings()
        {
            StrongSecurityPassword = AuthorizationConstants.DEFAULT_STRONG_SECURITY_PASSWORD;
            MinPasswordLength = AuthorizationConstants.DEFAULT_MIN_PASSWORD_LENGTH;
            MinutesToExpire = AuthorizationConstants.DEFAULT_MINUTES_TO_EXPIRE;
            MaxAttemptsToLock = AuthorizationConstants.DEFAULT_MAX_ATTEMPTS_TO_LOCK;
            MinutesToUnlock = AuthorizationConstants.DEFAULT_MINUTES_TO_UNLOCK;
            ExternalProviders = new Dictionary<ExternalAuthenticationProviders, ExternalAuthenticationSettings>();
        }


        internal static AuthorizationSettings GetFromBuilder(Action<AuthorizationSettingsBuilder<AuthorizationSettings>> settingsBuildingAction)
            => GetFromBuilder<AuthorizationSettings, AuthorizationSettingsBuilder<AuthorizationSettings>>(settingsBuildingAction);

        internal static TSettings GetFromBuilder<TSettings, TBuilder>(Action<TBuilder> settingsBuildingAction)
            where TBuilder : AuthorizationSettingsBuilder<TSettings>, new()
            where TSettings : AuthorizationSettings, new()
        {
            var settingsBuilder = new TBuilder();

            settingsBuildingAction.Invoke(settingsBuilder);

            var authSettings = settingsBuilder.Build();
            return authSettings;
        }

        internal static AuthorizationSettings GetFromFile(IConfiguration configuration, string authSectionName)
            => GetFromFile<AuthorizationSettings>(configuration, authSectionName);

        internal static TSettings GetFromFile<TSettings>(IConfiguration configuration, string authSectionName)
            where TSettings : AuthorizationSettings, new()
        {
            var authSettings = configuration.GetSection(authSectionName).Get<TSettings>();

            if (authSettings == null)
                authSettings = new TSettings();

            return authSettings;
        }

    }

}
