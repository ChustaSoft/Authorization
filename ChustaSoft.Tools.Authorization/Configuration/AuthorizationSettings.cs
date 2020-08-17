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

        public List<(ExternalAuthenticationProviders Provider, (string ClientId, string ClientSecret))> ExternalSettings { get; set; }


        public AuthorizationSettings()
        {
            StrongSecurityPassword = AuthorizationConstants.DEFAULT_STRONG_SECURITY_PASSWORD;
            MinPasswordLength = AuthorizationConstants.DEFAULT_MIN_PASSWORD_LENGTH;
            MinutesToExpire = AuthorizationConstants.DEFAULT_MINUTES_TO_EXPIRE;
            MaxAttemptsToLock = AuthorizationConstants.DEFAULT_MAX_ATTEMPTS_TO_LOCK;
            MinutesToUnlock = AuthorizationConstants.DEFAULT_MINUTES_TO_UNLOCK;
        }

    }
}
