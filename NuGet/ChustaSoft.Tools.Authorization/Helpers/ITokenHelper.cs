using ChustaSoft.Tools.Authorization.Models;
using System.Collections.Generic;

namespace ChustaSoft.Tools.Authorization
{
    public interface ITokenHelper<TUser>
        where TUser : User, new()
    {

        TokenInfo Generate(TUser user, IEnumerable<string> roles, string privateKey);

    }



    public interface ITokenHelper : ITokenHelper<User> { }

}
