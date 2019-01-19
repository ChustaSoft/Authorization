namespace ChustaSoft.Tools.Authorization.Configuration
{
    public class AuthorizationSettings
    {

        public int MinutesToExpire { get; set; }

        public int MinPasswordLength { get; set; }

        public bool StrongSecurityPassword { get; set; }


        public static AuthorizationSettings GetDefault()
            => new AuthorizationSettings
                {
                    MinPasswordLength = 6,
                    MinutesToExpire = 60,
                    StrongSecurityPassword = true
                };

    }
}
