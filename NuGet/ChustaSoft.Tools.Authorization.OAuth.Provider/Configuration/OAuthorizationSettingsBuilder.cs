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

    }

}
