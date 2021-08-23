namespace ChustaSoft.Tools.Authorization
{
    internal struct AuthorizationConstants
    {

        internal const int DEFAULT_MIN_PASSWORD_LENGTH = 6;
        internal const int DEFAULT_MINUTES_TO_EXPIRE = 60;
        internal const bool DEFAULT_STRONG_SECURITY_PASSWORD = true;
        internal const bool DEFAULT_CONFIRMATION_REQUIRED = false;
        internal const int DEFAULT_MINUTES_TO_UNLOCK = 15;
        internal const int DEFAULT_MAX_ATTEMPTS_TO_LOCK = 3;
        internal const string DEFAULT_CULTURE = "en-UK";

        internal const string CLAIM_PERMISSION_KEY = "PERMISSION";
        
        internal const string NO_EMAIL_SUFFIX_FORMAT = ".noemail@no-reply.com";

        internal const string AUTH_SETINGS_SECTION = "Authorization";

    }
}
