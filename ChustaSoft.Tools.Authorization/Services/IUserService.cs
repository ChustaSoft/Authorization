using System;
using System.Threading.Tasks;

namespace ChustaSoft.Tools.Authorization
{
    public interface IUserService
    {

        Task<User> GetAsync(Guid userId);

    }
}
