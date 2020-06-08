using System;
using System.Threading.Tasks;

namespace ChustaSoft.Tools.Authorization
{
    public interface IUserService<TUser>
         where TUser : User, new()
    {

        Task<TUser> GetAsync(Guid userId);

        Task<TUser> LoginAsync(Credentials credentials, LoginType loginType);

        Task<bool> CreateAsync(TUser user, string password);

    }



    public interface IUserService : IUserService<User> { }

}
