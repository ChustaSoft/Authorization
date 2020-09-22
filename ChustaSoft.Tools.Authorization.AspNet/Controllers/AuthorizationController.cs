using ChustaSoft.Common.Base;
using ChustaSoft.Tools.Authorization.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace ChustaSoft.Tools.Authorization.AspNet
{
    [Authorize]
    [ApiController]
    [Route("api/auth")]
    public class AuthorizationController : ApiControllerBase<AuthorizationController>
    {

        #region Fields

        private readonly ISessionService _sessionService;

        #endregion


        #region Constructor

        public AuthorizationController(ILogger<AuthorizationController> logger, ISessionService sessionService)
            : base(logger)
        {
            _sessionService = sessionService;
        }

        #endregion


        #region Public methods

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Credentials credentials)
        {
            var actionResponseBuilder = GetEmptyResponseBuilder<Session>();
            try
            {
                if (ModelState.IsValid)
                {
                    var session = await _sessionService.AuthenticateAsync(credentials);
                    actionResponseBuilder.SetData(session);

                    return Ok(actionResponseBuilder.Build());
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                actionResponseBuilder.AddError(new Common.Utilities.ErrorMessage(Common.Enums.ErrorType.Invalid, ex.Message));
                return Unauthorized(actionResponseBuilder.Build());
            }
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Credentials credentials)
        {
            var actionResponseBuilder = GetEmptyResponseBuilder<Session>();
            try
            {
                if (ModelState.IsValid)
                {
                    var session = await _sessionService.RegisterAsync(credentials);
                    actionResponseBuilder.SetData(session);

                    return Ok(actionResponseBuilder.Build());
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                actionResponseBuilder.AddError(new Common.Utilities.ErrorMessage(Common.Enums.ErrorType.Invalid, ex.Message));
                return BadRequest(actionResponseBuilder.Build());
            }
        }

        [AllowAnonymous]
        [HttpGet("external-login/{provider}/{redirectUrl}", Name = "web-session-external-login")]
        public IActionResult ExternalLogin([FromRoute]string provider, [FromRoute]string redirectUrl)
        {
            var loginCallbackUrl = Url.RouteUrl("web-session-external-login-callback", new { redirectUrl });
            var properties = _sessionService.GetExternalProperties(provider, loginCallbackUrl);

            return Challenge(properties, provider);
        }

        [HttpGet("external-login/{redirectUrl}/callback", Name = "web-session-external-login-callback")]
        public IActionResult ExternalLoginCallback([FromRoute]string redirectUrl)
        {
            var ub = new UriBuilder("https", redirectUrl);

            return new RedirectResult(ub.ToString());
        }

        #endregion

    }
}
