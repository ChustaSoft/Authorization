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

        private readonly IUserAuthenticationService _userAuthenticationService;

        #endregion


        #region Constructor

        public AuthorizationController(IUserAuthenticationService userAuthenticationService)
        {
            _userAuthenticationService = userAuthenticationService;
        }

        #endregion


        #region Public methods

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] Credentials credentials)
        {
            try
            {
                var session = _userAuthenticationService.AuthenticateAsync(credentials).Result;

                return Ok(session);
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Credentials credentials)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var session = await _userAuthenticationService.RegisterAsync(credentials);

                    return Ok(session);
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion

    }
}
