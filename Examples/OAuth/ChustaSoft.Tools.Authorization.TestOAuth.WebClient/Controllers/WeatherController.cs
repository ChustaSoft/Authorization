using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ChustaSoft.Tools.Authorization.TestOAuth.WebClient.Controllers
{
    [Authorize]
    public class WeatherController : Controller
    {

        private readonly ILogger<WeatherController> _logger;


        public WeatherController(ILogger<WeatherController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

    }
}
