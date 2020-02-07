using ChustaSoft.Common.Base;
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

        #endregion

    }
}
