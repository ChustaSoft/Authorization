using ChustaSoft.Tools.Authorization.Enums;
using ChustaSoft.Tools.Authorization.Models;


namespace ChustaSoft.Tools.Authorization.Services
{
    public interface ICredentialsService
    {

        LoginType ValidateCredentials(Credentials credentials);

    }
}
