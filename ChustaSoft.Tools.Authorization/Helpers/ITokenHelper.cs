using ChustaSoft.Tools.Authorization.Models;

namespace ChustaSoft.Tools.Authorization
{
    public interface ITokenHelper<TUser>
        where TUser : User, new()
    {

        TokenInfo Generate(TUser user);

    }



    public interface ITokenHelper : ITokenHelper<User> { }

}
