using System;
using System.Threading.Tasks;

namespace ChustaSoft.Tools.Authorization
{
    public interface IUserService<TUser>
         where TUser : User, new()
    {

        Task<TUser> GetAsync(Guid userId);

    }



    public interface IUserService : IUserService<User> { }

}
