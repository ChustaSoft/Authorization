using ChustaSoft.Tools.Authorization.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace ChustaSoft.Tools.Authorization
{
    public class ProviderService<TUser> : IProviderService
        where TUser : User, new()
    {

        private readonly SignInManager<TUser> _signInManager;


        public ProviderService(SignInManager<TUser> signInManager)
        {
            _signInManager = signInManager;
        }


        public AuthenticationProperties GetExternalProperties(string provider, string loginCallbackUrl)
        {
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, loginCallbackUrl);

            return properties;
        }

    }



    public class ProviderService : ProviderService<User>, IProviderService
    {
        public ProviderService(SignInManager<User> signInManager)
            : base(signInManager)
        { }
    }

}
