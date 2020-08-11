using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ChustaSoft.Tools.Authorization
{
    public static class SecurityKeyHelper
    {

        public static SecurityKey GetSecurityKey(string privateKey)
        {
            if (!string.IsNullOrEmpty(privateKey)) 
            { 
                var secretKeyBytes = Encoding.UTF8.GetBytes(privateKey);
                return new SymmetricSecurityKey(secretKeyBytes);
            }

            return null;
        }

    }
}
