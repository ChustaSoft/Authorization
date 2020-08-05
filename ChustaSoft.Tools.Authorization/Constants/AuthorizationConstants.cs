namespace ChustaSoft.Tools.Authorization
{
    internal struct AuthorizationConstants
    {

        internal const string SECRET_KEY = "AuthSecretKey";

        internal const int DEFAULT_MIN_PASSWORD_LENGTH = 6;
        internal const int DEFAULT_MINUTES_TO_EXPIRE = 60;
        internal const bool DEFAULT_STRONG_SECURITY_PASSWORD = true;
        internal const int DEFAULT_MINUTES_TO_UNLOCK = 15;
        internal const int DEFAULT_MAX_ATTEMPTS_TO_LOCK = 3;

    }
}
