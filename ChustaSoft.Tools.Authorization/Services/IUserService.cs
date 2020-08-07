using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChustaSoft.Tools.Authorization
{
    public interface IUserService<TUser>
         where TUser : User, new()
    {

        Task<TUser> GetAsync(Guid userId);

        Task<TUser> GetByUsername(string username, string password);

        Task<TUser> GetByEmail(string email, string password);

        Task<bool> CreateAsync(TUser user, string password, IDictionary<string, string> parameters);

        Task<bool> AssignRoleAsync(TUser user, IEnumerable<string> roleNames);

        Task<bool> ExistAsync(string userEmail);

    }



    public interface IUserService : IUserService<User> { }

}
