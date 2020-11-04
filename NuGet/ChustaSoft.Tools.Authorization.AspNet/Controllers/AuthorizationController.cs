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
        private readonly IProviderService _providerService;

        #endregion


        #region Constructor

        public AuthorizationController(ILogger<AuthorizationController> logger, ISessionService sessionService, IProviderService providerService)
            : base(logger)
        {
            _sessionService = sessionService;
            _providerService = providerService;
        }

        #endregion


        #region Public methods

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] Credentials credentials)
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
        public async Task<IActionResult> RegisterAsync([FromBody] Credentials credentials)
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
        [HttpPost("confirm")]
        public async Task<IActionResult> ConfirmAsync([FromBody] UserValidation userValidation) 
        {
            var actionResponseBuilder = GetEmptyResponseBuilder<Session>();
            try
            {
                if (ModelState.IsValid)
                {
                    var session = await _sessionService.ValidateAsync(userValidation);
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
        [HttpPost("activate")]
        public async Task<IActionResult> ActivateAsync([FromBody] UserActivation userActivation)
        {
            var actionResponseBuilder = GetEmptyResponseBuilder<string>();
            try
            {
                if (ModelState.IsValid)
                {
                    var operationFlag = await _sessionService.ActivateAsync(userActivation);
                    actionResponseBuilder.SetData(userActivation.Username);
                    actionResponseBuilder.SetStatus(operationFlag);

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
            var properties = _providerService.GetExternalProperties(provider, loginCallbackUrl);

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
