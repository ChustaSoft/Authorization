using ChustaSoft.Tools.Authorization.Models;
using System.Threading.Tasks;


namespace ChustaSoft.Tools.Authorization
{
    public interface ISessionService
    {

        Task<Session> AuthenticateAsync(Credentials credentials);

        Task<Session> RegisterAsync(Credentials credentials);

    }
}