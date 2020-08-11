using ChustaSoft.Tools.Authorization.Models;

namespace ChustaSoft.Tools.Authorization
{
    public interface ICredentialsBusiness
    {

        LoginType ValidateCredentials(Credentials credentials);

    }
}
