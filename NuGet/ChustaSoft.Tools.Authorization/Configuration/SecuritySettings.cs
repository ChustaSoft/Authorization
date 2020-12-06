namespace ChustaSoft.Tools.Authorization
{

    public class SecuritySettings : ISecuritySettings
    {

        public string PrivateKey { get; private set; }


        public SecuritySettings(string privateKey)
        {
            PrivateKey = privateKey;
        }

    }



    public interface ISecuritySettings 
    {
        string PrivateKey { get; }
    }

}
