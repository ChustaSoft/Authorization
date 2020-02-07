namespace ChustaSoft.Tools.Authorization
{
    public class AuthorizationSettings
    {

        public string SiteName { get; set; }

        public int MinutesToExpire { get; set; }

        public int MinPasswordLength { get; set; }

        public bool StrongSecurityPassword { get; set; }

        public string DefaultCulture { get; set; }


        public static AuthorizationSettings GetDefault()
            => new AuthorizationSettings
                {
                    MinPasswordLength = 6,
                    MinutesToExpire = 60,
                    StrongSecurityPassword = true
                };

    }
}
