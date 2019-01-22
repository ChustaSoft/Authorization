using ChustaSoft.Tools.Authorization.Constants;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;


namespace ChustaSoft.Tools.Authorization.Helpers
{
    public static class SecurityKeyHelper
    {

        public static SecurityKey GetSecurityKey(IConfiguration configuration)
        {
            var secretKeyBytes = Encoding.UTF8.GetBytes(configuration[AuthorizationConstants.SECRET_KEY]);
            var signingKey = new SymmetricSecurityKey(secretKeyBytes);

            return signingKey;
        }

    }
}
