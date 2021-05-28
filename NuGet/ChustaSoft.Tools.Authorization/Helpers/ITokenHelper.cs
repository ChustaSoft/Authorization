using ChustaSoft.Tools.Authorization.Models;
using System.Collections.Generic;
using System.Security.Claims;

namespace ChustaSoft.Tools.Authorization
{
    public interface ITokenHelper<TUser>
        where TUser : User, new()
    {

        TokenInfo Generate(TUser user, IEnumerable<string> roles, IEnumerable<Claim> claims, string privateKey);

    }



    public interface ITokenHelper : ITokenHelper<User> { }

}
