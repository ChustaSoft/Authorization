using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading.Tasks;

namespace ChustaSoft.Tools.Authorization.TestOAuth.WebClient.Controllers
{
    [Authorize]
    public class WeatherController : Controller
    {

        private readonly ILogger<WeatherController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public WeatherController(ILogger<WeatherController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var httpClient = _httpClientFactory.CreateClient("APIClient");
            var request = new HttpRequestMessage(HttpMethod.Get, "weatherforecast");
            var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var responseStream = await response.Content.ReadAsByteArrayAsync();

                return View();
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized || response.StatusCode == System.Net.HttpStatusCode.Forbidden) 
            {
                return RedirectToAction("AccessDenied", "Authorization");
            }

            throw new System.Exception("Problem accessing the API");
        }

    }
}
