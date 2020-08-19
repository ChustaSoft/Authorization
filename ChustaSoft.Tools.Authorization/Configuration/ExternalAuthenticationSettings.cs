namespace ChustaSoft.Tools.Authorization
{
    public class ExternalAuthenticationSettings
    {
        public ExternalAuthenticationProviders ProviderName { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }

}
