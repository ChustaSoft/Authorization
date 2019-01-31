using ChustaSoft.Tools.Authorization.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;


namespace ChustaSoft.Tools.Authorization.Services
{
    public interface ISessionService
    {

        Task<Session> AuthenticateAsync(Credentials credentials);

        Task<Session> RegisterAsync(Credentials credentials);

    }
}