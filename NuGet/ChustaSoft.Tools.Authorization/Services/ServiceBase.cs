namespace ChustaSoft.Tools.Authorization
{
    public class ServiceBase
    {

        protected AuthorizationSettings _authorizationSettings;


        protected ServiceBase(AuthorizationSettings authorizationSettings)
        {
            _authorizationSettings = authorizationSettings;
        }

    }
}
