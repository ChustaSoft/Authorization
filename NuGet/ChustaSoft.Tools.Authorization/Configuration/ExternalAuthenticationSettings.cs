using System.Collections.Generic;

namespace ChustaSoft.Tools.Authorization
{
    public class ExternalAuthenticationSettings
    {
        public IDictionary<ExternalAuthenticationProviders, ExternalAuthenticationProviderSettings> Providers { get; set; }
        public string DefaultRole { get; set; }

        public ExternalAuthenticationSettings()
        {
            Providers = new Dictionary<ExternalAuthenticationProviders, ExternalAuthenticationProviderSettings>();
        }
    }

    public class ExternalAuthenticationProviderSettings
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }

}
