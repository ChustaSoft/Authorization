using ChustaSoft.Common.Helpers;
using ChustaSoft.Tools.Authorization.Models;
using ChustaSoft.Tools.Authorization.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;


namespace ChustaSoft.Tools.Authorization.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/auth")]
    public class AuthorizationController : ControllerBase
    {

        #region Fields

        private readonly ISessionService _sessionService;

        #endregion


        #region Constructor

        public AuthorizationController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }

        #endregion


        #region Public methods

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] Credentials credentials)
        {
            var actionResponseBuilder = new ActionResponseBuilder<Session>();
            try
            {
                if (ModelState.IsValid)
                {
                    var session = _sessionService.AuthenticateAsync(credentials).Result;
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
            var actionResponseBuilder = new ActionResponseBuilder<Session>();
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
