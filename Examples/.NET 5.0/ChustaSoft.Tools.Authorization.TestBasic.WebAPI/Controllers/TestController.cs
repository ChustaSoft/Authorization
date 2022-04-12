using ChustaSoft.Common.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace ChustaSoft.Tools.Authorization.TestBasic.WebAPI.Controllers
{

    [Authorize]
    [Route("api/[controller]")]
    public class TestController : ApiControllerBase<TestController>
    {

        public TestController(ILogger<TestController> logger)
            : base(logger)
        { }


        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            return await Task.Factory.StartNew(() =>
            {

                _logger.LogInformation("Info requested");

                return Ok(true);
            });
        }

    }
}
