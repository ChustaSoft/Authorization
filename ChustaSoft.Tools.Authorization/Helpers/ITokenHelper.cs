namespace ChustaSoft.Tools.Authorization
{
    public interface ITokenHelper
    {

        TokenInfo Generate(User user);

    }
}
