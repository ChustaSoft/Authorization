using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ChustaSoft.Tools.Authorization.TestBasic.WebAPI.Controllers
{
    [Controller]
    [Route("web/[controller]")]
    public class SessionController : Controller
    {
        private readonly SignInManager<User> signInManager;

        public SessionController(SignInManager<User> signInManager, UserManager<User> userManager)
        {
            this.signInManager = signInManager;
        }

        [AllowAnonymous]
        [HttpGet("login/{provider}/{redirectUrl}", Name = "web-session-external-login")]
        public async Task<ActionResult> ExternalLogin([FromRoute]string provider, [FromRoute]string redirectUrl)
        {
            string loginCallbackUrl = Url.RouteUrl("web-session-external-login-callback", new { redirectUrl });
            AuthenticationProperties properties = signInManager.ConfigureExternalAuthenticationProperties(provider, loginCallbackUrl);
            
            return Challenge(properties, provider);
        }

        [HttpGet("login/{redirectUrl}/callback", Name = "web-session-external-login-callback")]
        public async Task<ActionResult> ExternalLoginCallback([FromRoute]string redirectUrl)
        {
            //Example of how to retrieve login information:
            //
            //ExternalLoginInfo externalLoginInfo = await signInManager.GetExternalLoginInfoAsync();
            //string username = externalLoginInfo.Principal.FindFirstValue(ClaimTypes.Name);
            //string email = externalLoginInfo.Principal.FindFirstValue(ClaimTypes.Email);

            UriBuilder ub = new UriBuilder("https", redirectUrl);
            return new RedirectResult(ub.ToString());
        }

    }
}
