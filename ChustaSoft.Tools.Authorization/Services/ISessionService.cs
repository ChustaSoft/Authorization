using ChustaSoft.Tools.Authorization.Models;
using Microsoft.AspNetCore.Authentication;
using System.Threading.Tasks;

namespace ChustaSoft.Tools.Authorization
{
    public interface ISessionService
    {

        Task<Session> AuthenticateAsync(Credentials credentials);

        Task<Session> RegisterAsync(Credentials credentials);

        Task<Session> ValidateAsync(UserValidation userValidation);

        AuthenticationProperties GetExternalProperties(string provider, string loginCallbackUrl);

    }
}