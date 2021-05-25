using ChustaSoft.Common.Base;
using ChustaSoft.Tools.Authorization.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace ChustaSoft.Tools.Authorization.AspNet
{
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

        /// <summary>
        /// Endpoint Login functionality, expecting credentials such as Username, Email or Phone and Password
        /// </summary>
        /// <param name="credentials">Credentials of the user</param>
        /// <returns>Session object, with user logged, token and expiration time</returns>
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

        /// <summary>
        /// Endpoint for Register functionality, expecting credentials for user registrarion. 
        /// Can retrieve parameters also for being re-thrown to Client API through custom actions with IUserCreated
        /// </summary>
        /// <param name="credentials">Credentials of the user</param>
        /// <returns>Session object, with user logged, token and expiration time</returns>
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] ValidableCredentials credentials)
        {
            var actionResponseBuilder = GetEmptyResponseBuilder<ValidableSession>();
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

        /// <summary>
        /// Endpoint for confirming user email or phone, expecting the email/phone and generated confirmation token.
        /// Could be not mandatory if confirmation required is not configured on Startup.
        /// </summary>
        /// <param name="userValidation">Email/Phone and correspondent confirmation token</param>
        /// <returns>Session object, with user logged, token and expiration time</returns>
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

        /// <summary>
        /// Get the token to reset the user password
        /// </summary>
        /// <param name="credentials"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("GenerateResetPasswordToken")]
        public async Task<IActionResult> GenerateResetPasswordToken([FromBody] ResetPasswordCredentials credentials)
        {
            var actionResponseBuilder = GetEmptyResponseBuilder<string>();
            try
            {
                if (ModelState.IsValid)
                {
                    var token = await _sessionService.GenerateResetPasswordTokenAsync(credentials);
                    actionResponseBuilder.SetData(token);
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

        /// <summary>
        /// Reset the user password
        /// </summary>
        /// <param name="credentials">User reset credentials</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCredentials credentials)
        {
            var actionResponseBuilder = GetEmptyResponseBuilder<string>();
            try
            {
                if (ModelState.IsValid)
                {
                    await _sessionService.ResetPasswordAsync(credentials);
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

        /// <summary>
        /// Endpoint for activate or deactivate user, expecting the username, password and activation or deactivation flag
        /// </summary>
        /// <param name="userActivation">Username, password and activation flag</param>
        /// <returns>Username and succeed flag</returns>
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
        [HttpGet("external-login/{provider}/{*redirectUrl}", Name = "web-session-external-login")]
        public IActionResult ExternalLogin([FromRoute] string provider, [FromRoute] string redirectUrl)
        {
            string loginCallbackUrl = Url.RouteUrl("web-session-external-login-callback", new { redirectUrl });

            if (!User.Identity.IsAuthenticated)
            {
                AuthenticationProperties properties = _providerService.GetExternalProperties(provider, loginCallbackUrl);
                return Challenge(properties, provider);
            }
            else
            {
                return Redirect(loginCallbackUrl);
            }
        }

        [HttpGet("external-login/{redirectUrl}/callback", Name = "web-session-external-login-callback")]
        public async Task<IActionResult> ExternalLoginCallback([FromRoute] string redirectUrl)
        {
            if (!User.Identity.IsAuthenticated)
            {
                await _sessionService.AuthenticateExternalAsync();
            }
            return Redirect(Uri.UnescapeDataString(redirectUrl));
        }

        #endregion

    }
}
