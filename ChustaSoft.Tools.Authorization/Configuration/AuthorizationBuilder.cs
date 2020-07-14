namespace ChustaSoft.Tools.Authorization.Configuration
{
    public class AuthorizationBuilder : IAuthorizationBuilder
    {

        public string EnvironmentName { get; set; }


        public AuthorizationBuilder(string environmentName)
        {
            EnvironmentName = environmentName;
        }

    }


    public interface IAuthorizationBuilder
    {
        string EnvironmentName { get; set; }
    }

}
