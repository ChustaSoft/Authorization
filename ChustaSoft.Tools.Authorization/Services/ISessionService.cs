using ChustaSoft.Tools.Authorization.Models;
using Microsoft.AspNetCore.Authentication;
using System.Threading.Tasks;


namespace ChustaSoft.Tools.Authorization
{
    public interface ISessionService
    {

        Task<Session> AuthenticateAsync(Credentials credentials);

        Task<Session> RegisterAsync(Credentials credentials);

        AuthenticationProperties BuildAuthenticationProperties(string provider, string loginCallbackUrl);


    }
}