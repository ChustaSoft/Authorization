using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace ChustaSoft.Tools.Authorization.Basic.Handlers
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {

        private readonly Func<string, string, bool> _userCredentialsProvider;


        public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, Func<string, string, bool> userCredentialsProvider)
            : base(options, logger, encoder, clock)
        {
            _userCredentialsProvider = userCredentialsProvider;
        }


        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            return await Task.Run(() => 
            {
                var endpoint = Context.GetEndpoint();
                if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null)
                    return AuthenticateResult.NoResult();

                if (!Request.Headers.ContainsKey("Authorization"))
                    return AuthenticateResult.Fail("Missing Authorization Header");

                bool authResult = false;
                string username, password = string.Empty;
                try
                {
                    var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                    var credentialBytes = Convert.FromBase64String(authHeader.Parameter ?? string.Empty);
                    var credentials = Encoding.UTF8.GetString(credentialBytes).Split(new[] { ':' }, 2);
                    username = credentials[0];
                    password = credentials[1];

                    authResult = _userCredentialsProvider.Invoke(username, password);
                }
                catch
                {
                    return AuthenticateResult.Fail("Invalid Authorization Header");
                }

                if (!authResult)
                    return AuthenticateResult.Fail("Invalid Username or Password");

                var claims = new[] {
                    new Claim(ClaimTypes.Name, username),
                };

                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);

                return AuthenticateResult.Success(ticket);
            });
        }

    }
}
