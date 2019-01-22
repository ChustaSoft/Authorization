using ChustaSoft.Tools.Authorization.Models;

namespace ChustaSoft.Tools.Authorization.Services
{
    public interface ITokenService
    {

        TokenInfo Generate(User user);

    }
}
