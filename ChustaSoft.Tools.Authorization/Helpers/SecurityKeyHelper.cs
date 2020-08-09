using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;


namespace ChustaSoft.Tools.Authorization
{
    public static class SecurityKeyHelper
    {

        public static SecurityKey GetSecurityKey(IConfiguration configuration)
        {
            string secretKey = configuration[AuthorizationConstants.SECRET_KEY];

            if (!string.IsNullOrEmpty(secretKey)) 
            { 
                var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);
                return new SymmetricSecurityKey(secretKeyBytes);
            }

            return null;
        }

    }
}
