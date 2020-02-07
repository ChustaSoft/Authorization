namespace ChustaSoft.Tools.Authorization
{
    public interface ITokenBuilder
    {

        TokenInfo Generate(User user);

    }
}
