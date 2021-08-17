using Microsoft.AspNetCore.Mvc;

namespace ChustaSoft.Tools.Authorization.TestOAuth.WebClient.Controllers
{
    public class AuthorizationController : Controller
    {
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
