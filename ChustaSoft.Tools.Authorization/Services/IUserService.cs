using ChustaSoft.Tools.Authorization.Models;
using System;
using System.Threading.Tasks;

namespace ChustaSoft.Tools.Authorization.Services
{
    public interface IUserService
    {

        Task<User> GetAsync(Guid userId);

    }
}
