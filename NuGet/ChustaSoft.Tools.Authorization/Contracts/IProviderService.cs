using Microsoft.AspNetCore.Authentication;

namespace ChustaSoft.Tools.Authorization
{
    public interface IProviderService
    {

        AuthenticationProperties GetExternalProperties(string provider, string loginCallbackUrl);

    }
}
